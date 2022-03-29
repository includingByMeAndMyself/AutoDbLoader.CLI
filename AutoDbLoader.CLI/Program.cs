using AutoDbLoader.DAL.MSSQL;
using AutoDbLoader.DAL.MSSQL.Context;
using AutoDbLoader.DAL.MSSQL.Interface;
using AutoDbLoader.DAL.MSSQL.Repository;
using AutoDbLoader.DAL.txt.Infrastructure;
using AutoDbLoader.DAL.txt.Interface;
using AutoDbLoader.DAL.txt.Repository;
using AutoDbLoader.CLI.Interface;
using AutoDbLoader.CLI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using AutoDbLoader.CLI.Infrastructure;

namespace AutoDbLoader.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var date = DateTime.Now.ToShortDateString();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
#if DEBUG
            var connectionString = config.GetConnectionString("TestConnection");
#else
            var connectionString = config.GetConnectionString("DefaultConnection");
#endif

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File($"..\\Logs\\{date} log.txt")
                .CreateLogger();



            using IHost host = Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((_, services) => services
                .AddAutoMapper(typeof(MappingProfile), typeof(DataAccessMappingProfile))
                .AddSingleton(x => new TxtSettings("..\\Data\\", "..\\Alias\\"))
                .AddSingleton(x => new JsonSettings("..\\!warning\\"))
                .AddTransient<IPaymentRepository, PaymentRepository>()
                .AddTransient<ITerritoryPaymentsRepository, TerritoryPaymentsRepository>()
                .AddTransient<IPaymentService, PaymentService>()
                .AddDbContext<TerritoryPaymentContext>(options =>
                            options.UseSqlServer(connectionString))
                .AddLogging(builder =>
                {
                    builder.AddSerilog(logger: Log.Logger, dispose: true);
                }))
                .Build();

            ExemplifyScoping(host.Services);
        }


        static void ExemplifyScoping(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var op = provider.GetRequiredService<IPaymentService>();
            op.LoadDataToDb();
        }
    }
}