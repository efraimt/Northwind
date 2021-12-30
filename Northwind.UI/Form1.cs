using Microsoft.EntityFrameworkCore;
using Northwind.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var db = new NorthwindContext())
            {
                var list = db.Customers.ToList();
                var res1 = db.Customers.Where(c => c.Country.ToLower() == "mexico").ToList();
                var res2 = db.Customers.ToList().Where(c => c.Country == "Mexico".ToLower()).ToList();

                var count = db.Customers.Count();
                var avg = db.Products.Average(p => p.UnitPrice);

                var qur = from p in db.Products
                          where p.UnitPrice > 500
                          select p;

                foreach (var item in qur)
                {
                    Debug.WriteLine(item.ProductName + " " + item.UnitPrice);
                }

                var prd8 = db.Products.Find(8);

                var prd8_Qur = db.Products.Include(p => p.Supplier).Include(p => p.OrderDetails);
                var prd8_1 = await prd8_Qur.FirstOrDefaultAsync(p => p.ProductId == 8);
                Console.WriteLine(prd8.Supplier.CompanyName);
                prd8.OrderDetails = db.OrderDetails.Where(od => od.ProductId == 8 && od.Quantity == 5).ToList();

                var p8_2 = await db.Products.FirstOrDefaultAsync(p => p.ProductId == 8);


                var customer = db.Customers.ToList();
                Debug.WriteLine(customer[0].Orders.First().Employee.FirstName);

                var ordersQur = db.Orders
                    .Include(o => o.Customer)
                    .ThenInclude(c => c.CustomerCustomerDemos);
                ordersQur.First();

                dataGridView1.DataSource = ordersQur.ToList();
                Order order = await db.Orders.FirstAsync();
                var details = order.OrderDetails;
                order.OrderDetails = db.OrderDetails.Where(od => od.OrderId == order.OrderId).ToList();

                var query = db.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Where(o => o.CustomerId == "5");
                var sqlStr = query.ToQueryString();



            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Turn off lazy loading in the context.OnConfiguring
            using (var ctx = new NorthwindContext())
            {
                var supliersList = ctx.Suppliers.Include(s => s.Products).ToList();
                foreach (var suplier in supliersList)
                    Debug.WriteLine(suplier.Products.Count());

                var productList = ctx.Products.Include(p => p.Category).ToList();
                foreach (var p in productList)
                    Debug.WriteLine(p.Category.CategoryName);

                Debug.WriteLine(ctx.Suppliers.Where(s => s.City == "mexico").ToQueryString());
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            //Turn on lazy loading in the context.OnConfiguring
            using (var ctx = new NorthwindContext())
            {
                var list = await ctx.Orders
                    .OrderByDescending(o => o.OrderId)
                    .Take(2)
                    .ToListAsync();
                foreach (var item in list)
                {
                    try
                    {
                        Debug.WriteLine(item.Customer.CompanyName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("//Turn on lazy loading in the context.OnConfiguring");
                    }
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var customer = new Customer() { CustomerId = "ABCD", CompanyName = "Quze" };
            using (var db = new NorthwindContext())
            {
                db.Customers.Add(customer);
                db.SaveChanges();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var db = new NorthwindContext())
            {
                var customer = db.Customers.Find("ABCD");
                customer.CompanyName = "Tralalal";
                db.SaveChanges();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (var db = new NorthwindContext())
            {
                var customers = db.Customers.ToList();
                var customer = customers.Where(c => c.CustomerId.Contains("ABCD")).First();
                foreach (var item in customers)
                {
                    Debug.WriteLine(item.CustomerId);
                }
                db.Customers.Remove(customer);

                db.SaveChanges();
            }
        }

        NorthwindContext northwindContext = new NorthwindContext();
        int index = 0;
        private void button9_Click(object sender, EventArgs e)
        {
            var customer = new Customer() { CustomerId = "YAKI"+ index++.ToString(), CompanyName = "Stam" + index.ToString() };
            northwindContext.Customers.Add(customer);

            Debug.WriteLine(northwindContext.Entry(customer).State);

            foreach (var item in northwindContext.Customers)
            {
                northwindContext.Entry(item).State = EntityState.Unchanged;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var list = northwindContext.Customers.ToList();
            list[index].CompanyName+= index.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            northwindContext.SaveChanges();
        }
    }
}
