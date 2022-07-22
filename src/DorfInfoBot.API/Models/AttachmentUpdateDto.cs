using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DorfInfoBot.API.Models
{
    public class AttachmentUpdateDto
    {
        [Required(ErrorMessage ="You must provide a title.")]
        [MaxLength(50)]
        public string Title { get; set; }
        public string PreviewImage { get; set; }
    }
}