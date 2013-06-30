using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiMung
{
    class User
    {
        public string email { get; set; }
        public int id { get; set; }
        public int countban { get; set; }
        public int countspam { get; set; }
        public int status { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string password { get; set; }
    }
}
