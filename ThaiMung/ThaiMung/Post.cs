using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiMung
{
    class Post
    {
        public int p_id { get;  set; }
        public double latitude { get; set; }
        public double longtitude { get; set; }
        public string dateTime { get;set; }
        public int id { get; set; }
        public string description { get;  set; }
        public int status { get;  set; }
        public int countSolve { get;  set; }
        public int countSeen { get;   set; }
        public int countSpam { get; set; }
        public List<string> nameTag { get; set; }


    }
}
