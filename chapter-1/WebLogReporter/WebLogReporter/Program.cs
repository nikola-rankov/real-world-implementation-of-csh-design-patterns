using System.Globalization;

namespace WebLogReporter
{
    //127.0.0.1 - - [16/Jan/2022:04:09:51 +0000] "GET /api/get_pricing_info/B641F364-DB29-4241-A45B-7AF6146BC HTTP/1.1" 200 5442 "-" "python-requests/2.25.0"
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("You must supply a path to the log file file you want to parse as well as a path for the output.");
                Console.WriteLine(@"For example:  WebLogReporter c:\temp\nginx-sample.log c:\temp\report.html");
                Environment.Exit(1);
            }
            
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("The path " + args[0] + " is not a valid log file.");
                Environment.Exit(1);
            }

            string inputFilePath = args[0];
            string outputFilePath = args[1];
            
            Console.WriteLine($"Processing File: {inputFilePath}");
            int lineCount = 0;

            var serverStorage = new SQLServerStorage();

            foreach (var line in System.IO.File.ReadLines(inputFilePath))
            {
                lineCount++;
                try
                {
                    var logEntry = new NginxLogEntry(line);
                    serverStorage.StoreLogLine(logEntry);
                }
                catch
                {
                    Console.WriteLine($"Problem on line {lineCount}");
                }
            }

            var reporter = new Report();
            reporter.GenerateReport(outputFilePath);
            
            Console.WriteLine($"Generated report for {lineCount} log lines");
            /*
            var newLogExEntry = new NginxLogEntry("127.0.0.1 - - [16/Jan/2022:04:09:51 +0000] \"GET /api/get_pricing_info/B641F364-DB29-4241-A45B-7AF6146BC HTTP/1.1\" 200 5442 \"-\" \"python-requests/2.25.0\"");

            var newSqlServerStorage = new SQLServerStorage();
            newSqlServerStorage.StoreLogLine(newLogExEntry);
            */
        }
    }
    
    
}