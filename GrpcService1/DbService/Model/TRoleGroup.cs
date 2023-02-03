using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.DbService.Model
{
    [Table("t_role_group")]
    public class TRoleGroup
    {
        [Key]
        [Column("role_group_id")]
        public int Id { get; set; }

        [ForeignKey("role_id")]
        public TRole Role { get; set; }

        [ForeignKey("group_id")]
        public TGroup Group { get; set; }


    }
}
