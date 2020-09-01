using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using allpax_sale_miner.ViewModels;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using allpax_sale_miner.Models;
using System.Dynamic;

namespace allpax_sale_miner.Controllers
{
    public class SalesOppsController : Controller
    {
        private allpax_sale_minerEntities db = new allpax_sale_minerEntities();
        public ActionResult Index(String customerCode, string name, string address, string city, string state)
        {
            ViewBag.customerCode = customerCode;
            ViewBag.name = name;
            ViewBag.address = address;
            ViewBag.city = city;
            ViewBag.state = state;

            List<machinesW_kitsAvlbl_BnotInstalled> mWkaBni = new List<machinesW_kitsAvlbl_BnotInstalled>();
            string mainconn = ConfigurationManager.ConnectionStrings["allpax_sale_minerEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            // begin empty and build custEqpmtWkitsAvlbl and custEqpmtWkitsInstld tables  
            //this is handled by a stored procedure on the sql server named dbo.bldSalesOppsTables
            sqlconn.Open();
            SqlCommand sqlcomm1 = new SqlCommand("dbo.bldSalesOppsTables", sqlconn);
            sqlcomm1.CommandType = System.Data.CommandType.StoredProcedure;
            sqlcomm1.ExecuteNonQuery();
            sqlconn.Close();
            //end empty and build custEqpmtWkitsAvlbl and custEqpmtWkitsInstld tables 

            //begin query for kits available, but not installed
            string sqlquery2 =
                "SELECT DISTINCT custEqpmtWkitsAvlbl.customerCode_cEqpmt, custEqpmtWkitsAvlbl.jobNum_cEqpmt, custEqpmtWkitsAvlbl.eqpmtType_cEqpmt, " +
                "cmps411.custEqpmtWkitsAvlbl.model_cEqpmt, cmps411.custEqpmtWkitsAvlbl.machineID_cEqpmt " +

                "FROM cmps411.custEqpmtWkitsAvlbl " +
                "LEFT JOIN cmps411.custEqpmtWkitsInstld ON " +
                "cmps411.custEqpmtWkitsAvlbl.machineID_cEqpmt = cmps411.custEqpmtWkitsInstld.machineID_kitsCurrent " +
                "AND cmps411.custEqpmtWkitsAvlbl.kitID_kitsAvlbl = cmps411.custEqpmtWkitsInstld.kitID_kitsCurrent " +
                "WHERE " +
                "custEqpmtWkitsInstld.machineID_kitsCurrent is NULL " +
                "AND " +
                "cmps411.custEqpmtWkitsInstld.kitID_kitsCurrent is NULL "+

                "AND custEqpmtWkitsAvlbl.customerCode_cEqpmt=@customerCode";
           
            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn);
            sqlcomm2.Parameters.AddWithValue("@customerCode", customerCode);
            SqlDataAdapter sda2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            foreach (DataRow dr2 in dt2.Rows)
            {
                machinesW_kitsAvlbl_BnotInstalled mWkitsAvlblBnotInstalled = new machinesW_kitsAvlbl_BnotInstalled();

                mWkitsAvlblBnotInstalled.customerCode = dr2[0].ToString();
                mWkitsAvlblBnotInstalled.jobNo = dr2[1].ToString();
                mWkitsAvlblBnotInstalled.eqpmtType = dr2[2].ToString();
                mWkitsAvlblBnotInstalled.model = dr2[3].ToString();
                mWkitsAvlblBnotInstalled.machineID = dr2[4].ToString();
                //mWkitsAvlblBnotInstalled.kitsAvlbl_kitID = dr2[5].ToString();
                mWkitsAvlblBnotInstalled.kitsAvlblbNotInstld = kitsAvlblbNotInstld(mWkitsAvlblBnotInstalled.customerCode, mWkitsAvlblBnotInstalled.jobNo, mWkitsAvlblBnotInstalled.machineID);

                mWkaBni.Add(mWkitsAvlblBnotInstalled);
            }

            //end query for kits available, but not installed
            return View(mWkaBni);
        }
        public List<string> kitsAvlblbNotInstld(string customerCode, string jobNo, string machineID)
        {
            List<string> mWkaBni = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpax_sale_minerEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            //begin query for kits available but not installed by machine
            string sqlquery3 = "SELECT custEqpmtWkitsAvlbl.customerCode_cEqpmt, custEqpmtWkitsAvlbl.jobNum_cEqpmt, custEqpmtWkitsAvlbl.eqpmtType_cEqpmt, " +
                "cmps411.custEqpmtWkitsAvlbl.model_cEqpmt, cmps411.custEqpmtWkitsAvlbl.machineID_cEqpmt, kitID_kitsAvlbl " +
                "FROM cmps411.custEqpmtWkitsAvlbl " +
                "LEFT JOIN cmps411.custEqpmtWkitsInstld " +
                "ON cmps411.custEqpmtWkitsAvlbl.machineID_cEqpmt = cmps411.custEqpmtWkitsInstld.machineID_kitsCurrent " +
                "AND cmps411.custEqpmtWkitsAvlbl.kitID_kitsAvlbl = cmps411.custEqpmtWkitsInstld.kitID_kitsCurrent " +
                "WHERE custEqpmtWkitsInstld.machineID_kitsCurrent is NULL " +
                "AND cmps411.custEqpmtWkitsInstld.kitID_kitsCurrent is NULL " +
                "AND custEqpmtWkitsAvlbl.customerCode_cEqpmt = @customerCode " +
                "AND custEqpmtWkitsAvlbl.jobNum_cEqpmt = @jobNo " +
                "AND cmps411.custEqpmtWkitsAvlbl.machineID_cEqpmt = @machineID";
            //end query for kits available but not installed by machine

            SqlCommand sqlcomm3 = new SqlCommand(sqlquery3, sqlconn);
            sqlcomm3.Parameters.Add(new SqlParameter("customerCode", customerCode));
            sqlcomm3.Parameters.Add(new SqlParameter("jobNo", jobNo));
            sqlcomm3.Parameters.Add(new SqlParameter("machineID", machineID));
            SqlDataAdapter sda3 = new SqlDataAdapter(sqlcomm3);
            DataTable dt3 = new DataTable();
            sda3.Fill(dt3);
            foreach (DataRow dr3 in dt3.Rows)
            {
                mWkaBni.Add(dr3[5].ToString());
            }
            return mWkaBni;
        }

    }
}
