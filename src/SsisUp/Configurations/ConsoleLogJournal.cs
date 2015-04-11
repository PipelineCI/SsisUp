using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;

namespace SsisUp.Configurations
{
    public class ConsoleLogJournal : IJournal
    {
        public static List<SqlScript> scriptsExecuted = new List<SqlScript>();

        public string[] GetExecutedScripts()
        {
            return scriptsExecuted.Select(x => x.Name).ToArray();
        }

        public void StoreExecutedScript(SqlScript script)
        {
            scriptsExecuted.Add(script);
        }
    }
}