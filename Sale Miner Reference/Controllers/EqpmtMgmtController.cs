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
    public class EqpmtMgmtController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();
        public ActionResult Index()
        {
            var sql = db.tbl_eqpmt_type.SqlQuery("SELECT * from cmps411.tbl_eqpmt_type").ToList();
            ViewBag.eqpmtType = new SelectList(db.tbl_eqpmt_type_mgmt.OrderBy(x => x.eqpmtType), "eqpmtType", "eqpmtType");

            return View(sql.ToList());
        }
        [HttpPost]
        public ActionResult AddEqpmt(tbl_eqpmt_type eqpmtAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_eqpmt_type Values({0},{1},{2})",
                eqpmtAdd.eqpmtType, eqpmtAdd.model, eqpmtAdd.description);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteEqpmt(tbl_eqpmt_type eqpmtDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_eqpmt_type WHERE id=({0})", eqpmtDelete.id);
            return RedirectToAction("Index");
        }
        public ActionResult UpdateEqpmt(tbl_eqpmt_type eqpmtUpdate)
        {
            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_eqpmt_type SET eqpmtType={0},model={1},description={2} WHERE id={3}",
                eqpmtUpdate.eqpmtType, eqpmtUpdate.model, eqpmtUpdate.description, eqpmtUpdate.id);

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
