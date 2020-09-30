using MongoDB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Extension
{
    public class DynamicUpdateAtt
    {
        public DynamicUpdateAtt()
        {

        }
        public DynamicUpdateAtt(params string[] MID)
        {
            List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
            foreach (var item in MID)
            {
                ClassHelper.CustPropertyInfo cpi = new ClassHelper.CustPropertyInfo("System.String.", item);
                lcpi.Add(cpi);
            }
              
            ClassHelper.AddProperty(typeof(DynamicUpdateAtt), lcpi);
        }
    }
}
