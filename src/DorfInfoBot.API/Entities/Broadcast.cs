using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DorfInfoBot.API.Models;

namespace DorfInfoBot.API.Entities
{
    public class Broadcast
    {
        // validation help: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("NewsId")]
        public News News { get; set; }
        public int NewsId { get; set; }
        [Required]
        [ForeignKey("ChannelId")]
        public Channel Channel { get; set; }
        public int ChannelId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBroadcast { get; set; }
    }
}