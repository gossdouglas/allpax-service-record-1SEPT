using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: customers
        public ActionResult Index()
        {
            //return View(db.tbl_customers.ToList());
            var sql = db.tbl_dailyReport.SqlQuery("SELECT * from tbl_dailyReport").ToList();
           
            return View(sql.ToList()); 
        }

        public ActionResult copyDailyReport(string jobID, string subJobID, string customerName, string location)
        {
            ViewBag.jobiD = jobID;
            ViewBag.subJobID = subJobID;
            ViewBag.customerName = customerName;
            ViewBag.location = subJobID;

            //return View(db.tbl_customers.ToList());
            //var sql = db.tbl_dailyReport.SqlQuery("SELECT * from tbl_dailyReport").ToList();

            //return View(sql.ToList());
            return View();
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
