using System.Globalization;
namespace WebLogReporter;

public class NginxLogEntry
{
    public enum HTTPVerbs { GET, POST, PUT, DELETE }
    public string ServerIpAddress { get; set; }
    public DateTime RequestDateTime { get; set; }
    public HTTPVerbs Verb { get; set; }
    public string Route {get; set;}
    public int ResponseCode { get; set; }
    public int SizeInBytes { get; set; }
    public string RequestingAgent { get; set; }

    public NginxLogEntry (string LogLine)
    {
        var parts = LogLine.Split();
        if (parts.Length < 12)
        {
            Console.WriteLine(LogLine);
        }

        ServerIpAddress = parts[0];
        var rawDateTime = parts[3].Split(" ")[0].Substring(1).Trim();
        RequestDateTime = DateTime.ParseExact(rawDateTime, "dd/MMM/yyyy:HH:mm:ss", CultureInfo.InvariantCulture);
        var rawHttpVerb = parts[5].Trim().Substring(1);
        Verb = (HTTPVerbs)Enum.Parse(typeof(HTTPVerbs), rawHttpVerb);
        Route = parts[6].Trim();
        ResponseCode = int.Parse(parts[8].Trim());
        SizeInBytes = int.Parse(parts[9].Trim());
        RequestingAgent = parts[11].Replace("\"", null);

    }

    public NginxLogEntry(string serverIpAddress, DateTime requestDateTime, string verb,
        string route, int responseCode, int sizeInBytes, string requestingAgent)
    {
        ServerIpAddress = serverIpAddress;
        RequestDateTime = requestDateTime;
        Verb = (HTTPVerbs)Enum.Parse(typeof(HTTPVerbs), verb);
        Route = route;
        ResponseCode = responseCode;
        SizeInBytes = sizeInBytes;
        RequestingAgent = requestingAgent;
    } 
}