using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrpcService1.DbService.Model
{
    [Table("t_author")]
    public class TAuthor
    {
        [Key]
        [Column("author_id")]
        public int AuthorId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("age")]
        public int? Age { get; set; }

        public List<TBook>? Books { get; set; }


    }
}
