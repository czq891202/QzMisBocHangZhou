using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace QzMisBocHangZhou.DAL
{
    public class DBCache
    {
        public static DbContext<OracleConnection> DataBase = new DbContext<OracleConnection>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    }
}