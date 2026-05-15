using System;

namespace DorfInfoBot.API.Models
{
    public class BroadcastUpdateDto
    {
        public int NewsId { get; set; }
        public int ChannelId { get; set; }
        public DateTime DateOfBroadcast { get; set; }
    }
}