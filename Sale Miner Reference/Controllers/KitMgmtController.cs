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
    public class KitMgmtController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();

        // GET: KitMgmt
        public ActionResult Index()
        {
            var sql = db.tbl_kit.SqlQuery("SELECT * from cmps411.tbl_kit").ToList();

            return View(sql.ToList());
        }
        //begin CMPS 411 controller code
        [HttpPost]
        public ActionResult AddKit(tbl_kit kitAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into cmps411.tbl_kit Values ({0},{1},{2}," +
                "{3},{4}," +
                "{5},{6}," +
                "{7},{8}," +
                "{9},{10}," +
                "{11}, {12}," +
                "'', '', " +//not used
                "'', '', " +//not used
                "'', ''," +//not used
                "'', ''," +//not used
                "'', ''," +//not used
                "{23},{24},{25})",

                kitAdd.kitID, kitAdd.description, kitAdd.filePath, 
                kitAdd.descKitItem_1, kitAdd.partNoKitItem_1, 
                kitAdd.descKitItem_2, kitAdd.partNoKitItem_2, 
                kitAdd.descKitItem_3, kitAdd.partNoKitItem_3, 
                kitAdd.descKitItem_4, kitAdd.partNoKitItem_4, 
                kitAdd.descKitItem_5, kitAdd.partNoKitItem_5, 
                kitAdd.descKitItem_6, kitAdd.partNoKitItem_6, 
                kitAdd.descKitItem_7, kitAdd.partNoKitItem_7, 
                kitAdd.descKitItem_8, kitAdd.partNoKitItem_8, 
                kitAdd.descKitItem_9, kitAdd.partNoKitItem_9, 
                kitAdd.descKitItem_10, kitAdd.partNoKitItem_10,
                kitAdd.infoPackage, kitAdd.mechDrawing, kitAdd.priceSheet);

            return new EmptyResult();
        }

        public ActionResult DeleteKit(tbl_kit kitDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM cmps411.tbl_kit WHERE id=({0})", kitDelete.id);
            return RedirectToAction("Index");
        }
        public ActionResult UpdateKit(tbl_kit kitUpdate)
        {

            db.Database.ExecuteSqlCommand("UPDATE cmps411.tbl_kit SET kitID={0},description={1},filePath={2}, " +
                "descKitItem_1={4}, partNoKitItem_1={5}, " +
                "descKitItem_2={6}, partNoKitItem_2={7}, " +
                "descKitItem_3={8}, partNoKitItem_3={9}, " +
                "descKitItem_4={10}, partNoKitItem_4={11}, " +
                "descKitItem_5={12}, partNoKitItem_5={13}, " +
                "descKitItem_6='', partNoKitItem_6='', " +//not used
                "descKitItem_7='', partNoKitItem_7='', " +//not used
                "descKitItem_8='', partNoKitItem_8='', " +//not used
                "descKitItem_9='', partNoKitItem_9='', " +//not used
                "descKitItem_10='', partNoKitItem_10='', " +//not used

                "infoPackage={24}, mechDrawing={25}, priceSheet={26} " +

                "WHERE id={3}",

                kitUpdate.kitID, kitUpdate.description, kitUpdate.filePath, kitUpdate.id, 
                kitUpdate.descKitItem_1, kitUpdate.partNoKitItem_1,
                kitUpdate.descKitItem_2, kitUpdate.partNoKitItem_2,
                kitUpdate.descKitItem_3, kitUpdate.partNoKitItem_3,
                kitUpdate.descKitItem_4, kitUpdate.partNoKitItem_4,
                kitUpdate.descKitItem_5, kitUpdate.partNoKitItem_5,
                kitUpdate.descKitItem_6, kitUpdate.partNoKitItem_6,//not used
                kitUpdate.descKitItem_7, kitUpdate.partNoKitItem_7,//not used
                kitUpdate.descKitItem_8, kitUpdate.partNoKitItem_8,//not used
                kitUpdate.descKitItem_9, kitUpdate.partNoKitItem_9,//not used
                kitUpdate.descKitItem_10, kitUpdate.partNoKitItem_10,//not used
                kitUpdate.infoPackage, kitUpdate.mechDrawing, kitUpdate.priceSheet);

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
