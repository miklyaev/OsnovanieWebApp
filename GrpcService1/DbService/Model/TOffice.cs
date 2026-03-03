using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.DbService.Model
{
    [Table("t_office")]
    public class TOffice
    {
        [Key]
        [Column("office_id")]
        public int OfficeId { get; set; }

        [Column("office_name")]
        public string? OfficeName { get; set; }

        [Column("creation_time")]
        public DateTime CreationTime { get; set; }

        [Column("off_time")]
        public DateTime? OffTime { get; set; }

        public List<TUser>? Users { get; set; }

        [ForeignKey("region_id")]
        public TRegion? Region { get; set; }
    }
}
