using System;
using System.ComponentModel.DataAnnotations;

namespace NScraper.Data
{
    public class Item
    {
        public long Id { get; set; }
        [MaxLength(500)]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
