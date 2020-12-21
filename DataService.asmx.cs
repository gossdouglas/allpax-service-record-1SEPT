using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using allpax_service_record.Models.Dropdown_Models;
using allpax_service_record.Models;
using allpax_service_record.Models.View_Models;

namespace allpax_service_record
{
    /// <summary>
    /// Summary description for DataService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DataService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public void GetAllJobNos()
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<dpdwn_jobID> jobIDs = new List<dpdwn_jobID>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllJobNos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    dpdwn_jobID jobID = new dpdwn_jobID();
                    jobID.jobID = rdr["jobID"].ToString();
                    jobIDs.Add(jobID);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(jobIDs));
        }

        [WebMethod]
        public void GetCustomerInfoByJobID(string jobID)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<vm_customerInfo> customerInfos = new List<vm_customerInfo>();
            using (SqlConnection con = new SqlConnection(cs))
            {

                con.Open();

                SqlCommand cmd = new SqlCommand("spGetCustomerInfoByJobID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@jobID",
                    Value = jobID
                };
                cmd.Parameters.Add(param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    vm_customerInfo customerInfo = new vm_customerInfo();
                    customerInfo.customerCode = rdr["customerCode"].ToString();
                    customerInfo.customerName = rdr["customerName"].ToString();
                    customerInfo.address = rdr["address"].ToString();
                    customerInfo.customerContact= rdr["customerContact"].ToString();
                    customerInfos.Add(customerInfo);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(customerInfos));
        }

        [WebMethod]
        public void GetJobCrspdtInfoByJobID (string jobID)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<vm_jobCrspdtInfo> jobCrspdtInfos = new List<vm_jobCrspdtInfo>();
            using (SqlConnection con = new SqlConnection(cs))
            {

                con.Open();

                SqlCommand cmd = new SqlCommand("spGetJobCrspdtInfoByJobID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@jobID",
                    Value = jobID
                };
                cmd.Parameters.Add(param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    vm_jobCrspdtInfo jobCrspdtInfo = new vm_jobCrspdtInfo();
                    jobCrspdtInfo.jobCrspdtName = rdr["name"].ToString();
                    jobCrspdtInfo.jobCrspdtEmail = rdr["email"].ToString();
                    jobCrspdtInfos.Add(jobCrspdtInfo);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(jobCrspdtInfos));
        }

        [WebMethod]
        public void GetAllTeamNames(string userName)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<dpdwn_teamNames> teamNames = new List<dpdwn_teamNames>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllTeamNames", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@userName",
                    Value = userName
                };
                cmd.Parameters.Add(param);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    dpdwn_teamNames teamName = new dpdwn_teamNames();
                    teamName.name = rdr["name"].ToString();
                    teamName.shortName = rdr["shortName"].ToString();
                    teamName.userName = rdr["userName"].ToString();
                    teamNames.Add(teamName);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(teamNames));
        }

        [WebMethod]
        public void GetAllTeamNamesByReportID(string userName, int dailyReportID)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<dpdwn_teamNames> teamNames = new List<dpdwn_teamNames>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetAllTeamNamesByreportID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlParameter param = new SqlParameter()
                //{
                //    ParameterName = "@userName",
                //    Value = userName
                //};
                //cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@dailyReportID", dailyReportID);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    dpdwn_teamNames teamName = new dpdwn_teamNames();
                    teamName.name = rdr["name"].ToString();
                    teamName.shortName = rdr["shortName"].ToString();
                    teamName.userName = rdr["userName"].ToString();
                    teamNames.Add(teamName);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(teamNames));
        }

        [WebMethod]
        public void GetSubJobTypesByJobID(string jobID)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<dpdwn_SubJobTypes> jobTypes = new List<dpdwn_SubJobTypes>();
            using (SqlConnection con = new SqlConnection(cs))
            {

                con.Open();

                SqlCommand cmd = new SqlCommand("spGetSubJobTypesByJobID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@jobID",
                    Value = jobID
                };
                cmd.Parameters.Add(param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    dpdwn_SubJobTypes jobType = new dpdwn_SubJobTypes();
                    jobType.subJobID = rdr["subJobID"].ToString();
                    jobType.description = rdr["description"].ToString();
                    jobTypes.Add(jobType);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(jobTypes));
        }

        [WebMethod]
        public void GetJobResourcesByJobID(string jobID)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<tbl_resourceTypes> resourceTypes = new List<tbl_resourceTypes>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetJobResourcesByJobID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@jobID", jobID);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    tbl_resourceTypes resourceType = new tbl_resourceTypes();
                    resourceType.resourceTypeID = (byte) rdr["resourceTypeID"];
                    resourceType.resourceType = rdr["resourceType"].ToString();
                    resourceType.description = rdr["description"].ToString();
                    resourceType.rate = (decimal)rdr["rate"];
                    resourceTypes.Add(resourceType);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(resourceTypes));
        }

        [WebMethod]
        public void GetJobInfoByJobID(string jobID)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<tbl_Jobs> jobs = new List<tbl_Jobs>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetJobInfoByJobID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@jobID", jobID);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    tbl_Jobs job = new tbl_Jobs();
                    job.jobID = rdr["jobID"].ToString();
                    job.description = rdr["description"].ToString();
                    job.customerCode = rdr["customerCode"].ToString();
                    job.customerContact = rdr["customerContact"].ToString();

                    job.active = (bool) rdr["active"];
                    job.location = rdr["location"].ToString();
                    job.nrmlHoursStart = rdr["nrmlHoursStart"].ToString();
                    job.nrmlHoursEnd = rdr["nrmlHoursEnd"].ToString();

                    job.dblTimeHours = (bool) rdr["dblTimeHours"];
                    job.nrmlHoursDaily = (int) rdr["nrmlHoursDaily"];
                    job.id = (int) rdr["id"];

                    jobs.Add(job);
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(jobs));
        }

        //[WebMethod]
        //public void SaveNewWorkDescEntry(int dailyReportID, string workDescription, string userName)
        //{
        //    string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
        //    List<dpdwn_teamNames> teamNames = new List<dpdwn_teamNames>();
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        SqlCommand cmd = new SqlCommand("spSaveWorkDescEntry", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@dailyReportID", dailyReportID);
        //        cmd.Parameters.AddWithValue("@workDescription", workDescription);
        //        cmd.Parameters.AddWithValue("@userName", userName);

        //        //con.Open();
        //        //SqlDataReader rdr = cmd.ExecuteReader();
        //        //while (rdr.Read())
        //        //{
        //        //    dpdwn_teamNames teamName = new dpdwn_teamNames();
        //        //    teamName.name = rdr["name"].ToString();
        //        //    teamName.shortName = rdr["shortName"].ToString();
        //        //    teamName.userName = rdr["userName"].ToString();
        //        //    teamNames.Add(teamName);
        //        //}
        //    }
        //    //JavaScriptSerializer js = new JavaScriptSerializer();
        //    //Context.Response.Write(js.Serialize(teamNames));
        //}

        [WebMethod]
        public void GetLastDlyRptCrtdByUserName(string dailyReportAuthor)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<string> dailyRptID = new List<string>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("spGetLastDlyRptCrtdByUserName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@dailyReportAuthor",
                    Value = dailyReportAuthor
                };
                cmd.Parameters.Add(param);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    dailyRptID.Add(rdr["dailyReportID"].ToString());
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(dailyRptID));
        }

        [WebMethod]
        public void GetLastTimeEntryByRptID_WkDesc_Cat (int dailyReportID, string workDescription, int workDescriptionCategory)
        {
            string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
            List<string> lastTimeEntry = new List<string>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetLastTimeEntryByRptID_WkDesc_Cat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlParameter param = new SqlParameter()
                //{
                //    ParameterName = "@userName",
                //    Value = userName
                //};
                //cmd.Parameters.Add(param);

                //cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@dailyReportID", dailyReportID);
                cmd.Parameters.AddWithValue("@workDescription", @workDescription);
                cmd.Parameters.AddWithValue("@workDescriptionCategory", workDescriptionCategory);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lastTimeEntry.Add(rdr["timeEntryID"].ToString());
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(lastTimeEntry));
        }

        //[WebMethod]
        //public void CopyDailyRpt(int copied_dailyReportID, int new_dailyReportID)
        //{
        //    string cs = ConfigurationManager.ConnectionStrings["allpaxServiceRecordEntities"].ConnectionString;
        //    //List<dpdwn_teamNames> teamNames = new List<dpdwn_teamNames>();
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        SqlCommand cmd = new SqlCommand("spCopyDailyRpt", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@copied_dailyReportID", copied_dailyReportID);
        //        cmd.Parameters.AddWithValue("@new_dailyReportID", new_dailyReportID);

        //        con.Open();
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        //while (rdr.Read())
        //        //{
        //        //    dpdwn_teamNames teamName = new dpdwn_teamNames();
        //        //    teamName.name = rdr["name"].ToString();
        //        //    teamName.shortName = rdr["shortName"].ToString();
        //        //    teamName.userName = rdr["userName"].ToString();
        //        //    teamNames.Add(teamName);
        //        //}
        //    }
        //    //JavaScriptSerializer js = new JavaScriptSerializer();
        //    //Context.Response.Write(js.Serialize(teamNames));
        //}
    }
}
