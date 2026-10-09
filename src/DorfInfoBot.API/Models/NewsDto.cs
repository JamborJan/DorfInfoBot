using System;
using System.Collections.Generic;

namespace DorfInfoBot.API.Models
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FullText { get; set; }
        public DateTime DateOriginalPost { get; set; }
        public string LinkOriginalPost { get; set; }
        public int NumberOfAttachment
        {
            get
            {
                return Attachment.Count;
            }
        }
        public ICollection<AttachmentDto> Attachment { get; set; }
            = new List<AttachmentDto>();
    }
}