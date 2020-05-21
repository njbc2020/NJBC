using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.DTO.Web
{
    public class DatailVM
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Reject { get; set; }
        public int Adv { get; set; }
        public int Good { get; set; }
        public int Bad { get; set; }
        public int Potential { get; set; }
        public int NullCount { get; set; }
    }
}
