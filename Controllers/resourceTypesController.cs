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
    public class resourceTypesController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        public ActionResult Index()
        {
            List<tbl_resourceTypes> resourceTypes = new List<tbl_resourceTypes>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            sqlconn.Open();

            string sqlquery1 =
                "SELECT tbl_resourceTypes.resourceTypeID, tbl_resourceTypes.resourceType, tbl_resourceTypes.description, tbl_resourceTypes.rate " +
                "FROM tbl_resourceTypes";

            SqlCommand sqlcomm1 = new SqlCommand(sqlquery1, sqlconn);
            SqlDataAdapter sda1 = new SqlDataAdapter(sqlcomm1);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                tbl_resourceTypes resourceType = new tbl_resourceTypes();

                resourceType.resourceTypeID = (byte)dr1[0];
                resourceType.resourceType = dr1[1].ToString();
                resourceType.description = dr1[2].ToString();
                resourceType.rate = (decimal)dr1[3];
                resourceTypes.Add(resourceType);
            }
            sqlconn.Close();
            return View(resourceTypes);
        }

        [HttpPost]
        public ActionResult AddResourceType(tbl_resourceTypes resourceTypeAdd)
        {
            db.Database.ExecuteSqlCommand("Insert into tbl_resourceTypes Values({0},{1},{2})",
               resourceTypeAdd.resourceType, resourceTypeAdd.description, resourceTypeAdd.rate);

            return Json(Url.Action("Index", "ResourceTypes"));
        }

        public ActionResult UpdateResourceType(tbl_resourceTypes resourceUpdate)
        {
            db.Database.ExecuteSqlCommand(
                "UPDATE tbl_resourceTypes " +
                "SET " +

                "resourceType = {0}, " +
                "description = {1}, " +
                "rate = {2} " +

                "WHERE resourceTypeID = {3}",
                resourceUpdate.resourceType, resourceUpdate.description, resourceUpdate.rate, resourceUpdate.resourceTypeID);

            return Json(Url.Action("Index", "ResourceTypes"));
        }

        public ActionResult DeleteResourceType(tbl_resourceTypes resourceTypeDelete)
        {
            db.Database.ExecuteSqlCommand("DELETE FROM tbl_resourceTypes WHERE resourceTypeID=({0})", resourceTypeDelete.resourceTypeID);

            return Json(Url.Action("Index", "ResourceTypes"));
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
