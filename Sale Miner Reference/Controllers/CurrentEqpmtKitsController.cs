﻿using System;
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
    public class CurrentEqpmtKitsController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();

        // GET: CurrentEqpmtKits
        public ActionResult Index()
        {
            var sql = db.tbl_eqpmt_kits_current.SqlQuery("SELECT * from cmps411.tbl_eqpmt_kits_current").ToList();

            ViewBag.kitID = new SelectList(db.tbl_kit, "kitID", "kitID");

            return View(sql.ToList());
        }

        //begin CMPS 411 controller code
        [HttpPost]
        public ActionResult AddCurrentEqpmt(tbl_eqpmt_kits_current currentEqpmtAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_eqpmt_kits_current Values({0},{1})",
                currentEqpmtAdd.machineID, currentEqpmtAdd.kitID);

            return RedirectToAction("Index");
        }

        //public ActionResult DeleteCurrentEqpmt(tbl_eqpmt_kits_current currentEqpmtDelete)
        //{
        //    db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_eqpmt_kits_current WHERE id=({0})", currentEqpmtDelete.id);
        //    return RedirectToAction("Index");
        //}

        public ActionResult DeleteCurrentEqpmt(tbl_eqpmt_kits_current currentEqpmtDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_eqpmt_kits_current WHERE machineID=({0}) AND " +
                "kitID=({1})", currentEqpmtDelete.machineID, currentEqpmtDelete.kitID);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCurrentEqpmt(tbl_eqpmt_kits_current currentEqpmtUpdate)
        {
            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_eqpmt_kits_current SET machineID={0},kitID={1} WHERE id={2}",
                currentEqpmtUpdate.machineID, currentEqpmtUpdate.kitID, currentEqpmtUpdate.id);

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