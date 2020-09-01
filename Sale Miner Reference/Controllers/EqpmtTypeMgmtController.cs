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
    public class EqpmtTypeMgmtController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();
        public ActionResult Index()
        {
            var sql = db.tbl_eqpmt_type_mgmt.SqlQuery("SELECT * from cmps411.tbl_eqpmt_type_mgmt").ToList();
            return View(sql.ToList());
        }
        [HttpPost]
        public ActionResult AddEqpmtType(tbl_eqpmt_type_mgmt eqpmtTypeAdd)
        {
            db.Database.ExecuteSqlCommand("INSERT into cmps411.tbl_eqpmt_type_mgmt(eqpmtType) VALUES(@p0)", @eqpmtTypeAdd.eqpmtType);

            return new EmptyResult();
        }

        public ActionResult DeleteEqpmtType(tbl_eqpmt_type_mgmt eqpmtTypeDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_eqpmt_type_mgmt WHERE id=({0})", eqpmtTypeDelete.id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateEqpmtType(tbl_eqpmt_type_mgmt eqpmtTypeUpdate)
        {
            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_eqpmt_type_mgmt SET eqpmtType={1} WHERE id={0}",
                eqpmtTypeUpdate.id, eqpmtTypeUpdate.eqpmtType);
            return RedirectToAction("Index");
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
