using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DorfInfoBot.API.Models
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PreviewImage { get; set; }
    }
}