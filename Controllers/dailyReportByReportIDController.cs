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
using System.Data.SqlClient;
using System.Configuration;
using System.Dynamic;

namespace allpax_service_record.Controllers
{
    [Authorize]
    public class dailyReportByReportIDController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: customers
        //public ActionResult Index(string reportID)
            public ActionResult Index(int reportID)
        {
            ViewBag.reportID = reportID;           

            List<vm_dailyReportByReportID> dailyReportByID = new List<vm_dailyReportByReportID>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            //begin query for kits available, but not installed
            string sqlquery1 =
                "SELECT tbl_dailyReport.dailyReportID, tbl_dailyReport.jobID, tbl_subJobTypes.description, tbl_dailyReport.date, " +
                "tbl_Jobs.customerContact,tbl_customers.customerName, tbl_customers.address, tbl_dailyReport.equipment, " +
                "tbl_dailyReport.startTime, tbl_dailyReport.endTime, tbl_dailyReport.lunchHours, tbl_customers.customerCode, tbl_dailyReport.dailyReportAuthor  " +

                "FROM tbl_dailyReport " +

                "INNER JOIN " +
                "tbl_Jobs ON tbl_Jobs.jobID = tbl_dailyReport.jobID " +
                "INNER JOIN " +
                "tbl_customers ON tbl_customers.customerCode = tbl_Jobs.customerCode " +
                "INNER JOIN " +
                "tbl_jobSubJobs ON tbl_jobSubJobs.jobID = tbl_Jobs.jobID " +
                "INNER JOIN " +
                "tbl_subJobTypes ON tbl_subJobTypes.subJobID = tbl_jobSubJobs.subJobID " +

                "WHERE " +

                "tbl_dailyReport.subJobID = tbl_subJobTypes.subJobID " +
                "AND " +
                "tbl_dailyReport.dailyReportID LIKE @reportID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.AddWithValue("@reportID", reportID);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_dailyReportByReportID vm_dailyReportByReportID = new vm_dailyReportByReportID();

                vm_dailyReportByReportID.dailyReportID = (int)dr1[0];
                vm_dailyReportByReportID.jobID = dr1[1].ToString();
                vm_dailyReportByReportID.description = dr1[2].ToString();
                vm_dailyReportByReportID.date = String.Format("{0:yyyy-MM-dd}", dr1[3]);
                vm_dailyReportByReportID.customerContact = dr1[4].ToString();
                vm_dailyReportByReportID.customerName = dr1[5].ToString();
                vm_dailyReportByReportID.address = dr1[6].ToString();
                vm_dailyReportByReportID.equipment = dr1[7].ToString();
                vm_dailyReportByReportID.startTime = dr1[8].ToString();
                vm_dailyReportByReportID.endTime = dr1[9].ToString();
                vm_dailyReportByReportID.lunchHours = (int)dr1[10];
                vm_dailyReportByReportID.customerCode = dr1[11].ToString();
                vm_dailyReportByReportID.jobCorrespondentName = jobCrspdtNameByJobID(vm_dailyReportByReportID.jobID);
                vm_dailyReportByReportID.jobCorrespondentEmail = jobCrspdtEmailByJobID(vm_dailyReportByReportID.jobID);
                vm_dailyReportByReportID.dailyReportAuthor = dr1[12].ToString();

                dailyReportByID.Add(vm_dailyReportByReportID);
            }

            sqlconn.Close();
            //end query
            return View(dailyReportByID);
        }
        
        public List<string> jobCrspdtNameByJobID(string jobID)
        {
            List<string> jobCrspdtNames = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_jobCorrespondents.name " +

            "FROM " +
            "tbl_jobCorrespondents " +
            "WHERE " +
            "tbl_jobCorrespondents.jobID = @jobID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("jobID", jobID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                jobCrspdtNames.Add(dr1[0].ToString());
            }
            return jobCrspdtNames;
        }

        public List<string> jobCrspdtEmailByJobID(string jobID)
        {
            List<string> jobCrspdtEmails = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_jobCorrespondents.email " +

            "FROM " +
            "tbl_jobCorrespondents " +
            "WHERE " +
            "tbl_jobCorrespondents.jobID = @jobID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("jobID", jobID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                jobCrspdtEmails.Add(dr1[0].ToString());
            }
            return jobCrspdtEmails;
        }

        //begin CMPS 411 controller code
        [HttpPost]
        public ActionResult AddDailyReport(tbl_dailyReport dailyReportAdd)
        {

             db.Database.ExecuteSqlCommand("Insert into tbl_dailyReport Values({0},{1},{2},{3},{4},{5},{6})",
                dailyReportAdd.jobID, dailyReportAdd.date, dailyReportAdd.subJobID, dailyReportAdd.startTime, dailyReportAdd.endTime, dailyReportAdd.lunchHours, dailyReportAdd.equipment); 
            return new EmptyResult();
            //return RedirectToAction("Home", "Index");
            //return Redirect("/Home");
        }

        [HttpPost]
        public ActionResult AddTeamMember(tbl_dailyReportUsers teamMemberAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
                teamMemberAdd.dailyReportID, teamMemberAdd.userName);

            return new EmptyResult(); 
            //return RedirectToAction("SalesLanding", "Index");
        }

        [HttpPost]
        public ActionResult DeleteTeamMember(tbl_dailyReportUsers teamMemberDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tbl_dailyReportUsers " +
                "WHERE " +
                "dailyReportID=({0})" +
                "AND userName = ({1})", teamMemberDelete.dailyReportID, teamMemberDelete.userName);

            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        public ActionResult UpdateCustomer(tbl_dailyReport custUpdate)
        {
            //db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_customer SET customerCode={0},name={1},address={2}, city={3}, state={4}, zipCode={5} WHERE id={6}",
            //      custUpdate.customerCode, custUpdate.name, custUpdate.address, custUpdate.city, custUpdate.state, custUpdate.zipCode, custUpdate.id);

            //db.Database.ExecuteSqlCommand("UPDATE tbl_customers SET customerCode={0}, customerName={1}, address={2} WHERE id={3}",
            //      custUpdate.customerCode, custUpdate.customerName, custUpdate.address, custUpdate.id);

            return RedirectToAction("Index");
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
