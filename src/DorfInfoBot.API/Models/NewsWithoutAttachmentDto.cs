using System;

namespace DorfInfoBot.API.Models
{
    public class NewsWithoutAttachmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullText { get; set; }
        public DateTime DateOriginalPost { get; set; }
        public string LinkOriginalPost { get; set; }
    }
}