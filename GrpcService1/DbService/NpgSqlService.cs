using GrpcService1.DbService.Model;
namespace GrpcService1.DbService
{
    public interface INpgSqlService
    {
        public List<User> GetAll();
        public User GetById(int id);
        public int AddUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int id);

    }
    public class NpgSqlService : INpgSqlService
    {
        public readonly IConfiguration _configuration;
        private readonly ILogger<NpgSqlService> _logger;
        public NpgSqlService(IConfiguration config, ILogger<NpgSqlService> log)
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
                CreationTime = DateTime.Now.ToUniversalTime()
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
                CreationTime = DateTime.Now.ToUniversalTime()
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

        public List<User> GetAll()
        {
            // получение данных           
            ApplicationContext db = new ApplicationContext();
            return (from x in db.Users
                    select new User
                    {
                        UserId = x.UserId,
                        UserName = x.UserName,
                        Password = x.Password,
                        Age = x.Age ?? 0
                    }).ToList();
        }

        public User GetById(int id)
        {
            ApplicationContext db = new ApplicationContext();
            return (from x in db.Users
                    where x.UserId == id
                    select new User
                    {
                        UserId = x.UserId,
                        UserName = x.UserName,
                        Password = x.Password,
                        Age = x.Age ?? 0
                    }).FirstOrDefault();
        }

    }
}
