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
using allpax_service_record.Models.MODEL_TESTING;
using Westwind.Web.Mvc;
using Westwind.Utilities;
using System.Net.Mail;

namespace allpax_service_record.Controllers
{
    public class dailyReportByReportIDPrintController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

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
                "tbl_dailyReport.startTime, tbl_dailyReport.endTime, tbl_dailyReport.lunchHours, tbl_customers.customerCode  " +

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

                dailyReportByID.Add(vm_dailyReportByReportID);
            }

            sqlconn.Close();

            string html = ViewRenderer.RenderView("~/views/dailyReportByReportIDPrint/index.cshtml", dailyReportByID);

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("allpaxtesting@gmail.com", "Allpax_1234");

            string body = html;

            using (var message = new MailMessage("allpaxtesting@gmail.com", "allpaxtesting@gmail.com"))
            {
                message.Subject = "Test";
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);
                }

                //end query
                return View(dailyReportByID);
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
