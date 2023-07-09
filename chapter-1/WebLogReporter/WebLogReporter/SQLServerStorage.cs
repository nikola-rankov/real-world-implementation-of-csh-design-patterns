using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace WebLogReporter;

public class SQLServerStorage
{
    public void StoreLogLine(NginxLogEntry entry)
    {
        using (SqlConnection con = new SqlConnection(@"Server=localhost,1433;Database=DesignPatterns;
                        User Id=sa;Password=Th3r31sN0Sp00n!;
                        Persist Security Info=False;Encrypt=False"))
        {
            var sql = new StringBuilder(@"INSERT INTO dbo.WebLogEntries (ServerIPAddress, RequestDateTime, 
                Verb, Route, ResponseCode, SizeInBytes, RequestingAgent) VALUES(");
            sql.Append("'" + entry.ServerIpAddress + "', ");
            sql.Append("'" + entry.RequestDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
            sql.Append("'" + entry.Verb + "', ");
            sql.Append("'" + entry.Route + "', ");
            sql.Append("'" + entry.ResponseCode + "', ");
            sql.Append("'" + entry.SizeInBytes + "', ");
            sql.Append("'" + entry.RequestingAgent + "')");
            
            con.Open();

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), con))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<NginxLogEntry> RetrieveLogLines()
    {
        var LogLines = new List<NginxLogEntry>();
        string sql = "SELECT * FROM [dbo].[WebLogEntries]";

        using (SqlConnection con = new SqlConnection(@"Server=localhost,1433;Database=DesignPatterns;
                        User Id=sa;Password=Th3r31sN0Sp00n!;
                        Persist Security Info=False;Encrypt=False"))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(sql.ToString(), con))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string serverIpAddress = reader.GetString(1);
                    DateTime requestDateTime = reader.GetDateTime(2);
                    string verb = reader.GetString(3);
                    string route = reader.GetString(4);
                    int responseCode = reader.GetInt32(5);
                    int sizeInBytes = reader.GetInt32(6);
                    string requestingAgent = reader.GetString(7);
                    var line = new NginxLogEntry(serverIpAddress, requestDateTime, verb,
                        route, responseCode, sizeInBytes, requestingAgent);
                    LogLines.Add(line);
                    
                }
            }
        }
        return LogLines;
    }
}