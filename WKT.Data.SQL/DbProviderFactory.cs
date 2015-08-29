using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Data.SQL
{
    public class DbProviderFactory
    {
        private DbProviderFactory()
        { }

        public static IDbProvider CreateInstance(string name)
        {
            IDbProvider _instance = new SQLDbProvider(name);
            return _instance;
        }

        public static void ResetDBProvider()
        {
            
        }
    }
}
