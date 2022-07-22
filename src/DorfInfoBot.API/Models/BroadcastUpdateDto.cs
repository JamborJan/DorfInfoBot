using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DorfInfoBot.API.Models
{
    public class BroadcastUpdateDto
    {
        public int NewsId { get; set; }
        public int ChannelId { get; set; }
        public DateTime DateOfBroadcast { get; set; }
    }
}