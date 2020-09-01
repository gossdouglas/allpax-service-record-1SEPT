using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_sale_miner.Models;

namespace allpax_sale_miner.Controllers
{
    public class CustomerEqpmtMgmtController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();

        // GET: CustomerEvent
        public ActionResult Index()
        {
            var sql = db.tbl_customer_eqpmt.SqlQuery("SELECT * from cmps411.tbl_customer_eqpmt").ToList();

            return View(sql.ToList());
        }
        //begin CMPS 411 controller code
        [HttpPost]
        public ActionResult AddCustEqpmt(tbl_customer_eqpmt custEqpmtAdd)
        {
            //make sure that the sql db fields are lined up as shown here in the logic.  i think the line below is just doing a dump.
            //i think this applies only sql adds
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_customer_eqpmt Values({0},{1},{2}, {3}, {4})",
                custEqpmtAdd.customerCode, custEqpmtAdd.eqpmtType, custEqpmtAdd.model, custEqpmtAdd.machineID, custEqpmtAdd.jobNum);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCustEqpmt(tbl_customer_eqpmt custEqpmtDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_customer_eqpmt WHERE id=({0})", custEqpmtDelete.id);
            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        public ActionResult UpdateCustEqpmt(tbl_customer_eqpmt custEqpmtUpdate)
        {
            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_customer_eqpmt SET customerCode={1},machineID={2}, jobNum={3}, eqpmtType={4}, model={5}  WHERE id={0}",
                custEqpmtUpdate.id, custEqpmtUpdate.customerCode, custEqpmtUpdate.machineID, custEqpmtUpdate.jobNum, custEqpmtUpdate.eqpmtType, custEqpmtUpdate.model);

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
