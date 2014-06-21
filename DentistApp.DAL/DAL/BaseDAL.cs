using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace DentistApp.DAL.DAL
{
    public static class BaseDAL
    {
        public static DentistDbContext DbContext {get;set;}
        public static DentistDbContext GetContext()
        {
            DbContext = new DentistDbContext();
            return DbContext;
            if (DbContext.Database.Connection.State != ConnectionState.Open)
                DbContext.Database.Connection.Open();
            return DbContext;

        }
    }
}
