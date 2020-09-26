﻿using System;
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
        public ActionResult AddDailyReport(tbl_dailyReport dailyReportAdd)
        {
             db.Database.ExecuteSqlCommand("Insert into tbl_dailyReport Values({0},{1},{2},{3},{4},{5},{6},{7})",
                dailyReportAdd.jobID, dailyReportAdd.date, dailyReportAdd.subJobID, dailyReportAdd.startTime, dailyReportAdd.endTime, 
                dailyReportAdd.lunchHours, dailyReportAdd.equipment, dailyReportAdd.dailyReportAuthor);

            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<string> new_dailyRptID = new List<string>();

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
                    new_dailyRptID.Add(rdr["dailyReportID"].ToString());
                }
            }

            db.Database.ExecuteSqlCommand("spCopyDailyRpt @p0, @p1", dailyReportAdd.dailyReportID, new_dailyRptID[0]);

            return new EmptyResult();
        }

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

            return View();
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
