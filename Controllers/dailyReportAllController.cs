using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_service_record.Models;
using allpax_service_record.Models.View_Models;

namespace allpax_service_record.Controllers
{
    [Authorize]
    public class dailyReportAllController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: customers
        public ActionResult Index()
        {

            List<vm_dailyReportViewAll> dailyRptViewAlls = new List<vm_dailyReportViewAll>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            //begin
            string sqlquery1 =
                "SELECT tbl_dailyReport.dailyReportID, tbl_Jobs.active, tbl_dailyReport.date, tbl_dailyReport.jobID, tbl_dailyReport.subJobID, " +
                "tbl_subJobTypes.description, tbl_customers.customerName, tbl_customers.address, tbl_customers.customerCode, tbl_jobs.customerContact " +
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
                    "tbl_dailyReport.subJobID = tbl_subJobTypes.subJobID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            //sqlcomm1.Parameters.AddWithValue("@dailyReportID", dailyReportID);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_dailyReportViewAll dailyRptViewAll = new vm_dailyReportViewAll();

                dailyRptViewAll.dailyReportID = (int)dr1[0];
                dailyRptViewAll.active = (Boolean)dr1[1];
                dailyRptViewAll.date = String.Format("{0:yyyy-MM-dd}", dr1[2]);
                dailyRptViewAll.jobID = dr1[3].ToString();
                dailyRptViewAll.subJobID = dr1[4].ToString();
                dailyRptViewAll.description = dr1[5].ToString(); 
                dailyRptViewAll.customerName = dr1[6].ToString();
                dailyRptViewAll.address = dr1[7].ToString();
                dailyRptViewAll.customercode = dr1[8].ToString();
                dailyRptViewAll.customerContact = dr1[9].ToString();
                dailyRptViewAll.team = TeamNamesByDailyReportID(dailyRptViewAll.dailyReportID);

                dailyRptViewAlls.Add(dailyRptViewAll);
            }
            sqlconn.Close();

            return View(dailyRptViewAlls);
        }

        public ActionResult Filtered(string startDate, string endDate)
        {

            //string sql = @"SELECT tbl_dailyReport.dailyReportID, tbl_Jobs.active, tbl_dailyReport.date, tbl_dailyReport.jobID, " +
            //"tbl_subJobTypes.description, tbl_customers.customerName, tbl_customers.address, tbl_customers.customerCode, tbl_jobs.customerContact, tbl_dailyReport.subJobID " +
            //"FROM tbl_dailyReport " +

            //"INNER JOIN " +
            //"tbl_Jobs ON tbl_Jobs.jobID = tbl_dailyReport.jobID " +
            //"INNER JOIN " +
            //"tbl_customers ON tbl_customers.customerCode = tbl_Jobs.customerCode " +
            //"INNER JOIN " +
            //"tbl_jobSubJobs ON tbl_jobSubJobs.jobID = tbl_Jobs.jobID " +
            //"INNER JOIN " +
            //"tbl_subJobTypes ON tbl_subJobTypes.subJobID = tbl_jobSubJobs.subJobID " +
            //"WHERE " +
            //"tbl_dailyReport.subJobID = tbl_subJobTypes.subJobID " +
            //"AND " +
            //"tbl_dailyReport.date >= {0} " +
            //"AND " +
            //"tbl_dailyReport.date <= {1}";

            //List<vm_dailyReportViewAll> list = db.Database.SqlQuery<vm_dailyReportViewAll>
            //(sql, startDate, endDate).ToList();

            //return View(list);

            List<vm_dailyReportViewAll> dailyRptViewAlls = new List<vm_dailyReportViewAll>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            //begin
            string sqlquery1 =
             "SELECT tbl_dailyReport.dailyReportID, tbl_Jobs.active, tbl_dailyReport.date, tbl_dailyReport.jobID, " +
            "tbl_subJobTypes.description, tbl_customers.customerName, tbl_customers.address, tbl_customers.customerCode, tbl_jobs.customerContact, tbl_dailyReport.subJobID " +
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
            //"tbl_dailyReport.date >= {0} " +
            "tbl_dailyReport.date >= @startDate " +
            "AND " +
            //"tbl_dailyReport.date <= {1}";
            "tbl_dailyReport.date <= @endDate";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.AddWithValue("@startDate", startDate);
            sqlcomm1.Parameters.AddWithValue("@endDate", endDate);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_dailyReportViewAll dailyRptViewAll = new vm_dailyReportViewAll();

                dailyRptViewAll.dailyReportID = (int)dr1[0];
                dailyRptViewAll.active = (Boolean)dr1[1];
                dailyRptViewAll.date = String.Format("{0:yyyy-MM-dd}", dr1[2]);
                dailyRptViewAll.jobID = dr1[3].ToString();
                dailyRptViewAll.subJobID = dr1[4].ToString();
                dailyRptViewAll.description = dr1[5].ToString();
                dailyRptViewAll.customerName = dr1[6].ToString();
                dailyRptViewAll.address = dr1[7].ToString();
                dailyRptViewAll.customercode = dr1[8].ToString();
                dailyRptViewAll.customerContact = dr1[9].ToString();
                dailyRptViewAll.team = TeamNamesByDailyReportID(dailyRptViewAll.dailyReportID);

                dailyRptViewAlls.Add(dailyRptViewAll);
            }
            sqlconn.Close();

            return View(dailyRptViewAlls);
        }

        public List<string> TeamNamesByDailyReportID(int dailyReportID)
        {
            List<string> team = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            //begin query for kits available but not installed by machine
            string sqlquery1 = "SELECT tbl_Users.name " +
            "FROM " +
            "tbl_Users " +

            "INNER JOIN " +
            "tbl_dailyReportUsers ON tbl_dailyReportUsers.userName = tbl_Users.userName " +

            "WHERE " +
            "tbl_dailyReportUsers.dailyReportID = @dailyReportID";
            //end query for kits available but not installed by machine

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("dailyReportID", dailyReportID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                team.Add(dr1[0].ToString());
            }
            return team;
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
