using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseApp.DbService.Model
{
    //[Table("t_user")]
    public class TUser
    {
        //[Key]
        //[Column("user_id")]
        public int UserId { get; set; }

        //[Column("user_name")]
        public string? UserName { get; set; }

        //[Column("password")]
        public string? Password { get; set; }

        //[Column("age")]
        public int? Age { get; set; }

       // [Column("first_name")]
        public string? FirstName { get; set; }

       //[Column("last_name")]
        public string? LastName { get; set; }

        //[Column("patronymic")]
        public string? Patronymic { get; set; }

        //[Column("creation_time")]
        //public DateTime CreationTime { get; set; }

        //[Column("off_time")]
        //public DateTime? OffTime { get; set; }

    }
}
