﻿using System;
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

        public ActionResult Index(int reportID)
        {
            ViewBag.reportID = reportID;           

            List<vm_dailyReportByReportID> dailyReportByID = new List<vm_dailyReportByReportID>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_dailyReport.dailyReportID, tbl_dailyReport.jobID, tbl_dailyReport.subJobID, tbl_subJobTypes.description, tbl_dailyReport.date, " +
                "tbl_Jobs.customerContact,tbl_customers.customerName, tbl_customers.address, tbl_dailyReport.equipment, tbl_dailyReport.startTime, " +
                "tbl_dailyReport.endTime, tbl_dailyReport.lunchHours, tbl_customers.customerCode, tbl_dailyReport.dailyReportAuthor, tbl_dailyReport.submissionStatus " +

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
                vm_dailyReportByReportID.subJobID = dr1[2].ToString();
                vm_dailyReportByReportID.description = dr1[3].ToString();
                vm_dailyReportByReportID.date = String.Format("{0:yyyy-MM-dd}", dr1[4]);
                vm_dailyReportByReportID.customerContact = dr1[5].ToString();
                vm_dailyReportByReportID.customerName = dr1[6].ToString();
                vm_dailyReportByReportID.address = dr1[7].ToString();
                vm_dailyReportByReportID.equipment = dr1[8].ToString();
                vm_dailyReportByReportID.startTime = dr1[9].ToString();
                vm_dailyReportByReportID.endTime = dr1[10].ToString();
                vm_dailyReportByReportID.lunchHours = (int)dr1[11];
                vm_dailyReportByReportID.customerCode = dr1[12].ToString();
                //GET JOB CORRESPONDENT NAME RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.jobCorrespondentName = jobCrspdtNameByJobID(vm_dailyReportByReportID.jobID);
                //GET JOB CORRESPONDENT EMAIL ADDRESS RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.jobCorrespondentEmail = jobCrspdtEmailByJobID(vm_dailyReportByReportID.jobID);
                //
                vm_dailyReportByReportID.dailyReportAuthor = dr1[13].ToString();
                vm_dailyReportByReportID.submissionStatus = (int)dr1[14];

                //GET WORK DESCRIPTION RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.workDescArr = getWorkDescRecords(vm_dailyReportByReportID.dailyReportID);
                //GET DELAYS RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.delaysArr = getDelaysRecords(vm_dailyReportByReportID.dailyReportID);
                //GET WARRANTY DELAYS RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.wntyDelaysArr = getWntyDelaysRecords(vm_dailyReportByReportID.dailyReportID);

                dailyReportByID.Add(vm_dailyReportByReportID);//add all of the revelevant data objects to dailyReportByID...
            }

            sqlconn.Close();

            return View(dailyReportByID);//...to be passed to the view

            //return RedirectToAction("Index", "dailyReportAll");//redirects, and the daily report does kick out the records to its view
        }

        public ActionResult AddDailyReport(vm_dailyReport dailyReportAdd)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            int passedDailyRptID = dailyReportAdd.dailyReportID;

            if (dailyReportAdd.workDescArr != null)
            {
                foreach (vm_workDesc item in dailyReportAdd.workDescArr)
                {
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_dailyReportTimeEntry VALUES({0}, {1}, {2}, {3}) ",

                    passedDailyRptID, item.workDescription, item.workDescriptionCategory, item.hours);

                    foreach (string userNames in item.userNames)
                    {
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

                    passedDailyRptID, item.workDescription, userNames, item.workDescriptionCategory);
                    }
                }
            }

            if (dailyReportAdd.delaysArr != null)
            {
                foreach (vm_delays item in dailyReportAdd.delaysArr)
                {
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_dailyReportTimeEntry VALUES({0}, {1}, {2}, {3}) ",

                    passedDailyRptID, item.workDescription, item.workDescriptionCategory, item.hours);

                    foreach (string userNames in item.userNames)
                    {
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

                    passedDailyRptID, item.workDescription, userNames, item.workDescriptionCategory);
                    }
                }
            }

            if (dailyReportAdd.wntyDelaysArr != null)
            {
                foreach (vm_wntyDelays item in dailyReportAdd.wntyDelaysArr)
                {
                    db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_dailyReportTimeEntry VALUES({0}, {1}, {2}, {3}) ",

                    passedDailyRptID, item.workDescription, item.workDescriptionCategory, item.hours);

                    foreach (string userNames in item.userNames)
                    {
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

                    passedDailyRptID, item.workDescription, userNames, item.workDescriptionCategory);
                    }
                }
            }

            return Json(Url.Action("Index", "dailyReportAll"));
        }

        public ActionResult copyDailyReport(int reportID)
        {
            ViewBag.reportID = reportID;

            List<vm_dailyReportByReportID> dailyReportByID = new List<vm_dailyReportByReportID>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_dailyReport.dailyReportID, tbl_dailyReport.jobID, tbl_dailyReport.subJobID, tbl_subJobTypes.description, tbl_dailyReport.date, " +
                "tbl_Jobs.customerContact,tbl_customers.customerName, tbl_customers.address, tbl_dailyReport.equipment, tbl_dailyReport.startTime, " +
                "tbl_dailyReport.endTime, tbl_dailyReport.lunchHours, tbl_customers.customerCode, tbl_dailyReport.dailyReportAuthor, tbl_dailyReport.submissionStatus " +

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
                vm_dailyReportByReportID.subJobID = dr1[2].ToString();
                vm_dailyReportByReportID.description = dr1[3].ToString();
                vm_dailyReportByReportID.date = String.Format("{0:yyyy-MM-dd}", dr1[4]);
                vm_dailyReportByReportID.customerContact = dr1[5].ToString();
                vm_dailyReportByReportID.customerName = dr1[6].ToString();
                vm_dailyReportByReportID.address = dr1[7].ToString();
                vm_dailyReportByReportID.equipment = dr1[8].ToString();
                vm_dailyReportByReportID.startTime = dr1[9].ToString();
                vm_dailyReportByReportID.endTime = dr1[10].ToString();
                vm_dailyReportByReportID.lunchHours = (int)dr1[11];
                vm_dailyReportByReportID.customerCode = dr1[12].ToString();
                //GET JOB CORRESPONDENT NAME RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.jobCorrespondentName = jobCrspdtNameByJobID(vm_dailyReportByReportID.jobID);
                //GET JOB CORRESPONDENT EMAIL ADDRESS RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.jobCorrespondentEmail = jobCrspdtEmailByJobID(vm_dailyReportByReportID.jobID);
                //
                vm_dailyReportByReportID.dailyReportAuthor = dr1[13].ToString();
                vm_dailyReportByReportID.submissionStatus = (int)dr1[14];

                //GET WORK DESCRIPTION RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.workDescArr = getWorkDescRecords(vm_dailyReportByReportID.dailyReportID);
                //GET DELAYS RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.delaysArr = getDelaysRecords(vm_dailyReportByReportID.dailyReportID);
                //GET WARRANTY DELAYS RECORDS FOR THE DAILY REPORT
                vm_dailyReportByReportID.wntyDelaysArr = getWntyDelaysRecords(vm_dailyReportByReportID.dailyReportID);

                dailyReportByID.Add(vm_dailyReportByReportID);//add all of the revelevant data objects to dailyReportByID...
            }

            sqlconn.Close();

            return View(dailyReportByID);//...to be passed to the view

            //return RedirectToAction("Index", "dailyReportAll");//redirects, and the daily report does kick out the records to its view
        }

        //DELETE A TIME ENTRY
        public ActionResult DeleteTimeEntry(vm_dailyReportByReportID timeEntryDelete)
        {
            if (timeEntryDelete.timeEntryDeleteArr.Count >= 1)
            {
                //System.Diagnostics.Debug.WriteLine("size of timeEntryDeleteArr is " + timeEntryDelete.timeEntryDeleteArr.Count);
                foreach (string item in timeEntryDelete.timeEntryDeleteArr)
                {
                    //System.Diagnostics.Debug.WriteLine(item);
                    db.Database.ExecuteSqlCommand("DELETE FROM tbl_dailyReportTimeEntry WHERE timeEntryID=({0})", item);
                }
            }

            //return new EmptyResult();
            return Json(Url.Action("Index", "dailyReportAll"));
        }

        public ActionResult UpdateDailyReportLogInfo (tbl_dailyReport dailyReportLogInfoUpdate)
        {
            //db.Database.ExecuteSqlCommand("Insert into tbl_dailyReport Values({0},{1})",
            //    teamMemberAdd.dailyReportID, teamMemberAdd.userName);

            db.Database.ExecuteSqlCommand(
                "UPDATE tbl_dailyReport " +
                "SET " +

                "jobID = {1}, " +
                "date = {2}, " +
                "subJobID = {3}, " +
                "startTime = {4}, " +
                "endTime = {5}, " +
                "lunchHours = {6}, " +
                "equipment = {7}, " +
                "dailyReportAuthor = {8} " +

                "WHERE dailyReportID = {0}",
                dailyReportLogInfoUpdate.dailyReportID, dailyReportLogInfoUpdate.jobID, dailyReportLogInfoUpdate.date, dailyReportLogInfoUpdate.subJobID,
                dailyReportLogInfoUpdate.startTime, dailyReportLogInfoUpdate.endTime, dailyReportLogInfoUpdate.lunchHours,
                dailyReportLogInfoUpdate.equipment, dailyReportLogInfoUpdate.dailyReportAuthor);

            //return new EmptyResult();
            return Json(Url.Action("Index", "dailyReportAll"));
            //return Json("complete");
        }

        public ActionResult UpdateDailyReportSubmissionStatus(tbl_dailyReport submissionStatusUpdate)
        {
            db.Database.ExecuteSqlCommand(
                "UPDATE tbl_dailyReport " +
                "SET " +

                "submissionStatus = {1} " +

                "WHERE dailyReportID = {0}",
                submissionStatusUpdate.dailyReportID, submissionStatusUpdate.submissionStatus);

            return Json(Url.Action("Index", "dailyReportAll"));
        }

        public ActionResult UpdateDailyReportTimeEntries(vm_dailyReportByReportID dailyReportTimeEntryUpdate)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            int passedDailyRptID = dailyReportTimeEntryUpdate.dailyReportID;

            if (dailyReportTimeEntryUpdate.workDescArr != null)
            {
                foreach (vm_workDesc item in dailyReportTimeEntryUpdate.workDescArr)
                {
                    db.Database.ExecuteSqlCommand(
                        "UPDATE tbl_dailyReportTimeEntry " +
                        "SET " +

                        "workDescription = {1}, " +
                        "hours = {3} " +

                        "WHERE " +
                        "timeEntryID = {0}",

                        item.timeEntryID, item.workDescription, item.workDescriptionCategory, item.hours);

                    if (item.userNamesToAdd != null)
                    { 
                        
                        foreach (string userNames in item.userNamesToAdd)
                    {
                        db.Database.ExecuteSqlCommand(

                    "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES({0}, {1}) ",
                    item.timeEntryID, userNames);
                        }                    
                    }

                    if (item.userNamesToDelete != null)
                    {
                        foreach (string userNames in item.userNamesToDelete)
                        {
                            db.Database.ExecuteSqlCommand(

                            "DELETE FROM tbl_dailyReportTimeEntryUsers " +
                            "WHERE " +

                            "timeEntryID=({0}) AND userName = ({1})",
                            item.timeEntryID, userNames);
                        }
                    }
                }
            }

            if (dailyReportTimeEntryUpdate.delaysArr != null)
            {
                foreach (vm_delays item in dailyReportTimeEntryUpdate.delaysArr)
                {
                    db.Database.ExecuteSqlCommand(
                        "UPDATE tbl_dailyReportTimeEntry " +
                        "SET " +

                        "workDescription = {1}, " +
                        "hours = {3} " +

                        "WHERE " +
                        "timeEntryID = {0}",

                        item.timeEntryID, item.workDescription, item.workDescriptionCategory, item.hours);

                    if (item.userNamesToAdd != null)
                    {

                        foreach (string userNames in item.userNamesToAdd)
                        {
                            db.Database.ExecuteSqlCommand(

                        "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES({0}, {1}) ",
                        item.timeEntryID, userNames);
                        }
                    }

                    if (item.userNamesToDelete != null)
                    {
                        foreach (string userNames in item.userNamesToDelete)
                        {
                            db.Database.ExecuteSqlCommand(

                            "DELETE FROM tbl_dailyReportTimeEntryUsers " +
                            "WHERE " +

                            "timeEntryID=({0}) AND userName = ({1})",
                            item.timeEntryID, userNames);
                        }
                    }
                }
            }

            if (dailyReportTimeEntryUpdate.wntyDelaysArr != null)
            {
                foreach (vm_wntyDelays item in dailyReportTimeEntryUpdate.wntyDelaysArr)
                {
                    db.Database.ExecuteSqlCommand(
                        "UPDATE tbl_dailyReportTimeEntry " +
                        "SET " +

                        "workDescription = {1}, " +
                        "hours = {3} " +

                        "WHERE " +
                        "timeEntryID = {0}",

                        item.timeEntryID, item.workDescription, item.workDescriptionCategory, item.hours);

                    if (item.userNamesToAdd != null)
                    {

                        foreach (string userNames in item.userNamesToAdd)
                        {
                            db.Database.ExecuteSqlCommand(

                        "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES({0}, {1}) ",
                        item.timeEntryID, userNames);
                        }
                    }

                    if (item.userNamesToDelete != null)
                    {
                        foreach (string userNames in item.userNamesToDelete)
                        {
                            db.Database.ExecuteSqlCommand(

                            "DELETE FROM tbl_dailyReportTimeEntryUsers " +
                            "WHERE " +

                            "timeEntryID=({0}) AND userName = ({1})",
                            item.timeEntryID, userNames);
                        }
                    }
                }
            }

            //return new EmptyResult();
            //return RedirectToAction("SalesLanding", "Index");
            return Json(Url.Action("Index", "dailyReportAll"));
        }

        public ActionResult AddTeamMember(tbl_dailyReportUsers teamMemberAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportUsers Values({0},{1})",
                teamMemberAdd.dailyReportID, teamMemberAdd.userName);

            return new EmptyResult();
        }

        public ActionResult DeleteTeamMember(tbl_dailyReportUsers teamMemberDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tbl_dailyReportUsers " +
                "WHERE " +
                "dailyReportID=({0})" +
                "AND userName = ({1})", teamMemberDelete.dailyReportID, teamMemberDelete.userName);

            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        //GET JOB CORRESPONDENT NAME RECORDS FOR THE DAILY REPORT
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

        //GET JOB CORRESPONDENT EMAIL ADDRESS RECORDS FOR THE DAILY REPORT
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

        //GET WORK DESCRIPTION RECORDS FOR THE DAILY REPORT
        public List<vm_workDesc> getWorkDescRecords(int dailyReportID)
        {
            List<vm_workDesc> workDescs = new List<vm_workDesc>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            //begin
            string sqlquery1 =
                "SELECT tbl_dailyReportTimeEntry.dailyReportID, tbl_dailyReportTimeEntry.workDescription, " +
                "tbl_dailyReportTimeEntry.timeEntryID " +

                "FROM tbl_dailyReportTimeEntry " +
                "WHERE " +
                "tbl_dailyReportTimeEntry.dailyReportID = @dailyReportID " +
                "AND " +
                "tbl_dailyReportTimeEntry.workDescriptionCategory = '1'";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.AddWithValue("@dailyReportID", dailyReportID);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_workDesc workDesc = new vm_workDesc();

                workDesc.dailyReportID = (int)dr1[0];
                workDesc.workDescription = dr1[1].ToString();
                workDesc.timeEntryID = (int)dr1[2];

                workDesc.userNames = usersByTimeEntryID(workDesc.timeEntryID);
                workDesc.userShortNames = userShortNamesByTimeEntryID(workDesc.timeEntryID);

                workDescs.Add(workDesc);
            }
            //sqlconn.Close();
            //end
            return workDescs;
        }

        //GET DELAYS RECORDS FOR THE DAILY REPORT
        public List<vm_delays> getDelaysRecords(int dailyReportID)
        {
            List<vm_delays> workDescs = new List<vm_delays>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

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
                vm_delays workDesc = new vm_delays();

                workDesc.dailyReportID = (int)dr1[0];
                workDesc.workDescription = dr1[1].ToString();
                //workDesc.hours = (int)dr1[2];
                //workDesc.hours = (float)dr1[2];
                workDesc.hours = (decimal)dr1[2];
                workDesc.timeEntryID = (int)dr1[3];

                workDesc.userNames = usersByTimeEntryID(workDesc.timeEntryID);
                workDesc.userShortNames = userShortNamesByTimeEntryID(workDesc.timeEntryID);

                workDescs.Add(workDesc);
            }

            return workDescs;
        }

        //GET WARRANTY DELAYS RECORDS FOR THE DAILY REPORT
        public List<vm_wntyDelays> getWntyDelaysRecords(int dailyReportID)
        {
            List<vm_wntyDelays> workDescs = new List<vm_wntyDelays>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_dailyReportTimeEntry.dailyReportID, tbl_dailyReportTimeEntry.workDescription, " +
                "tbl_dailyReportTimeEntry.hours, tbl_dailyReportTimeEntry.timeEntryID " +

                "FROM tbl_dailyReportTimeEntry " +
                "WHERE " +
                "tbl_dailyReportTimeEntry.dailyReportID = @dailyReportID " +
                "AND " +
                "tbl_dailyReportTimeEntry.workDescriptionCategory = '3'";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            sqlcomm1.Parameters.AddWithValue("@dailyReportID", dailyReportID);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                vm_wntyDelays workDesc = new vm_wntyDelays();

                workDesc.dailyReportID = (int)dr1[0];
                workDesc.workDescription = dr1[1].ToString();
                //workDesc.hours = (int)dr1[2];
                workDesc.hours = (decimal)dr1[2];
                workDesc.timeEntryID = (int)dr1[3];

                workDesc.userNames = usersByTimeEntryID(workDesc.timeEntryID);
                workDesc.userShortNames = userShortNamesByTimeEntryID(workDesc.timeEntryID);

                workDescs.Add(workDesc);
            }
            return workDescs;
        }

        //GET THE USER NAMES ATTACHED TO A TIME ENTRY
        public List<string> usersByTimeEntryID(int timeEntryID)
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

        //GET THE USER SHORT NAMES ATTACHED TO A TIME ENTRY
        public List<string> userShortNamesByTimeEntryID(int timeEntryID)
        {
            List<string> userShortNames = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            //string sqlquery1 = "SELECT tbl_Users.shortName " +

            //"FROM " +
            //"tbl_Users " +

            //"INNER JOIN " +
            //"tbl_dailyReportTimeEntryUsers ON " +
            //"tbl_Users.userName = tbl_dailyReportTimeEntryUsers.userName " +

            //"WHERE " +
            //"tbl_dailyReportTimeEntryUsers.timeEntryID = @timeEntryID";

            string sqlquery1 = "SELECT AspNetUsers.shortName " +

            "FROM " +
            "AspNetUsers " +

            "INNER JOIN " +
            "tbl_dailyReportTimeEntryUsers ON " +
            "AspNetUsers.userName = tbl_dailyReportTimeEntryUsers.userName " +

            "WHERE " +
            "tbl_dailyReportTimeEntryUsers.timeEntryID = @timeEntryID";

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
