using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_sale_miner.Models;

namespace allpax_sale_miner.Controllers
{
    public class CustomerEventController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();
        public ActionResult Index()
        {
            var sql = db.tbl_customer_event.SqlQuery("SELECT * from cmps411.tbl_customer_event").ToList();

            return View(sql.ToList());
        }
        [HttpPost]
        public ActionResult AddEvent(tbl_customer_event eventAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_customer_event Values({0},{1},{2},{3},{4})",
                eventAdd.customerCode, eventAdd.eventType, eventAdd.startDate, eventAdd.endDate, eventAdd.eventID);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteEvent(tbl_customer_event eventDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_customer_event WHERE id=({0})", eventDelete.id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateEvent(tbl_customer_event eventUpdate)
        {          
            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_customer_event SET customerCode={0},eventType={1},startDate={2}, endDate={3}, eventID={4} WHERE id={5}",
                eventUpdate.customerCode, eventUpdate.eventType, eventUpdate.startDate, eventUpdate.endDate, eventUpdate.eventID, eventUpdate.id);

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
