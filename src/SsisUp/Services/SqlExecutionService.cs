using System;
using System.Collections.Generic;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;

namespace SsisUp.Services
{
    public interface ISqlExecutionService
    {
        int Execute(string connectionString, IEnumerable<SqlScript> sqlScripts, bool debug);
    }

    public class SqlExecutionService : ISqlExecutionService
    {
        public int Execute(string connectionString, IEnumerable<SqlScript> sqlScripts, bool debug)
        {
            UpgradeEngine upgradeEngine;
            
            if (debug)
                upgradeEngine =
                    DeployChanges.To
                        .SqlDatabase(connectionString)
                        .JournalTo(new NullJournal())
                        .WithScripts(sqlScripts)
                        .LogScriptOutput()
                        .LogToConsole()
                        .Build();
            else
                upgradeEngine =
                    DeployChanges.To
                        .SqlDatabase(connectionString)
                        .JournalTo(new NullJournal())
                        .WithScripts(sqlScripts)
                        .LogToConsole()
                        .Build();

            var result = upgradeEngine.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}