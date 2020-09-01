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
    public class EventTypeController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();

        public ActionResult Index()
        {
            var sql = db.tbl_event_type.SqlQuery("SELECT * from cmps411.tbl_event_type").ToList();
            return View(sql.ToList());
        }
        [HttpPost]
        public ActionResult AddEvent(tbl_event_type eventAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_event_type Values({0},{1})",
                eventAdd.eventID, eventAdd.eventType);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteEvent(tbl_event_type eventDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_event_type WHERE id=({0})", eventDelete.id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateEvent(tbl_event_type eventUpdate)
        {
            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_event_type SET eventID={0}, eventType={1} WHERE id={2}",
                eventUpdate.eventID, eventUpdate.eventType, eventUpdate.id);

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
