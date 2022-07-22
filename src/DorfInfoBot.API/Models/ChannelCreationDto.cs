using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DorfInfoBot.API.Models
{
    public class ChannelCreationDto
    {
        // validation help: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}