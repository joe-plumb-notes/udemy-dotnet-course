using System.IO;
using System;

using Microsoft.Extensions.Configuration;
using MyApp.Data;
using System.Text.Json.Nodes;
using MyApp.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;

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

            string computersJson = File.ReadAllText("ComputersSnake.json");

            // Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
            //     cfg.CreateMap<ComputerSnake, Computer>()
            //         .ForMember(destination => destination.ComputerId, options => 
            //             options.MapFrom(source => source.computer_id))
            //         .ForMember(destination => destination.Motherboard, options => 
            //             options.MapFrom(source => source.motherboard))
            //         .ForMember(destination => destination.CPUCores, options => 
            //             options.MapFrom(source => source.cpu_cores))
            //         .ForMember(destination => destination.HasWiFi, options => 
            //             options.MapFrom(source => source.has_wifi))
            //         .ForMember(destination => destination.HasLTE, options => 
            //             options.MapFrom(source => source.has_lte))
            //         .ForMember(destination => destination.ReleaseDate, options => 
            //             options.MapFrom(source => source.release_date))
            //         .ForMember(destination => destination.Price, options => 
            //             options.MapFrom(source => source.price))
            //         .ForMember(destination => destination.VideoCard, options => 
            //             options.MapFrom(source => source.video_card));      
            // }));

            IEnumerable<Computer>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson);

            // IEnumerable<Computer> computersResult = mapper.Map<IEnumerable<Computer>>(computersSystem);
        
            if (computersSystem != null)
            {
                
                foreach (Computer computer in computersSystem)
                {
                    // Convert values for input to SQL table    
                    string motherboard = computer.Motherboard != null ? $"'{EscapeSingleQuote(computer.Motherboard)}'" : "NULL";
                    string videoCard = computer.VideoCard != null ? $"'{EscapeSingleQuote(computer.VideoCard)}'" : "NULL";
                    string releaseDate = computer.ReleaseDate != null ? $"'{computer.ReleaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")}'" : "NULL";
                    int hasWiFi = computer.HasWiFi ? 1 : 0;
                    int hasLTE = computer.HasLTE ? 1 : 0;

                    string insertSql = $@"INSERT INTO TutorialAppSchema.Computer 
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
                    (
                        {motherboard}, 
                        {computer.CPUCores}, 
                        {hasWiFi}, 
                        {hasLTE}, 
                        {releaseDate}, 
                        {computer.Price}, 
                        {videoCard}
                    )";

                     dapper.ExecuteSql(insertSql);
                }
            }

            static string EscapeSingleQuote(string input)
            {
                string output = input.Replace("'", "''");
            
                return output;
            };        
        }
    }
}