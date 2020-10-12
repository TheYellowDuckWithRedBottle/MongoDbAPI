using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.JWT
{
    public class InToken
    {
        //token
        public string TokenStr { get; set; }
        //过期时间
        public DateTime Expires { get; set; }
    }
}
