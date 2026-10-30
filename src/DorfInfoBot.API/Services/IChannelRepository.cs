using System.Collections.Generic;
using DorfInfoBot.API.Entities;

namespace DorfInfoBot.API.Services
{
    public interface IChannelRepository
    {
        IEnumerable<Channel> GetChannel();
        Channel GetChannel(int channelId);
        News GetOldestUnsentNewsByChannel(int chanelId);
        void AddChannel(Channel channel);
        void UpdateChannel(Channel channel);
        void DeleteChannel(Channel channel);
        bool ChannelExists(int channelId);
        IEnumerable<Broadcast> GetBroadcast();
        Broadcast GetBroadcast(int broadcastId);
        Broadcast GetBroadcastForChannelAndNews(int channelId, int newsId);
        void AddBroadcast(Broadcast broadcast);
        void DeleteBroadcast(Broadcast broadcast);
        bool BroadcastExists(int channelId, int newsId);
        bool Save();
    }
 }