using System.IO;
using System;

using Microsoft.Extensions.Configuration;
using MyApp.Data;
using System.Text.Json.Nodes;
using MyApp.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            DataContextDapper dapper = new DataContextDapper(config);

            string computersJson = File.ReadAllText("Computers.json");

            // System Serializer Options
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Newtonsoft Serializer settings
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            IEnumerable<Computer>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);

            IEnumerable<Computer>? computersNewtonsoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);
        
            if (computersSystem != null)
            {
                foreach (Computer computer in computersSystem)
                {
                    string insertSql = @"INSERT INTO TutorialAppSchema.Computer 
                    (
                        Motherboard, 
                        CPUCores,
                        HasWifi,
                        HasLTE,
                        ReleaseDate,
                        Price,
                        VideoCard
                    )
                    VALUES
                    ('" +
                    EscapeSingleQuote(computer.Motherboard )+ "', '" +
                    computer.CPUCores + "', '" +
                    computer.HasWiFi + "', '" +
                    computer.HasLTE + "', '" +
                    computer.ReleaseDate?.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" +
                    computer.Price + "', '" +
                    EscapeSingleQuote(computer.VideoCard)
                    + "')";
                    dapper.ExecuteSql(insertSql);
                }
            }

            static string EscapeSingleQuote(string input)
            {
                string output = input.Replace("'", "''");
            
                return output;
            };

            // string computerscopySystem = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);
            // File.WriteAllText("computersCopySystem.txt", computerscopySystem);

            // string computerscopyNewtonsoft = JsonConvert.SerializeObject(computersNewtonsoft, settings);
            // File.WriteAllText("computersCopyNewtonsoft.txt", computerscopyNewtonsoft);
            
            
            // Console.WriteLine(computers);

            

            // File.WriteAllText("log.txt", insertSql);

            // using StreamWriter openFile = new("log.txt", append: true);

            // openFile.WriteLine(insertSql);
            // openFile.WriteLine("writing text to files is bananas");
            // openFile.Close();

            // Console.WriteLine(File.ReadAllText("log.txt"));
        
        }
    }
}