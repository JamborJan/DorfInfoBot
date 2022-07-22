using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DorfInfoBot.API.Models
{
    public class BroadcastCreationDto
    {
        // validation help: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0
        [Required]
        public int NewsId { get; set; }
        [Required]
        public int ChannelId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBroadcast { get; set; }
    }
}