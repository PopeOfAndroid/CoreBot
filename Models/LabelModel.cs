using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot.Models
{
    public class LabelModel
    {
        //labelname -> facebook -> labelid in DB
        public string name { get; set; }
        public string user { get; set; }
        public string psid { get; set; }
    }

    public class LabelData
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class RootObject2
    {
        public List<LabelData> data { get; set; }
    }
}
