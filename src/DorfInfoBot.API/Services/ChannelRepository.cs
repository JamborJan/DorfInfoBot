using System;
using System.Collections.Generic;
using System.Linq;
using DorfInfoBot.API.Contexts;
using DorfInfoBot.API.Entities;

namespace DorfInfoBot.API.Services
{
    public class ChannelRepository : IChannelRepository
    {
         private readonly NewsContext _context;

         public ChannelRepository(NewsContext context)
         {
           _context = context ?? throw new ArgumentNullException(nameof(context));
         }

         public IEnumerable<Channel> GetChannel()
         {
            return _context.Channel.OrderBy(c => c.Name).ToList();
         }

         public Channel GetChannel(int channelId)
         {
            return _context.Channel.FirstOrDefault(c => c.Id == channelId);
         }
         
         public News GetOldestUnsentNewsByChannel(int channelId)
         {
            var broadcastsForChannel = _context.Broadcast.Where(c => c.ChannelId == channelId);
            var query = _context.News.GroupJoin(
                  broadcastsForChannel, 
                     news => news.Id,
                     broadcast => broadcast.NewsId,
                     (n,b) => new { News = n, Broadcast = b })
               .SelectMany(
                     b => b.Broadcast.DefaultIfEmpty(),
                     (n,b) => new { news = n.News, broadcast = b})
               .Where(b => b.broadcast == null)
               .OrderBy(n => n.news.DateOriginalPost)
               .Take(1)
               .ToList();

            if (query.Count < 1)
            {
               return null;
            }
            
            var result = query.ElementAt(0);
            var newsId = result.news.Id;
            return _context.News.SingleOrDefault(n => n.Id == newsId);
         }
         
         public void AddChannel(Channel channel)
         {
            _context.Channel.Add(channel);
         }

         public void UpdateChannel(Channel channel)
         {
            // when saving, entity framework core will update the DB, thus no udpate action is required
            // for better reading and in case in the future another iplementation is required, this method is added empty
         }

         public void DeleteChannel(Channel channel)
         {
            _context.Channel.Remove(channel);
         }

         public bool ChannelExists(int channelId)
         {
            return _context.Channel.Any(c => c.Id == channelId);
         }

         public IEnumerable<Broadcast> GetBroadcast()
         {
            return _context.Broadcast.OrderBy(b => b.DateOfBroadcast).ToList();
         }
         
         public Broadcast GetBroadcast(int broadcastId)
         {
            return _context.Broadcast
               .Where(b => b.Id == broadcastId).FirstOrDefault();
         }
         
         public Broadcast GetBroadcastForChannelAndNews(int channelId, int newsId)
         {
            return _context.Broadcast
               .Where(b => b.ChannelId == channelId && b.NewsId == newsId).FirstOrDefault();
         }
         
         public void AddBroadcast(Broadcast broadcast)
         {
            _context.Broadcast.Add(broadcast);
         }
         
         public void DeleteBroadcast(Broadcast broadcast)
         {
            _context.Broadcast.Remove(broadcast);
         }
         
         public bool BroadcastExists(int channelId, int newsId)
         {
            return _context.Broadcast.Any(b => b.ChannelId == channelId && b.NewsId == newsId);
         }

         public bool Save()
         {
            return (_context.SaveChanges() >= 0);
         }

    }
}