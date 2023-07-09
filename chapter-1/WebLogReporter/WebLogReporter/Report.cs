using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WebLogReporter;

public class Report
{
    //TODO: The rest of the code goes here

    public void GenerateReport(string OutputPath)
    {
        var database = new SQLServerStorage();
        var logLines = database.RetrieveLogLines();
        
        var output = new 
            StringBuilder(@"<html><head><title>Web Log Report</title></head><body>");
        output.Append(@"<table><tr><th>Request Date</th><th>Verb</th><th>Route</th>
            <th>Code</th><th>Size</th><th>Agent</th></tr>");
        foreach (var logLine in logLines)
        {
            output.Append("<tr>");
            output.Append("<td>" + 
                          logLine.RequestDateTime.ToString() + "</td>");
            output.Append("<td>" + logLine.Verb + "</td>");
            output.Append("<td>" + logLine.Route + "</td>");
            output.Append("<td>" + 
                          logLine.ResponseCode.ToString() + "</td>");
            output.Append("<td>" + 
                          logLine.SizeInBytes.ToString() + "</td>");
            output.Append("<td>" + 
                          logLine.RequestingAgent.ToString() + "</td>");
            output.Append("</tr>");
        }
        output.Append("</table></body></html>");
        File.WriteAllText(OutputPath, output.ToString());
    }
}