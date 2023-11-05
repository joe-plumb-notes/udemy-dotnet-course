using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyApp.Data;
using MyApp.Models;

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
            DataContextEntityFramework entityFramework = new DataContextEntityFramework(config);

            string sqlCommand = "SELECT GETDATE();";

            DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);

            Console.WriteLine("Current Date: " + rightNow);                       

            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                CPUCores = 4,
                HasWiFi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 1954.87m,
                VideoCard = "RTX3060"
            };

            entityFramework.Add(myComputer);
            entityFramework.SaveChanges();
            

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
            myComputer.Motherboard + "', '" +
            myComputer.CPUCores + "', '" +
            myComputer.HasWiFi + "', '" +
            myComputer.HasLTE + "', '" +
            myComputer.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" +
            myComputer.Price + "', '" +
            myComputer.VideoCard 
            + "')";

            // Console.WriteLine(insertSql);

            // int result = dapper.ExecuteSqlWithRowCount(insertSql);

            //Console.WriteLine("Rows affected from SQL : " + result);


            string sqlSelect = @"SELECT 
                Computer.ComputerId,
                Computer.Motherboard, 
                Computer.CPUCores,
                Computer.HasWifi,
                Computer.HasLTE,
                Computer.ReleaseDate,
                Computer.Price,
                Computer.VideoCard
            FROM TutorialAppSchema.Computer
            ";

            

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

            foreach(Computer singlecomputer in computers)
            {
                Console.WriteLine(singlecomputer.ComputerId+ "', '" +
                    singlecomputer.Motherboard + "', '" +
                    singlecomputer.CPUCores + "', '" +
                    singlecomputer.HasWiFi + "', '" +
                    singlecomputer.HasLTE + "', '" +
                    singlecomputer.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" +
                    singlecomputer.Price + "', '" +
                    singlecomputer.VideoCard);
            }

            IEnumerable<Computer>? computersEf = entityFramework.Computer?.ToList<Computer>();

            if ( computersEf != null)
            {
                foreach(Computer singlecomputer in computersEf)
                {
                    Console.WriteLine(singlecomputer.ComputerId+ "', '" +
                        singlecomputer.Motherboard + "', '" +
                        singlecomputer.CPUCores + "', '" +
                        singlecomputer.HasWiFi + "', '" +
                        singlecomputer.HasLTE + "', '" +
                        singlecomputer.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', '" +
                        singlecomputer.Price + "', '" +
                        singlecomputer.VideoCard);
                }
            } 

            File.WriteAllLines()
            
            // Console.WriteLine(myComputer.Motherboard);
            // Console.WriteLine(myComputer.HasWiFi);
            // Console.WriteLine(myComputer.VideoCard);
            // Console.WriteLine(myComputer.ReleaseDate);

        }
    }
}