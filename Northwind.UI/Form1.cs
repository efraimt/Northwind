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
            using (var db  =new NorthwindContext())
            {
                //var list = db.Customers.ToList();
                //var res1 = db.Customers.Where(c => c.Country.ToLower() == "mexico").ToList();
                ////var res2 = db.Customers.ToList().Where(c => c.Country == "Mexico".ToLower()).ToList();

                //var count = db.Customers.Count();
                //var avg = db.Products.Average(p => p.UnitPrice);

                //var qur = from p in db.Products
                //          where p.UnitPrice > 500
                //          select p;

                //foreach (var item in qur)
                //{
                //    Debug.WriteLine(item.ProductName + " " + item.UnitPrice);
                //}

                var prd8 = db.Products.Find(8);
                
                var prd8_Qur = db.Products.Include(p=> p.Supplier).Include(p=> p.OrderDetails);
                var prd8_1 = await prd8_Qur.FirstOrDefaultAsync(p=>p.ProductId==8);
                Console.WriteLine(prd8.Supplier.CompanyName);
                prd8.OrderDetails = db.OrderDetails.Where(od=>od.ProductId==8 && od.Quantity == 5 ).ToList(); 

                var p8_2  =await db.Products.FirstOrDefaultAsync(p => p.ProductId == 8);


                var customer = db.Customers.ToList();
                Debug.WriteLine(customer[0].Orders.First().Employee.FirstName);

                var ordersQur = db.Orders.Include(o => o.Customer).ThenInclude(c=>c.CustomerCustomerDemos);
                ordersQur.First();

                dataGridView1.DataSource= ordersQur.ToList();

            }
        }
    }
}
