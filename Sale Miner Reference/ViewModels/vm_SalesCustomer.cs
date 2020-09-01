using allpax_sale_miner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace allpax_sale_miner.ViewModels
{
    public class vm_SalesCustomer
    {
        public string jobNo { get; set; }
        public string customerCode { get; set; }
        public string eqpmtType { get; set; }
        public string model { get; set; }
        public string machineID { get; set; }
        public string name { get; set; }
        public List<string> jobNoList { get; internal set; }
        public List<string> kitsCurrent { get; internal set; }
        public List<string> kitsAvlblbNotInstld { get; internal set; }
    }
}