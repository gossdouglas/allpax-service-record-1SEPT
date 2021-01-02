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
            vm_Jobs vm_Jobs = new vm_Jobs();
            List<tbl_Jobs> jobList = new List<tbl_Jobs>();

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
                job.nrmlHoursDaily = (int) dr1[8];
                job.dblTimeHours = (Boolean)dr1[9];
                job.id = (int)dr1[10];

                jobList.Add(job);
            }

            vm_Jobs.jobs = jobList;
            vm_Jobs.customers = customerList();
            vm_Jobs.subJobTypes = subJobTypesByJobID("%");
            vm_Jobs.resourceTypes = resourceTypesByJobID("%");

            sqlconn.Close();

            return View(vm_Jobs);
        }

        [HttpPost]
        public ActionResult AddJob(vm_Jobs jobAdd)
        {

            db.Database.ExecuteSqlCommand("Insert into tbl_Jobs Values({0},{1},{2},{3},{4},{5},{6},{7},{8}, {9})",
               jobAdd.jobID, jobAdd.description, jobAdd.customerCode, jobAdd.customerContact, jobAdd.active,
               jobAdd.location, jobAdd.nrmlHoursStart, jobAdd.nrmlHoursEnd, jobAdd.dblTimeHours, jobAdd.nrmlHoursDaily);

            if (jobAdd.jobSubJobsAdd != null)
            {
                //add sub-jobs to this job
                foreach (string item in jobAdd.jobSubJobsAdd)
                {
                    db.Database.ExecuteSqlCommand("Insert into tbl_jobSubJobs Values({0},{1})",
                    jobAdd.jobID, item);
                }
            }

            if (jobAdd.resourceTypes != null)
            {
                //add resource types to this job
                foreach (tbl_resourceTypes item in jobAdd.resourceTypes)
                {
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_jobResourceTypes VALUES({0}, {1}, {2}) ",

                    jobAdd.jobID, item.resourceTypeID, item.rate);
                }
            }

            if (jobAdd.jobCrspdtInfo != null)
            {
                //add correspondents to this job
                foreach (tbl_jobCorrespondents item in jobAdd.jobCrspdtInfo)
                {
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_jobCorrespondents VALUES({0}, {1}, {2}, {3}) ",

                    //jobAdd.jobID, item.jobCrspdtName, item.jobCrspdtEmail, "1");
                    jobAdd.jobID, item.name, item.email, "1");
                }
            }

            return Json(Url.Action("Index", "Jobs"));
        }

        public ActionResult UpdateJob(vm_Jobs jobUpdate)
        {

            db.Database.ExecuteSqlCommand(
                "UPDATE tbl_Jobs " +
                "SET " +

                "description = {1}, " +
                "customerCode = {2}, " +
                "customerContact = {3}, " +
                "active = {4}, " +
                "nrmlHoursStart = {5}, " +
                "nrmlHoursEnd = {6}, " +
                "dblTimeHours = {7}, " +
                "nrmlHoursDaily = {8} " +

                "WHERE jobID = {0}",
                jobUpdate.jobID, jobUpdate.description, jobUpdate.customerCode, jobUpdate.customerContact,
                jobUpdate.active, jobUpdate.nrmlHoursStart, jobUpdate.nrmlHoursEnd,
                jobUpdate.dblTimeHours, jobUpdate.nrmlHoursDaily);

            if (jobUpdate.jobSubJobsAdd != null)
            {
                foreach (string subJobID in jobUpdate.jobSubJobsAdd)
                {
                    //System.Diagnostics.Debug.WriteLine(subJobID);
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_jobSubJobs(jobID, subJobID) VALUES({0}, {01}) ",

                    jobUpdate.jobID, subJobID);
                }
            }

            if (jobUpdate.jobSubJobsDelete != null)
            {
                foreach (string subJobID in jobUpdate.jobSubJobsDelete)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM tbl_jobSubJobs " +
                        "WHERE jobID= {0} AND subJobID = {1}", jobUpdate.jobID, subJobID);
                }
            }

            if (jobUpdate.resourceTypesAdd != null)
            {
                //foreach (string resourceTypeID in jobUpdate.resourceTypesAdd)
                //{
                //    db.Database.ExecuteSqlCommand(

                //    "INSERT INTO tbl_jobResourceTypes(jobID, resourceTypeID) VALUES({0}, {01}) ",

                //    jobUpdate.jobID, resourceTypeID);
                //}

                foreach (tbl_jobResourceTypes item in jobUpdate.resourceTypesAdd)
                {
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_jobResourceTypes VALUES({0}, {1}, {2}) ",

                    item.jobID, item.resourceTypeID, item.rate);
                }
            }

            if (jobUpdate.resourceTypesDelete != null)
            {
                foreach (tbl_jobResourceTypes item in jobUpdate.resourceTypesDelete)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM tbl_jobResourceTypes " +
                    "WHERE jobID= {0} AND resourceTypeID = {1}", jobUpdate.jobID, item.resourceTypeID);
                }
            }

            if (jobUpdate.resourceTypesEdit != null)
            {
                foreach (tbl_jobResourceTypes item in jobUpdate.resourceTypesEdit)
                {
                        db.Database.ExecuteSqlCommand("UPDATE tbl_jobResourceTypes " +
                            "SET " +
                            "rate = {2} " +
                            "WHERE jobID= {0} AND resourceTypeID = {1}", jobUpdate.jobID, item.resourceTypeID, item.rate);
                }
            }

            if (jobUpdate.jobCrspdtInfoAdd != null)
            {
                foreach (tbl_jobCorrespondents item in jobUpdate.jobCrspdtInfoAdd)
                {
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_jobCorrespondents VALUES({0}, {1}, {2}, {3}) ",

                    jobUpdate.jobID, item.name, item.email, "1");
                }
            }

            if (jobUpdate.jobCrspdtInfoDelete != null)
            {
                foreach (tbl_jobCorrespondents item in jobUpdate.jobCrspdtInfoDelete)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM tbl_jobCorrespondents " +
                    "WHERE jobCorrespondentID= {0} ", item.jobCorrespondentID);
                }
            }

            if (jobUpdate.jobCrspdtInfoEdit != null)
            {
                foreach (tbl_jobCorrespondents item in jobUpdate.jobCrspdtInfoEdit)
                {
                    db.Database.ExecuteSqlCommand("UPDATE tbl_jobCorrespondents " +
                            "SET " +
                            "name = {1}, " +
                            "email = {2} " +
                            "WHERE jobCorrespondentID= {0} ", item.jobCorrespondentID, item.name, item.email);
                }
            }

            return Json(Url.Action("Index", "Jobs"));
        }

        //GET SUB JOB TYPES
        public List<tbl_subJobTypes> subJobTypesByJobID(string jobID)
        {
            List<tbl_subJobTypes> subJobTypes = new List<tbl_subJobTypes>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_subJobTypes.subJobID, tbl_subJobTypes.description " +

            "FROM tbl_subJobTypes ";

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

                subJobTypes.Add(subJobType);
            }
            return subJobTypes;
        }

        //GET RESOURCE TYPES
        public List<tbl_resourceTypes> resourceTypesByJobID(string jobID)
        {
            List<tbl_resourceTypes> resourceTypes = new List<tbl_resourceTypes>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_resourceTypes.resourceTypeID, tbl_resourceTypes.resourceType, tbl_resourceTypes.description, tbl_resourceTypes.rate " +

            "FROM tbl_resourceTypes ";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("jobID", jobID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                tbl_resourceTypes resourceType = new tbl_resourceTypes();

                resourceType.resourceTypeID = (byte)dr1[0];
                resourceType.resourceType = dr1[1].ToString();
                resourceType.description = dr1[2].ToString();
                resourceType.rate = (decimal)dr1[3];

                resourceTypes.Add(resourceType);
            }
            return resourceTypes;
        }

        //GET RESOURCE TYPES
        public List<tbl_customers> customerList()
        {
            List<tbl_customers> customers = new List<tbl_customers>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_customers.customerCode, tbl_customers.customerName, tbl_customers.address, tbl_customers.id " +

            "FROM tbl_customers";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            //sqlcomm1.Parameters.Add(new SqlParameter("jobID", jobID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                tbl_customers customer = new tbl_customers();

                customer.customerCode = dr1[0].ToString();
                customer.customerName = dr1[1].ToString();
                customer.address = dr1[2].ToString();
                customer.id = (int)dr1[3];

                customers.Add(customer);
            }
            return customers;
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
