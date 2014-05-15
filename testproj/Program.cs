using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System.IO;
using System.Diagnostics;
using Microsoft.Cis.Eventing;
using Microsoft.Cis.Monitoring.Events;
using System.Diagnostics.Tracing;
using Microsoft.Cis.Monitoring.DataAccess.AsyncClient.Proxy;
using Microsoft.Cis.Monitoring.DataAccess.Client;

namespace testproj
{
    public class SearchTimeEntity : BaseEntity
    {
        public SearchTimeEntity()
        {
        }
        /// <summary>
        /// Message
        /// </summary>
        public string IndexEntityRK { get; set; }
        public string CreateTime { get; set; }

    }

    public class BaseEntity : TableEntity
    {
        public BaseEntity() { }
        public BaseEntity(string entity, string filename, string line, string content)
        {
            Entity = entity;
            FileName = filename;
            Line = line;
            Content = content;
        }
        public string Entity { get; set; }
        public string FileName { get; set; }
        public string Line { get; set; }
        public string Content { get; set; }
    }

    [RDEvent(22222, TraceEventType.Information)]
    class TestEvent : RDEventBase
    {
        [RDEventProperty]
        public string Name { get; set; }
        [RDEventProperty]
        public int Result { get; set; }
        [RDEventProperty]
        public string Operation { get; set; }
    }

    class Program
    {
        public static void GetByTableQuery()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=sijhua;AccountKey=E9n4RL0JBZl4iyQF0SyTBgeEesKjP5+K1+5guirjIzuuVh65d9Crd0ksXXOCAFiya3djBQt72yo+Bv0CYy9HxQ==");
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            var tableContext = tableClient.GetTableServiceContext();


            CloudTable searchTimeTable = tableClient.GetTableReference("SearchTimeTable");

            TableQuery<DynamicTableEntity> query = null;


            query = new TableQuery<DynamicTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "PK"));
            // .Take(10);
            var result = searchTimeTable.ExecuteQuery(query);

            foreach (var r in result)
            {
                Console.WriteLine(r.PartitionKey);
            }
        }

        public static void GetByTableContext()
        {
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=sijhua;AccountKey=E9n4RL0JBZl4iyQF0SyTBgeEesKjP5+K1+5guirjIzuuVh65d9Crd0ksXXOCAFiya3djBQt72yo+Bv0CYy9HxQ==");
            var tableClient = account.CreateCloudTableClient();
            var context = tableClient.GetTableServiceContext();
            var table = tableClient.GetTableReference("User");


            var query = context.CreateQuery<DynamicTableEntity>("SearchTimeTable");
            var temp = (from item in query
                        where item.PartitionKey == "PK"
                        select item).First();
            Console.WriteLine(temp.ToString());
        }

        public static void ParseMappingConfig()
        {
            Dictionary<string, Dictionary<string, List<string>>> nameArg = new Dictionary<string, Dictionary<string, List<string>>>();
            Dictionary<string, Dictionary<string, int>> tableScenarios = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> blobScenarios = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> elseScenarios = new Dictionary<string, Dictionary<string, int>>();
            List<string> parameterList = new List<string>();
            string[] configs = Directory.GetFiles(@"F:\enlist\rd_store_n\services\xstore\storagetests\SCTE\configs", "*.xml", SearchOption.AllDirectories);
            foreach (string config in configs)
            {
                //Console.WriteLine(config);
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(ScenarioInstanceMap));
                System.IO.StreamReader file = new System.IO.StreamReader(config);
                try
                {
                    int tablecount = 0;
                    int blobcount = 0;
                    ScenarioInstanceMap scenarioMap = (ScenarioInstanceMap)reader.Deserialize(file);
                    foreach (var map in scenarioMap.Mapping)
                    {
                        foreach (var scenario in map.WorkloadScenarios)
                        {
                            if (string.IsNullOrEmpty(scenario.Name) || !scenario.Name.Contains("Table"))
                                continue;
                            /*
                            if (!nameArg.ContainsKey(scenario.Name))
                            {
                                nameArg.Add(scenario.Name, new Dictionary<string, List<string>>());
                            }*/

                            if (scenario.Arguments == null)
                                continue;

                            foreach (var arg in scenario.Arguments)
                            {
                                if (string.IsNullOrEmpty(arg.Name))
                                    continue;
                                if (!parameterList.Contains(arg.Name))
                                    parameterList.Add(arg.Name);
                                /*
                                if (nameArg[scenario.Name].ContainsKey(arg.Name))
                                {
                                    if (!nameArg[scenario.Name][arg.Name].Contains(arg.Value))
                                    {
                                        nameArg[scenario.Name][arg.Name].Add(arg.Value);
                                        //Console.WriteLine(scenario.Name + " " + arg.Name + " " + arg.Value);
                                    }
                                }
                                else
                                {
                                    nameArg[scenario.Name].Add(arg.Name, new List<string>());
                                    nameArg[scenario.Name][arg.Name].Add(arg.Value);
                                }*/
                            }
                            /*
                            if (scenario.Type.Contains("Table"))
                            {
                                if (!tableScenarios.ContainsKey(scenario.Type))
                                {
                                    tableScenarios.Add(scenario.Type, new Dictionary<string, int>());
                                }
                                if(tableScenarios[scenario.Type].ContainsKey(scenario.Name))
                                    tableScenarios[scenario.Type][scenario.Name]++;
                                else
                                    tableScenarios[scenario.Type].Add(scenario.Name,1);
                                tablecount++;
                            }
                            else if (scenario.Type.Contains("Blob"))
                            {
                                if (!blobScenarios.ContainsKey(scenario.Type))
                                {
                                    blobScenarios.Add(scenario.Type, new Dictionary<string, int>());
                                }
                                if(blobScenarios[scenario.Type].ContainsKey(scenario.Name))
                                    blobScenarios[scenario.Type][scenario.Name]++;
                                else
                                    blobScenarios[scenario.Type].Add(scenario.Name,1);
                                blobcount++;
                            }
                            else
                            {
                                if (!elseScenarios.ContainsKey(scenario.Type))
                                {
                                    elseScenarios.Add(scenario.Type, new Dictionary<string, int>());
                                }
                                if(elseScenarios[scenario.Type].ContainsKey(scenario.Name))
                                    elseScenarios[scenario.Type][scenario.Name]++;
                                else
                                    elseScenarios[scenario.Type].Add(scenario.Name,1);
                            }*/
                        }
                    }
                    //Console.WriteLine(string.Format("{0}\t{1}\t{2}", Path.GetFileName(config), tablecount, blobcount));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            Console.Write("Type,InstanceName,");
            foreach (var p in parameterList)
                Console.Write("{0},", p);
            Console.WriteLine();

            foreach (string config in configs)
            {
                //Console.WriteLine(config);
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(ScenarioInstanceMap));
                System.IO.StreamReader file = new System.IO.StreamReader(config);
                try
                {
                    ScenarioInstanceMap scenarioMap = (ScenarioInstanceMap)reader.Deserialize(file);
                    foreach (var map in scenarioMap.Mapping)
                    {
                        foreach (var scenario in map.WorkloadScenarios)
                        {
                            if (string.IsNullOrEmpty(scenario.Name) || !scenario.Name.Contains("Table"))
                                continue;
                            Console.Write("{0},{1},", scenario.Type, scenario.Name);

                            if (scenario.Arguments == null)
                            {
                                Console.WriteLine();
                                continue;
                            }

                            foreach (var name in parameterList)
                            {
                                foreach (var arg in scenario.Arguments)
                                {
                                    if (string.IsNullOrEmpty(arg.Name))
                                        continue;
                                    if(arg.Name.Equals(name))
                                        Console.Write("{0}", arg.Value);
                                }
                                Console.Write(",");
                            }
                            Console.WriteLine();
                        }
                    }
                    //Console.WriteLine(string.Format("{0}\t{1}\t{2}", Path.GetFileName(config), tablecount, blobcount));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            /*
            foreach (var na in nameArg)
            {
                //Console.Write(na.Key);

                foreach (var name in na.Value)
                {

                    Console.Write(na.Key + "," + name.Key + ",");
                    foreach (var v in name.Value)
                        Console.Write(v + ",");
                    Console.WriteLine("");
                }
            }*/


            Console.ReadLine();
            /*
            foreach(var tablescenario in tableScenarios)
            {
                Console.Write(tablescenario.Key);

                foreach (var name in tablescenario.Value)
                {

                    Console.Write(","+name.Key + "," + name.Value);
                    Console.WriteLine("");
                }
            }
            Console.WriteLine("");
            foreach (var tablescenario in blobScenarios)
            {
                Console.Write(tablescenario.Key);

                foreach (var name in tablescenario.Value)
                {

                    Console.Write("," + name.Key + "," + name.Value);
                    Console.WriteLine("");
                }
            }
            Console.WriteLine("");
            foreach (var tablescenario in elseScenarios)
            {
                Console.Write(tablescenario.Key);

                foreach (var name in tablescenario.Value)
                {

                    Console.Write("," + name.Key + "," + name.Value);
                    Console.WriteLine("");
                }
            }*/
        }

        private static void CleanUpTables(string connectStr)
        {
            var account = CloudStorageAccount.Parse(connectStr);
            var tableClient = account.CreateCloudTableClient();
            foreach(var table in tableClient.ListTables())
            {
                if(table.Name.Contains("Sijia"))
                    table.Delete();
            }
        }

        public class Class1 { }
        public class DerivedC1 : Class1 { }
        public class DerivedC2 : DerivedC1 { }
        static void Main(string[] args)
        {            DateTimeOffset start = DateTimeOffset.Now.AddHours(-4); //TimeHelper.GetDefaultTimeInterval().Start;//
            DateTimeOffset end = DateTimeOffset.Now;// TimeHelper.GetDefaultTimeInterval().End;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Microsoft.Cis.Monitoring.DataAccess.AsyncClient.IMonitoringDataAccess dataAccessSvc = new WebServerDataAccess(new Uri("https://stage.diagnostics.monitoring.core.windows.net/DataAccess"));
            TabularData data = dataAccessSvc.EndGetTabularData(dataAccessSvc.BeginGetTabularData("WOSSDevCounter10MEventVer1v0", start, end, "", false, null, null));
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
//            CleanUpTables("DefaultEndpointsProtocol=https;AccountName=sijiamds;AccountKey=A9F0ko4o92YucaM2H7+SqQEVaj7E+tJuiZMiBCuLQlV0FOOGfcyC0dQsgIxG+TSAwSeadHP5O+Z103La4T0zrQ==");
/*            ParseMappingConfig();

            GetByTableQuery();
            
            GetByTableContext();
            Console.ReadLine();
            GetByTableQuery();
            Console.ReadLine();
            */

        }
    }
}
