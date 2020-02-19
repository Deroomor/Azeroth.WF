using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azeroth.STSM
{
    public class DbContext:System.Data.Entity.DbContext
    {
        public DbContext() : base("name=master")
        {

        }

        public System.Data.Entity.DbSet<FlowInfo> FlowInfo { get; set; }
    }
}
