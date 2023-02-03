using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcService1.DbService.Model
{
    [Table("t_region")]
    public class TRegion
    {
        [Key]
        [Column("region_id")]
        public int RegionId { get; set; }

        [Column("region_name")]
        public string? RegionName { get; set; }

        [Column("region_code")]
        public int RegionCode { get; set; }

        public List<TOffice>? Offices { get; set; }
    }
}
