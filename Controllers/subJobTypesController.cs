using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_service_record.Models;
using System.Configuration;
using System.Data.SqlClient;
namespace allpax_service_record.Controllers
{
    public class subJobTypesController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: subJobTypes
        public ActionResult Index()
        {
            List<tbl_subJobTypes> subJobTypes = new List<tbl_subJobTypes>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_subJobTypes.subJobID, tbl_subJobTypes.description " +
                "FROM tbl_subJobTypes";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                tbl_subJobTypes subJobType = new tbl_subJobTypes();

                subJobType.subJobID = (byte)dr1[0];
                subJobType.description = dr1[1].ToString();
                subJobTypes.Add(subJobType);
            }
            sqlconn.Close();
            return View(subJobTypes);

            //return View(db.tbl_subJobTypes.ToList());
        }

        [HttpPost]
        public ActionResult AddSubJobType(tbl_subJobTypes subJobTypeAdd)
        {
            try
            {
                db.Database.ExecuteSqlCommand("Insert into tbl_subJobTypes Values({0})",
               subJobTypeAdd.description);

                return Json(Url.Action("Index", "SubJobTypes"));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }           
        }

        public ActionResult UpdateSubJobType(tbl_subJobTypes subJobTypeUpdate)
        {
            try
            {
                db.Database.ExecuteSqlCommand(
                "UPDATE tbl_subJobTypes " +
                "SET " +

                "description = {0} " +

                "WHERE subJobID = {1}",
                subJobTypeUpdate.description, subJobTypeUpdate.subJobID);

                return Json(Url.Action("Index", "SubJobTypes"));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }           
        }

        public ActionResult DeleteSubJobType(tbl_subJobTypes subJobTypeDelete)
        {
            try
            {
                db.Database.ExecuteSqlCommand("DELETE FROM tbl_subJobTypes WHERE subJobID=({0})", subJobTypeDelete.subJobID);

                return Json(Url.Action("Index", "SubJobTypes"));
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }            
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
