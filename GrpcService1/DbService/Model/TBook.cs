using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrpcService1.DbService.Model
{
    [Table("t_book")]
    public class TBook
    {
        [Key]
        [Column("book_id")]
        public int BookId { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("pages")]
        public int? Pages { get; set; }

        [ForeignKey("author_id")] 
        public TAuthor? Author { get; set; }

        [Column("issue_date")]
        public DateTime? IssueDate { get; set; }
    }
}
