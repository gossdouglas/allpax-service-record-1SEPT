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
    public class workDescDelaysController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: workDesc
        public ActionResult Index(int dailyReportID)
         {
            List<vm_workDesc> workDescs = new List<vm_workDesc>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();
           
            //begin
            string sqlquery1 =
                "SELECT tbl_dailyReportTimeEntry.dailyReportID, tbl_dailyReportTimeEntry.workDescription, " +
                "tbl_dailyReportTimeEntry.hours, tbl_dailyReportTimeEntry.timeEntryID " +

                "FROM tbl_dailyReportTimeEntry " +
                "WHERE " +
                "tbl_dailyReportTimeEntry.dailyReportID = @dailyReportID " +
                "AND " +
                "tbl_dailyReportTimeEntry.workDescriptionCategory = '2'";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.AddWithValue("@dailyReportID", dailyReportID);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_workDesc workDesc = new vm_workDesc();

                workDesc.dailyReportID = (int) dr1[0];
                workDesc.workDescription = dr1[1].ToString();
                workDesc.hours = (int)dr1[2];
                workDesc.timeEntryID = (int)dr1[3];
                //workDesc.timeEntryID = (int) dr1[2];

                workDesc.userNames = workDescUsersByTimeEntryID(workDesc.timeEntryID);
                workDesc.userShortNames = workDescUserShortNamesByTimeEntryID(workDesc.timeEntryID);

                workDescs.Add(workDesc);
            }
            //sqlconn.Close();
            //end
            return View(workDescs);
        }

        public List<string> workDescUsersByTimeEntryID(int timeEntryID)
        {
            List<string> userNames = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            //begin query for kits available but not installed by machine
            string sqlquery1 = "SELECT tbl_dailyReportTimeEntryUsers.userName " +

            "FROM " +
            "tbl_dailyReportTimeEntryUsers " +
            "WHERE " +
            "tbl_dailyReportTimeEntryUsers.timeEntryID = @timeEntryID";
            //end query for kits available but not installed by machine

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("timeEntryID", timeEntryID));           
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda3.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                userNames.Add(dr1[0].ToString());
            }
            return userNames;
        }
        public List<string> workDescUserShortNamesByTimeEntryID(int timeEntryID)
        {
            List<string> userShortNames = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            //begin query for kits available but not installed by machine
            string sqlquery1 = "SELECT tbl_Users.shortName " +

            "FROM " +
            "tbl_Users " +

            "INNER JOIN " +
            "tbl_dailyReportTimeEntryUsers ON " +
            "tbl_Users.userName = tbl_dailyReportTimeEntryUsers.userName " +

            "WHERE " +
            "tbl_dailyReportTimeEntryUsers.timeEntryID = @timeEntryID";
            //end query for kits available but not installed by machine

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.Add(new SqlParameter("timeEntryID", timeEntryID));
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                userShortNames.Add(dr1[0].ToString());
            }
            return userShortNames;
        }
     
        [HttpPost]
        public ActionResult AddWorkDesc(vm_workDesc workDescAdd)
        {
            ////--IF THE DAILY REPORT DOESN'T ALREADY EXIST...
            //db.Database.ExecuteSqlCommand
            //    ("DECLARE @id INT " +
            //    "DECLARE @timeEntryID INT " +
            //    "IF NOT EXISTS (SELECT * FROM tbl_dailyReportTimeEntry " +
            //    "WHERE " +
            //    "dailyReportID = {0} " +
            //    "AND " +
            //    "workDescription = {1} " +
            //    "AND " +
            //    "workDescriptionCategory ={3}) " +

            //    "BEGIN " +

            //    "INSERT INTO tbl_dailyReportTimeEntry VALUES({0}, {1}, {3}, {4}) " +
            //    "SET @id = SCOPE_IDENTITY() " +
            //    "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES(@id, {2}) " +

            //    "END " +

            //    //--IF THE DAILY REPORT DOES ALREADY EXIST...
            //    "ELSE " +
            //    "BEGIN " +

            //    "SET @timeEntryID = " +
            //        "(SELECT tbl_dailyReportTimeEntry.timeEntryID " +
            //        "FROM tbl_dailyReportTimeEntry " +
            //        "WHERE " +

            //        "tbl_dailyReportTimeEntry.dailyReportID like {0} " +
            //        "AND " +
            //        "workDescription = {1} " +
            //        "AND " +
            //        "workDescriptionCategory = {3}) " +

            //        "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES(@timeEntryID, {02}) " +

            //    "END",

            //    workDescAdd.dailyReportID, workDescAdd.workDescription, workDescAdd.userName, workDescAdd.workDescriptionCategory, workDescAdd.hours);
           
            return new EmptyResult();
            //return View();
            //return RedirectToAction("Index");
            //return RedirectToAction("Index", new { dailyReportID = workDescAdd.dailyReportID });
            //return RedirectToAction("Index", "DailyReportByReportID", new { reportID = workDescAdd.dailyReportID});
        }

        [HttpPost]
        public ActionResult AddWorkDescDelaysNarr(vm_workDesc workDescAdd)
        {
            //--IF THE DAILY REPORT DOESN'T ALREADY EXIST...
            db.Database.ExecuteSqlCommand(

                "INSERT INTO tbl_dailyReportTimeEntry VALUES({0}, {1}, {2}, {3}) ",

                workDescAdd.dailyReportID, workDescAdd.workDescription, workDescAdd.workDescriptionCategory, workDescAdd.hours);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AddWorkDescDelaysTeam(vm_workDesc workDescAdd)
        {
            //--IF THE DAILY REPORT DOES ALREADY EXIST...
            db.Database.ExecuteSqlCommand(

            "DECLARE @timeEntryID INT " +

                    "SET @timeEntryID = " +
                        "(SELECT tbl_dailyReportTimeEntry.timeEntryID " +
                        "FROM tbl_dailyReportTimeEntry " +
                        "WHERE " +

                        "tbl_dailyReportTimeEntry.dailyReportID like {0} " +
                        "AND " +
                        "workDescription = {1} " +
                        "AND " +
                        "workDescriptionCategory = {3}) " +

                    "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES(@timeEntryID, {02}) ",

                workDescAdd.dailyReportID, workDescAdd.workDescription, workDescAdd.userName, workDescAdd.workDescriptionCategory);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult AddTeamMember(vm_workDesc teamMemberAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportTimeEntryUsers Values({0},{1})",
                teamMemberAdd.timeEntryID, teamMemberAdd.userName);

            return new EmptyResult();
        }

        public ActionResult DeleteWorkDesc(vm_workDesc workDescDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tbl_dailyReportTimeEntry WHERE timeEntryID=({0})", workDescDelete.timeEntryID);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteTeamMember(vm_workDesc teamMemberDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tbl_dailyReportTimeEntryUsers " +
                "WHERE " +
                "timeEntryID=({0})" +
                "AND userName = ({1})", teamMemberDelete.timeEntryID, teamMemberDelete.userName);

            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        public ActionResult UpdateWorkDesc(vm_workDesc workDescUpdate)
        {
            db.Database.ExecuteSqlCommand("UPDATE tbl_dailyReportTimeEntry SET workDescription={1}, workDescriptionCategory={2}, hours={3} WHERE timeEntryID={0}",
                  workDescUpdate.timeEntryID, workDescUpdate.workDescription, workDescUpdate.workDescriptionCategory, workDescUpdate.hours);

            return new EmptyResult();
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
