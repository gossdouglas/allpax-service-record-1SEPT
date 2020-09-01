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
    public class KitsAvlblMgmtController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();

        public ActionResult Index()
        {
            ViewBag.eqpmtType = new SelectList(db.tbl_eqpmt_type.OrderBy(x => x.eqpmtType), "eqpmtType", "eqpmtType");
            ViewBag.model = new SelectList(db.tbl_eqpmt_type.OrderBy(x => x.model), "model", "model");
            ViewBag.kitID = new SelectList(db.tbl_kit.OrderBy(x => x.kitID), "kitID", "kitID");

            var sql = db.tbl_eqpmt_kits_avlbl.SqlQuery("SELECT * from cmps411.tbl_eqpmt_kits_avlbl").ToList();

            return View(sql.ToList());
        }

        //begin CMPS 411 controller code
        [HttpPost]
        public ActionResult AddAvlblKit(tbl_eqpmt_kits_avlbl avlblKitAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_eqpmt_kits_avlbl Values({0},{1},{2})",
                avlblKitAdd.eqpmtType, avlblKitAdd.model, avlblKitAdd.kitID);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAvlblKit(tbl_eqpmt_kits_avlbl avlblKitDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_eqpmt_kits_avlbl WHERE id=({0})", avlblKitDelete.id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateAvlblKit(tbl_eqpmt_kits_avlbl avlblKitUpdate)
        {
            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_eqpmt_kits_avlbl SET eqpmtType={0},model={1},kitID={2} WHERE id={3}",
                avlblKitUpdate.eqpmtType, avlblKitUpdate.model, avlblKitUpdate.kitID, avlblKitUpdate.id);
            return new EmptyResult();
        }
        //end CMPS 411 controller code

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
