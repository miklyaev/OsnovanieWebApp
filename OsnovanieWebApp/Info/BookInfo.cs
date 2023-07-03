using System;

namespace OsnovanieWebApp.Info
{
    public class BookInfo
    {
        public string? Title { get; set; }
        public int Pages { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? IssueDate { get; set; }
    }
}
