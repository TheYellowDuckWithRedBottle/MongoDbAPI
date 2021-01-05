﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Common
{
    public class MongoConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public List<string> PngCollectionName { get; set; }
    }
}
