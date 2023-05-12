using ClickHouseApp.DbService.Model;
using ClickHouseApp.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octonica.ClickHouseClient;
using Polly;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ClickHouseApp.DbService
{
    public interface IClickHouseService
    {
        public int AddUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int id);

    }

    public class ClickHouseService : IClickHouseService
    {
        public readonly IConfiguration _configuration;
        private readonly ILogger<ClickHouseService> _logger;
        private readonly ClickHouseConnection _connection;

        public ClickHouseService(IConfiguration config, ILogger<ClickHouseService> log, IOptions<ClickhouseOptions> clickhouseOptions)
        {
            _configuration = config;
            _logger = log;
            var connectionString = clickhouseOptions.Value.ConnectionString;
            log.LogInformation("Using Clickhouse {ClickhouseConnectionString}", connectionString);

            //var sb = new ClickHouseConnectionStringBuilder(connectionString);
            //_connection = new ClickHouseConnection(sb);
            //_connection.Open();
        }

        public void UpdateUser(User user)
        {
            ApplicationContext db = new ApplicationContext();
            TUser tUser = new TUser
            {
                Password = user.Password,
                Age = user.Age,
                //CreationTime = DateTime.Now.ToUniversalTime()
            };

            db.Users.Update(tUser);
            db.SaveChanges();
        }
        public int AddUser(User user)
        {
            ApplicationContext db = new ApplicationContext();
            TUser tUser = new TUser
            {
                UserName = user.UserName,
                Password = user.Password,
                Age = user.Age,
                //CreationTime = DateTime.Now.ToUniversalTime()
            };

            var newUser = db.Users.Add(tUser);
            db.SaveChanges();
            return newUser.Entity.UserId;
        }

        public void DeleteUser(int id)
        {
            ApplicationContext db = new ApplicationContext();
            TUser? user = db.Users.Where(x => x.UserId == id).FirstOrDefault();

            if (user == null)
                return;

            db.Users.Remove(user);
            db.SaveChanges();
        }

    }
}
