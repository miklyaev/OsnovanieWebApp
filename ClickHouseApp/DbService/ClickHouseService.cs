using ClickHouseApp.DbService.Model;
using ClickHouseApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ClickHouseService(IConfiguration config, ILogger<ClickHouseService> log)
        {
            _configuration = config;
            _logger = log;
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
