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
    public class CustomerMgmtController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();

        // GET: CustomerMgmt
        public ActionResult Index()
        {
            var sql = db.tbl_customer.SqlQuery("SELECT * from cmps411.tbl_customer").ToList();
          
            return View(sql.ToList());            
        }
                  
        //begin CMPS 411 controller code
        [HttpPost]
        public ActionResult AddCustomer(tbl_customer customerAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_customer Values({0},{1},{2}, {3}, {4}, {5})", 
                customerAdd.customerCode, customerAdd.name, customerAdd.address, customerAdd.city, customerAdd.state, customerAdd.zipCode);

            return new EmptyResult();
            //return RedirectToAction("SalesLanding", "Index");
        }

        public ActionResult DeleteCustomer(tbl_customer custDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_customer WHERE id=({0})", custDelete.id);

            return RedirectToAction("Index");
        } 
        public ActionResult UpdateCustomer(tbl_customer custUpdate)
        {
          db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_customer SET customerCode={0},name={1},address={2}, city={3}, state={4}, zipCode={5} WHERE id={6}", 
                custUpdate.customerCode, custUpdate.name, custUpdate.address, custUpdate.city, custUpdate.state, custUpdate.zipCode, custUpdate.id);

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
