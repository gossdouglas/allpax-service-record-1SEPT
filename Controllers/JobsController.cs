using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_service_record.Models;
using allpax_service_record.Models.View_Models;
using System.Configuration;
using System.Data.SqlClient;

namespace allpax_service_record.Controllers
{
    public class jobsController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        public ActionResult Index()
        {
            //List<tbl_Jobs> jobs = new List<tbl_Jobs>();
            //List<vm_Jobs> jobs = new List<vm_Jobs>();
            List<vm_Jobs> jobs = new List<vm_Jobs>();
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
                //tbl_Jobs job = new tbl_Jobs();
                vm_Jobs job = new vm_Jobs();

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
                //GET JOB CORRESPONDENT NAME RECORDS FOR THE DAILY REPORT
                //job.subJobTypes = subJobTypesByJobID("%");

                jobs.Add(job);
            }
            sqlconn.Close();

            //GET JOB CORRESPONDENT NAME RECORDS FOR THE DAILY REPORT
            //jobs.subJobTypes = subJobTypesByJobID("%");

            return View(jobs);
        }

        //GET JOB CORRESPONDENT NAME RECORDS FOR THE DAILY REPORT
        public List<tbl_subJobTypes> subJobTypesByJobID(string jobID)
        {
            //List<tbl_subJobTypes> jobCrspdtNames = new List<tbl_subJobTypes>;
            List<tbl_subJobTypes> subJobTypes = new List<tbl_subJobTypes>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_jobSubJobs.jobID, tbl_subJobTypes.subJobID, tbl_subJobTypes.description " +

            "FROM tbl_subJobTypes " +
            "INNER JOIN tbl_jobSubJobs ON tbl_jobSubJobs.subJobID=tbl_subJobTypes.subJobID " +
            "WHERE " +
            "tbl_jobSubJobs.jobID = @jobID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("jobID", jobID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                tbl_subJobTypes subJobType = new tbl_subJobTypes();

                subJobType.subJobID = (byte)dr1[0];
                subJobType.description = dr1[1].ToString();
                subJobType.id = (int)dr1[2];

                subJobTypes.Add(subJobType);
            }
            return subJobTypes;
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
