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

namespace allpax_service_record.Controllers
{
    [Authorize]
    public class dailyReportAllController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: customers
        public ActionResult Index()
        {
            //return View(db.tbl_customers.ToList());
            //var sql = db.tbl_dailyReport.SqlQuery("SELECT * from tbl_dailyReport").ToList();
            List<vm_dailyReportViewAll> list  = db.Database.SqlQuery<vm_dailyReportViewAll>("SELECT tbl_dailyReport.dailyReportID, tbl_Jobs.active, tbl_dailyReport.date, tbl_dailyReport.jobID, " +
                "tbl_subJobTypes.description, tbl_customers.customerName, tbl_customers.address FROM tbl_dailyReport " +

                    "INNER JOIN " +
                    "tbl_Jobs ON tbl_Jobs.jobID = tbl_dailyReport.jobID " +
                    "INNER JOIN " +
                    "tbl_customers ON tbl_customers.customerCode = tbl_Jobs.customerCode " +
                    "INNER JOIN " +
                    "tbl_jobSubJobs ON tbl_jobSubJobs.jobID = tbl_Jobs.jobID " +
                    "INNER JOIN " +
                    "tbl_subJobTypes ON tbl_subJobTypes.subJobID = tbl_jobSubJobs.subJobID " +
                    "WHERE " +
                    "tbl_dailyReport.subJobID = tbl_subJobTypes.subJobID").ToList();

            return View(list.ToList()); 
        }

        public ActionResult Filtered(string startDate, string endDate)
        {
            //return View(db.tbl_customers.ToList());
            //var sql = db.tbl_dailyReport.SqlQuery("SELECT * from tbl_dailyReport").ToList();

            string sql = @"SELECT tbl_dailyReport.dailyReportID, tbl_Jobs.active, tbl_dailyReport.date, tbl_dailyReport.jobID, " +
            "tbl_subJobTypes.description, tbl_customers.customerName, tbl_customers.address FROM tbl_dailyReport " +

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
            "tbl_dailyReport.date >= {0} " +
            "AND " +
            "tbl_dailyReport.date <= {1}";

            List<vm_dailyReportViewAll> list = db.Database.SqlQuery<vm_dailyReportViewAll>
            (sql, startDate, endDate).ToList();

            //List<vm_dailyReportViewAll> list = db.Database.SqlQuery<vm_dailyReportViewAll>
            //("SELECT tbl_dailyReport.dailyReportID, tbl_Jobs.active, tbl_dailyReport.date, tbl_dailyReport.jobID, " +
            //"tbl_subJobTypes.description, tbl_customers.customerName, tbl_customers.address FROM tbl_dailyReport " +

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
            //"tbl_dailyReport.date >= @p0 " +
            //"AND " +
            //"tbl_dailyReport.date <= @p1").ToList();

            //return View(list.ToList());
            return View(list);
        }

        //begin CMPS 411 controller code
        [HttpPost]
        public ActionResult AddDailyReport(tbl_dailyReport dailyReportAdd)
        {
             db.Database.ExecuteSqlCommand("Insert into tbl_dailyReport Values({0},{1},{2},{3},{4},{5},{6})",
                dailyReportAdd.jobID, dailyReportAdd.date, dailyReportAdd.subJobID, dailyReportAdd.startTime, dailyReportAdd.endTime, dailyReportAdd.lunchHours, dailyReportAdd.equipment); 
            //return new EmptyResult();
            return RedirectToAction("Home", "Index");
            //return Redirect("/Home");
        }

        public ActionResult DeleteCustomer(tbl_dailyReport custDelete)
        {
            //db.Database.ExecuteSqlCommand("DELETE FROM tbl_customers WHERE id=({0})", custDelete.id);

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
