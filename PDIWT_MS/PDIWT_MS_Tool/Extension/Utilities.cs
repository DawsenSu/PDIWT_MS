using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.DgnEC;
using Bentley.ECObjects.Schema;
using Bentley.MstnPlatformNET;

namespace PDIWT_MS_Tool.Extension
{
    public static class Utilities
    {
        public static bool TryParseSchemaString(string schemastring, out string schemaname, out int majornumber, out int minornumber)
        {
            schemaname = string.Empty;
            majornumber = minornumber = 0;
            string[] schemanamepiece = schemastring?.Split('.');
            if (schemanamepiece == null || schemanamepiece.Length < 3)
                return false;
            schemaname = schemanamepiece.First();
            majornumber = Convert.ToInt32(schemanamepiece.ElementAtOrDefault(1));
            minornumber = Convert.ToInt32(schemanamepiece.ElementAtOrDefault(2));
            return true;
        }
        public static IECClass[] GetActiveModelAllClasses()
        {
            List<IECClass> classes = new List<IECClass>();
            FindInstancesScope scope = FindInstancesScope.CreateScope(Session.Instance.GetActiveDgnFile(), new FindInstancesScopeOption());
            foreach (string schemastring in DgnECManager.Manager.DiscoverSchemasForModel(Session.Instance.GetActiveDgnModel(),ReferencedModelScopeOption.All,false))
            {
                string schemaName;
                int majornum, minornum;
                if (!TryParseSchemaString(schemastring, out schemaName, out majornum, out minornum))
                    throw new InvalidOperationException($"{schemastring}解析错误");
                IECSchema schema = DgnECManager.Manager.LocateSchemaInScope(scope, schemaName, majornum, minornum, SchemaMatchType.Exact);
                var ecClass = schema?.GetClasses();
                if (ecClass != null)
                {
                    classes.AddRange(ecClass);
                }
            }
            return classes.ToArray();
        }
    }
}
