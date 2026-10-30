using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DorfInfoBot.API.Models
{
    public class NewsCreationDto
    {
        // validation help: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0
        [Required(ErrorMessage ="You must provide a title.")]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public string FullText { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOriginalPost { get; set; }
        [Required]
        [Url]
        public string LinkOriginalPost { get; set; }
        [Required]
        public string ExternalKey { get; set; }
        public ICollection<AttachmentDto> Attachment { get; set; }
            = new List<AttachmentDto>();
    }
}