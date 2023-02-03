using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.DbService.Model
{
    [Table("t_user_role")]
    public class TUserRole
    {
        [Key]
        [Column("user_role_id")]
        public int Id { get; set; }

        [ForeignKey("user_id")]
        public TUser User { get; set; }

        [ForeignKey("role_id")]
        public TRole Role { get; set; }
    }
}
