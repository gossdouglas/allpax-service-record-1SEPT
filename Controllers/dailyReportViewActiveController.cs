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
using System.Diagnostics;

namespace allpax_service_record.Controllers
{
    public class dailyReportViewActiveController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: dailyReportViewActive
        public ActionResult Index()
        {
            List<vm_dailyReportViewActive> dailyReportViewActives = new List<vm_dailyReportViewActive>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_dailyReport.date, tbl_dailyReport.jobID, tbl_dailyReport.startTime, tbl_dailyReport.endTime, tbl_dailyReport.lunchHours, " +
                "tbl_subJobTypes.description, tbl_Jobs.active, " +

                "(SELECT convert(decimal(3,1), DATEDIFF ( mi, tbl_dailyReport.startTime , tbl_dailyReport.endTime ) / 60.0) - tbl_dailyReport.lunchHours) as totalHours, " +
                "(SELECT SUM(hours) " +
                       "FROM tbl_dailyReportTimeEntry " +
                       "WHERE tbl_dailyReportTimeEntry.dailyReportID = tbl_dailyReport.dailyReportID AND " +

                       "tbl_dailyReportTimeEntry.workDescriptionCategory = '2') as delayHours, " +
                "(SELECT SUM(hours) " +
                        "FROM tbl_dailyReportTimeEntry " +
                        "WHERE tbl_dailyReportTimeEntry.dailyReportID = tbl_dailyReport.dailyReportID AND " +

                        "tbl_dailyReportTimeEntry.workDescriptionCategory = '3') as wntyDelayHours, " +
                "tbl_dailyReport.dailyReportID " +

                "FROM tbl_dailyReport " +

                "INNER JOIN tbl_subJobTypes ON " +
                "tbl_subJobTypes.subJobID = tbl_dailyReport.subJobID " +
                "INNER JOIN tbl_Jobs ON " +
                "tbl_Jobs.jobID = tbl_dailyReport.jobID " +

                "WHERE " +
                "tbl_Jobs.active = '1'"; 

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_dailyReportViewActive dailyReportViewActive = new vm_dailyReportViewActive();

                dailyReportViewActive.date = String.Format("{0:yyyy-MM-dd}", dr1[0]);
                dailyReportViewActive.jobID = dr1[1].ToString();
                dailyReportViewActive.startTime = (TimeSpan)dr1[2];
                dailyReportViewActive.endTime = (TimeSpan)dr1[3];
                dailyReportViewActive.lunchHours = (int)dr1[4];
                dailyReportViewActive.subJobDescription = dr1[5].ToString();
                dailyReportViewActive.active = (Boolean)dr1[6];
                dailyReportViewActive.totalHours = (decimal)dr1[7];

                try
                {
                    dailyReportViewActive.delayHours = (decimal)dr1[8];
                }

                catch (Exception)
                {
                    dailyReportViewActive.delayHours = 0;
                }

                try
                {
                    dailyReportViewActive.wntyDelayHours = (decimal)dr1[9];
                }

                catch (Exception)
                {
                    dailyReportViewActive.wntyDelayHours = 0;
                }

                dailyReportViewActive.dailyReportID = (int)dr1[10];
                dailyReportViewActive.teamUserNames = TeamUserNamesByDailyReportID(dailyReportViewActive.dailyReportID);
                dailyReportViewActive.teamNames = TeamNamesByDailyReportID(dailyReportViewActive.dailyReportID);
                dailyReportViewActive.teamShortNames = TeamShortNamesByDailyReportID(dailyReportViewActive.dailyReportID);

                dailyReportViewActives.Add(dailyReportViewActive);
            }
            sqlconn.Close();
            return View(dailyReportViewActives);
        }

        public ActionResult Filtered(string startDate, string endDate)
        {
            List<vm_dailyReportViewActive> dailyReportViewActives = new List<vm_dailyReportViewActive>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_dailyReport.date, tbl_dailyReport.jobID, tbl_dailyReport.startTime, tbl_dailyReport.endTime, tbl_dailyReport.lunchHours, " +
                "tbl_subJobTypes.description, tbl_Jobs.active, " +

                "(SELECT convert(decimal(3,1), DATEDIFF ( mi, tbl_dailyReport.startTime , tbl_dailyReport.endTime ) / 60.0) - tbl_dailyReport.lunchHours) as totalHours, " +
                "(SELECT SUM(hours) " +
                       "FROM tbl_dailyReportTimeEntry " +
                       "WHERE tbl_dailyReportTimeEntry.dailyReportID = tbl_dailyReport.dailyReportID AND " +

                       "tbl_dailyReportTimeEntry.workDescriptionCategory = '2') as delayHours, " +
                "(SELECT SUM(hours) " +
                        "FROM tbl_dailyReportTimeEntry " +
                        "WHERE tbl_dailyReportTimeEntry.dailyReportID = tbl_dailyReport.dailyReportID AND " +

                        "tbl_dailyReportTimeEntry.workDescriptionCategory = '3') as wntyDelayHours, " +
                "tbl_dailyReport.dailyReportID " +

                "FROM tbl_dailyReport " +

                "INNER JOIN tbl_subJobTypes ON " +
                "tbl_subJobTypes.subJobID = tbl_dailyReport.subJobID " +
                "INNER JOIN tbl_Jobs ON " +
                "tbl_Jobs.jobID = tbl_dailyReport.jobID " +

                "WHERE " +
                "tbl_dailyReport.date >= @startDate " +
                "AND " +
                "tbl_dailyReport.date <= @endDate " +
                "AND " +
                "tbl_Jobs.active = '1'";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.AddWithValue("@startDate", startDate);
            sqlcomm1.Parameters.AddWithValue("@endDate", endDate);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_dailyReportViewActive dailyReportViewActive = new vm_dailyReportViewActive();

                dailyReportViewActive.date = String.Format("{0:yyyy-MM-dd}", dr1[0]);
                dailyReportViewActive.jobID = dr1[1].ToString(); ;
                dailyReportViewActive.startTime = (TimeSpan)dr1[2];
                dailyReportViewActive.endTime = (TimeSpan)dr1[3];
                dailyReportViewActive.lunchHours = (int)dr1[4];
                dailyReportViewActive.subJobDescription = dr1[5].ToString();
                dailyReportViewActive.active = (Boolean)dr1[6];
                dailyReportViewActive.totalHours = (decimal)dr1[7];

                try
                {
                    dailyReportViewActive.delayHours = (decimal)dr1[8];
                }

                catch (Exception)
                {
                    dailyReportViewActive.delayHours = 0;
                }

                try
                {
                    dailyReportViewActive.wntyDelayHours = (decimal)dr1[9];
                }

                catch (Exception)
                {
                    dailyReportViewActive.wntyDelayHours = 0;
                }

                //dailyReportViewActive.delayHours = (decimal)dr1[8];
                //dailyReportViewActive.wntyDelayHours = (decimal)dr1[9];
                dailyReportViewActive.dailyReportID = (int)dr1[10];
                dailyReportViewActive.teamUserNames = TeamUserNamesByDailyReportID(dailyReportViewActive.dailyReportID);
                dailyReportViewActive.teamNames = TeamNamesByDailyReportID(dailyReportViewActive.dailyReportID);
                dailyReportViewActive.teamShortNames = TeamShortNamesByDailyReportID(dailyReportViewActive.dailyReportID);

                dailyReportViewActives.Add(dailyReportViewActive);
            }
            sqlconn.Close();
            return View(dailyReportViewActives);
        }

        public List<string> TeamUserNamesByDailyReportID(int dailyReportID)
        {
            List<string> teamUserNames = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_Users.userName " +
            "FROM " +
            "tbl_Users " +
            "INNER JOIN " +
            "tbl_dailyReportUsers ON tbl_dailyReportUsers.userName = tbl_Users.userName " +
            "WHERE " +
            "tbl_dailyReportUsers.dailyReportID = @dailyReportID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("dailyReportID", dailyReportID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                teamUserNames.Add(dr1[0].ToString());
            }
            return teamUserNames;
        }
        public List<string> TeamNamesByDailyReportID(int dailyReportID)
        {
            List<string> teamNames = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_Users.name " +
            "FROM " +
            "tbl_Users " +
            "INNER JOIN " +
            "tbl_dailyReportUsers ON tbl_dailyReportUsers.userName = tbl_Users.userName " +
            "WHERE " +
            "tbl_dailyReportUsers.dailyReportID = @dailyReportID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("dailyReportID", dailyReportID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                teamNames.Add(dr1[0].ToString());
            }
            return teamNames;
        }
        public List<string> TeamShortNamesByDailyReportID(int dailyReportID)
        {
            List<string> teamShortNames = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string sqlquery1 = "SELECT tbl_Users.shortName " +
            "FROM " +
            "tbl_Users " +
            "INNER JOIN " +
            "tbl_dailyReportUsers ON tbl_dailyReportUsers.userName = tbl_Users.userName " +
            "WHERE " +
            "tbl_dailyReportUsers.dailyReportID = @dailyReportID";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("dailyReportID", dailyReportID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                teamShortNames.Add(dr1[0].ToString());
            }
            return teamShortNames;
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
