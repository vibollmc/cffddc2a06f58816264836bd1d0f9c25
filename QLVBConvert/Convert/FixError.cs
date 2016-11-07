using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convert
{
    public class FixError
    {
        Logging _logger = new Logging();

        public void BuildIndex(Stringconnect strconnectdich)
        {

            var sqlIndex = "DECLARE @Database VARCHAR(255) "
            + "DECLARE @Table VARCHAR(255) "
            + "DECLARE @cmd NVARCHAR(500) "
            + "DECLARE @fillfactor INT "
            + "set @Database='" + strconnectdich.Database + "' "
            + "SET @fillfactor = 90 "
            + "SET @cmd = 'DECLARE TableCursor CURSOR FOR SELECT table_catalog + ''.'' + table_schema + ''.'' + table_name as tableName  "
            + "FROM ' + @Database + '.INFORMATION_SCHEMA.TABLES WHERE table_type = ''BASE TABLE'''  "
            + "EXEC (@cmd) "
            + " OPEN TableCursor  "
            + " FETCH NEXT FROM TableCursor INTO @Table   "
            + " WHILE @@FETCH_STATUS = 0   "
            + " BEGIN  "
            + " SET @cmd = 'ALTER INDEX ALL ON ' + @Table + ' REBUILD WITH (FILLFACTOR = ' + CONVERT(VARCHAR(3),@fillfactor) + ')'  "
            + " EXEC (@cmd)  "
            + " FETCH NEXT FROM TableCursor INTO @Table   "
            + " END "
            + " CLOSE TableCursor  "
            + " DEALLOCATE TableCursor "
            ;
            Utils.RunQuery(sqlIndex, strconnectdich);

        }


    }
}
