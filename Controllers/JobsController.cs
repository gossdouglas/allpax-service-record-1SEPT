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
    public class jobsController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        public ActionResult Index()
        {
            List<tbl_Jobs> jobs = new List<tbl_Jobs>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_Jobs.jobID, tbl_Jobs.description, tbl_Jobs.customerCode, tbl_Jobs.customerContact, tbl_Jobs.active, tbl_Jobs.location, tbl_Jobs.nrmlHoursStart, " +
                "tbl_Jobs.nrmlHoursEnd, tbl_Jobs.nrmlHoursDaily, tbl_Jobs.dblTimeHours, tbl_Jobs.id " +
                "FROM tbl_Jobs";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                tbl_Jobs job = new tbl_Jobs();

                job.jobID = dr1[0].ToString();
                job.description = dr1[1].ToString();
                job.customerCode = dr1[2].ToString();
                job.customerContact = dr1[3].ToString();
                job.active = (Boolean)dr1[4];
                job.location = dr1[5].ToString();
                job.nrmlHoursStart = dr1[6].ToString();
                job.nrmlHoursEnd = dr1[7].ToString();
                job.nrmlHoursDaily = dr1[8].ToString();
                job.dblTimeHours = (Boolean)dr1[9];
                job.id = (int)dr1[10];
                jobs.Add(job);
            }
            sqlconn.Close();
            return View(jobs);
        }

        //public ActionResult UpdateJob(tbl_Jobs customerUpdate)
        //{
        //    db.Database.ExecuteSqlCommand(
        //        "UPDATE tbl_Jobs " +
        //        "SET " +

        //        "customerCode = {0}, " +
        //        "customerName = {1}, " +
        //        "address = {2} " +

        //        "WHERE id = {3}",
        //        customerUpdate.customerCode, customerUpdate.customerName, customerUpdate.address, customerUpdate.id);

        //    return Json(Url.Action("Index", "Customers"));
        //}

        //[HttpPost]
        //public ActionResult AddJob(tbl_Jobs customerAdd)
        //{
        //    db.Database.ExecuteSqlCommand("Insert into tbl_Jobs Values({0},{1},{2})",
        //       customerAdd.customerCode, customerAdd.customerName, customerAdd.address);

        //    return Json(Url.Action("Index", "Customers"));
        //}

        //public ActionResult DeleteCustomer(tbl_Jobs customerDelete)
        //{
        //    db.Database.ExecuteSqlCommand("DELETE FROM tbl_Jobs WHERE customerCode=({0})", customerDelete.customerCode);

        //    return Json(Url.Action("Index", "Customers"));
        //}

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
