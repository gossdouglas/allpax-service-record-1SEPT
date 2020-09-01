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
    public class workDescByTimeEntryID : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: customers
        //public ActionResult Index(int dailyReportID)
            public ActionResult Index()
        {
            var sql = db.tbl_dailyReportTimeEntry.SqlQuery("SELECT * from tbl_dailyReportTimeEntry").ToList();

            return View(sql.ToList());
        }

        [HttpPost]
        public ActionResult AddWorkDesc(vm_workDesc workDescAdd)
        {
            //db.Database.ExecuteSqlCommand("Insert into tbl_dailyReportTimeEntry Values({0},{1})",
            //   workDescAdd.dailyReportID, workDescAdd.workDescription); 

            db.Database.ExecuteSqlCommand("IF NOT EXISTS(SELECT * FROM tbl_dailyReportTimeEntry WHERE dailyReportID = {0}) " +
                "BEGIN" +

                "DECLARE @id INT" +
                "INSERT INTO tbl_dailyReportTimeEntry VALUES({0}, {1})" +
                "SET @id = SCOPE_IDENTITY()" +
                "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES(@id, {2})" +

                "END" +

                "ELSE" +
                "BEGIN" +

                "SET @timeEntryID = " +
                    "(SELECT tbl_dailyReportTimeEntry.timeEntryID" +

                    "FROM tbl_dailyReportTimeEntry" +

                    "WHERE" +

                    "tbl_dailyReportTimeEntry.dailyReportID like {0})" +
                    "INSERT INTO tbl_dailyReportTimeEntryUsers(timeEntryID, userName) VALUES(@timeEntryID, {2})" +

                "END)",
                workDescAdd.dailyReportID, workDescAdd.workDescription, workDescAdd.userName);

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
