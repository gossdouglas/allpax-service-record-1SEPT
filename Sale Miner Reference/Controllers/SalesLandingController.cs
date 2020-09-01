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
    public class SalesLandingController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();

        // GET: CustomerMgmt
        public ActionResult Index()
        {
            //allpax_sale_minerEntities entities = new allpax_sale_minerEntities();
            //List<tbl_customer> custMgmt = entities.tbl_customer.ToList();

            //return View(custMgmt.ToList());


            var sql = db.tbl_customer.SqlQuery("SELECT * from cmps411.tbl_customer").ToList();
          
            return View(sql.ToList());
            
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
