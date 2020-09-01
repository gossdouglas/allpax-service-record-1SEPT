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
using System.Security.Cryptography;

namespace allpax_sale_miner.Controllers
{
    public class SalesCustomerController : Controller
    {

        public ActionResult Index(string customerCode)
        {
            List<vm_SalesCustomer> SalesCustomer = new List<vm_SalesCustomer>();

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

            //begin query for customer equipment        
            string sqlquery =
                "SELECT cmps411.tbl_customer_eqpmt.jobNum, cmps411.tbl_customer_eqpmt.customerCode, " +
                "cmps411.tbl_customer_eqpmt.model, cmps411.tbl_customer_eqpmt.machineID, cmps411.tbl_customer.name " +
                "FROM " +
                "cmps411.tbl_customer_eqpmt " +
                "INNER JOIN " +
                "cmps411.tbl_customer ON cmps411.tbl_customer_eqpmt.customerCode = cmps411.tbl_customer.customerCode "+
                "WHERE cmps411.tbl_customer_eqpmt.customerCode LIKE @customerCode";
            //end query for customer equipment

            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlcomm.Parameters.AddWithValue("@customerCode", customerCode);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                vm_SalesCustomer vm_SalesCustomer = new vm_SalesCustomer();

                vm_SalesCustomer.jobNo = dr[0].ToString();
                vm_SalesCustomer.customerCode = dr[1].ToString();
                vm_SalesCustomer.model = dr[2].ToString();
                vm_SalesCustomer.machineID = dr[3].ToString();
                vm_SalesCustomer.name = dr[4].ToString();

                vm_SalesCustomer.jobNoList = jobNoList(vm_SalesCustomer.customerCode);

                vm_SalesCustomer.kitsCurrent = kitsCurrent(vm_SalesCustomer.customerCode, vm_SalesCustomer.machineID);
                vm_SalesCustomer.kitsAvlblbNotInstld = kitsAvlblbNotInstld(vm_SalesCustomer.customerCode, vm_SalesCustomer.jobNo, vm_SalesCustomer.machineID);
                SalesCustomer.Add(vm_SalesCustomer);
            }
           
            sqlconn.Close();
                      
             return View(SalesCustomer);
        }

        //begin job list
        public List<string> jobNoList(string customerCode)
        {
            List<string> jl = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpax_sale_minerEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            //begin query for currently installed kits by machine 
            string sqlquery4 = "SELECT DISTINCT cmps411.tbl_customer_eqpmt.jobNum, cmps411.tbl_customer_eqpmt.customerCode " +
                "FROM cmps411.tbl_customer_eqpmt " +
                "WHERE cmps411.tbl_customer_eqpmt.customerCode like @customerCode";
            //end query for currently installed kits by machine 

            SqlCommand sqlcomm4 = new SqlCommand(sqlquery4, sqlconn);
            sqlcomm4.Parameters.Add(new SqlParameter("customerCode", customerCode));
            SqlDataAdapter sda4 = new SqlDataAdapter(sqlcomm4);
            DataTable dt4 = new DataTable();
            sda4.Fill(dt4);
            foreach (DataRow dr4 in dt4.Rows)
            {
                jl.Add(dr4[0].ToString());
            }
            return jl;
        }

        //begin add kitsCurrent
        public List<string> kitsCurrent(string customerCode, string machineID)
        {
            List<string> kc = new List<string>();

            string mainconn = ConfigurationManager.ConnectionStrings["allpax_sale_minerEntities"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            //begin query for currently installed kits by machine 
            string sqlquery2 = "SELECT cmps411.tbl_customer_eqpmt.customerCode, cmps411.tbl_eqpmt_kits_current.machineID, cmps411.tbl_eqpmt_kits_current.kitID " +
                "FROM cmps411.tbl_customer_eqpmt " +
                "INNER JOIN cmps411.tbl_eqpmt_kits_current ON cmps411.tbl_customer_eqpmt.machineID = tbl_eqpmt_kits_current.machineID " +
                "WHERE cmps411.tbl_customer_eqpmt.customerCode = @customerCode AND cmps411.tbl_eqpmt_kits_current.machineID = @machineID";
            //end query for currently installed kits by machine 

            SqlCommand sqlcomm2 = new SqlCommand(sqlquery2, sqlconn);
            sqlcomm2.Parameters.Add(new SqlParameter("customerCode", customerCode));
            sqlcomm2.Parameters.Add(new SqlParameter("machineID", machineID));
            SqlDataAdapter sda2 = new SqlDataAdapter(sqlcomm2);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            foreach (DataRow dr2 in dt2.Rows)
            {
                kc.Add(dr2[2].ToString());              
            }
            return kc;            
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