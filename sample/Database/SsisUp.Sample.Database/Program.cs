using System;
using System.Configuration;
using System.Reflection;
using DbUp;

namespace SsisUp.Sample.Database
{
    class Program
    {
        static int Main()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SampleDb"].ConnectionString;

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.WriteLine("Press any key to exit...");

                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            Console.WriteLine("Press any key to exit...");

            return 0;
        }
    }
}
