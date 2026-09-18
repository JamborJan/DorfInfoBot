using System;

namespace DorfInfoBot.API.Models
{
    public class BroadcastDto
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public int ChannelId { get; set; }
        public DateTime DateOfBroadcast { get; set; }
    }
}