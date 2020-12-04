using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_service_record.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace allpax_service_record.Controllers
{
    public class customersController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        public ActionResult Index()
        {
            List<tbl_customers> customers = new List<tbl_customers>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_customers.customerCode, tbl_customers.customerName, tbl_customers.address, tbl_customers.id " + 
                "FROM tbl_customers";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                tbl_customers customer = new tbl_customers();

                customer.customerCode = dr1[0].ToString();
                customer.customerName = dr1[1].ToString();
                customer.address = dr1[2].ToString();
                customer.id = (int)dr1[3];
                customers.Add(customer);
            }
            sqlconn.Close();
            return View(customers);
        }

        public ActionResult UpdateCustomer(tbl_customers customerUpdate)
        {
            db.Database.ExecuteSqlCommand(
                "UPDATE tbl_customers " +
                "SET " +

                "customerCode = {0}, " +
                "customerName = {1}, " +
                "address = {2} " +

                "WHERE id = {3}",
                customerUpdate.customerCode, customerUpdate.customerName, customerUpdate.address, customerUpdate.id);

            return Json(Url.Action("Index", "Customers"));
        }

        [HttpPost]
        public ActionResult AddCustomer(tbl_customers customerAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into tbl_customers Values({0},{1},{2})",
               customerAdd.customerCode, customerAdd.customerName, customerAdd.address);

            return Json(Url.Action("Index", "Customers"));
        }

        public ActionResult DeleteCustomer(tbl_customers customerDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tbl_customers WHERE customerCode=({0})", customerDelete.customerCode);

            return Json(Url.Action("Index", "Customers"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
