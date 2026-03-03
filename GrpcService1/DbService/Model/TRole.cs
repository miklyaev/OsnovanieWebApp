using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.DbService.Model
{
    [Table("t_role")]
    public class TRole
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("role_name")]
        public int RoleName { get; set; }

        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Column("off_time")]
        public DateTime? OffTime { get; set; }
    }
}
