using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace allpax_sale_miner.ViewModels
{
    public class machinesW_kitsAvlbl_BnotInstalled
    {
        public string jobNo { get; set; }
        public string customerCode { get; set; }
        public string name { get; set; }
        public string eqpmtType { get; set; }
        public string model { get; set; }
        public string machineID { get; set; }
        public string kitsCurrent_machineID { get; set; }
        public string kitsCurrent_kitID { get; set; }
        public string kitsAvlbl_model { get; set; }
        public string kitsAvlbl_kitID { get; set; }
        public List<string> kitsAvlblbNotInstld { get; internal set; }
    }
}