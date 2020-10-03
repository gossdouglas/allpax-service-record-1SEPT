using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_service_record.Models;
using allpax_service_record.Models.View_Models;
using System.Diagnostics;


namespace allpax_service_record.Controllers
{
    [Authorize]
    public class dailyReportController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        public ActionResult Index()
        {
            var sql = db.tbl_dailyReport.SqlQuery("SELECT * from tbl_dailyReport").ToList();
           
            return View(sql.ToList()); 
        }

        [HttpPost]
        public ActionResult AddDailyReport(vm_dailyReport dailyReportAdd)
        {

            db.Database.ExecuteSqlCommand("Insert into tbl_dailyReport Values({0},{1},{2},{3},{4},{5},{6},{7})",
               dailyReportAdd.jobID, dailyReportAdd.date, dailyReportAdd.subJobID, dailyReportAdd.startTime, dailyReportAdd.endTime,
               dailyReportAdd.lunchHours, dailyReportAdd.equipment, dailyReportAdd.dailyReportAuthor);

            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            //List<string> new_dailyRptID = new List<string>();
            String new_dailyRptID = "";

            int tempDailyReportID = 170;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetLastDlyRptCrtdByUserName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@dailyReportAuthor",
                    Value = dailyReportAdd.dailyReportAuthor
                };
                cmd.Parameters.Add(param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    //new_dailyRptID.Add(rdr["dailyReportID"].ToString());
                    new_dailyRptID = rdr["dailyReportID"].ToString();
                }
            }

            //db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
            //   "169", "dbanner");  works

            //db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
            //   "169", dailyReportAdd.dailyRptTeamArr[0]);works

            //db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
            //   tempDailyReportID, dailyReportAdd.dailyRptTeamArr[0]);works


            foreach (string item in dailyReportAdd.dailyRptTeamArr)
            { // Loop through List with foreach

                //db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
                //new_dailyRptID, item);

                //db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
                //tempDailyReportID, item);works

                db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
                new_dailyRptID, item);

                //Debug.WriteLine(new_dailyRptID + " " + item);
            }

            //db.Database.ExecuteSqlCommand("spCopyDailyRpt @p0, @p1", dailyReportAdd.dailyReportID, new_dailyRptID[0]);

            return new EmptyResult();
        }

        //[HttpPost]
        //public ActionResult AddDailyReport(tbl_dailyReport dailyReportAdd)
        //{
        //     db.Database.ExecuteSqlCommand("Insert into tbl_dailyReport Values({0},{1},{2},{3},{4},{5},{6},{7})",
        //        dailyReportAdd.jobID, dailyReportAdd.date, dailyReportAdd.subJobID, dailyReportAdd.startTime, dailyReportAdd.endTime, 
        //        dailyReportAdd.lunchHours, dailyReportAdd.equipment, dailyReportAdd.dailyReportAuthor);

        //    string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
        //    List<string> new_dailyRptID = new List<string>();

        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();

        //        SqlCommand cmd = new SqlCommand("spGetLastDlyRptCrtdByUserName", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlParameter param = new SqlParameter()
        //        {
        //            ParameterName = "@dailyReportAuthor",
        //            Value = dailyReportAdd.dailyReportAuthor
        //        };
        //        cmd.Parameters.Add(param);
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            new_dailyRptID.Add(rdr["dailyReportID"].ToString());
        //        }
        //    }

        //    db.Database.ExecuteSqlCommand("spCopyDailyRpt @p0, @p1", dailyReportAdd.dailyReportID, new_dailyRptID[0]);

        //    return new EmptyResult();
        //}

        public ActionResult copyDailyReport(string jobID, string description, string subJobID, string customerName, 
            string location, string customercode, string customerContact, string equipment, int copiedDailyReportID)
        {
            ViewBag.jobiD = jobID;
            ViewBag.subJobID = subJobID;
            ViewBag.description = description;
            ViewBag.customerName = customerName;
            ViewBag.location = location;
            ViewBag.customercode = customercode;
            ViewBag.customerContact = customerContact;
            ViewBag.equipment = equipment;
            ViewBag.copiedDailyReportID = copiedDailyReportID;

            List<vm_dailyReportByReportID> dailyReportByID = new List<vm_dailyReportByReportID>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

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
            sqlcomm1.Parameters.AddWithValue("@reportID", copiedDailyReportID);
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

        public ActionResult DeleteDailyReport(tbl_dailyReport dailyReportDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tbl_dailyReport WHERE jobID=({0})", dailyReportDelete.jobID);

            return RedirectToAction("Index");
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
