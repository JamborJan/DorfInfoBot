using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DorfInfoBot.API.Contexts;
using DorfInfoBot.API.Entities;

namespace DorfInfoBot.API.Services
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsContext _context;
        
        public NewsRepository(NewsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<News> GetNews()
        {
            return _context.News.OrderByDescending(n => n.DateOriginalPost).ToList();
        }

        public News GetNews(int newsId, bool includeAttachments)
        {
            if (includeAttachments)
            {
                return _context.News
                    .Include(n => n.Attachment).FirstOrDefault(n => n.Id == newsId);
            }

            return _context.News.FirstOrDefault(n => n.Id == newsId);
        }

        public IEnumerable<Broadcast> GetBroadcastsForNews(int newsId)
        {
            var query = _context.News.GroupJoin(
                     _context.Broadcast, 
                     news => news.Id,
                     broadcast => broadcast.NewsId,
                     (n,b) => new { News = n, Broadcast = b })
               .SelectMany(
                     b => b.Broadcast.DefaultIfEmpty(),
                     (n,b) => new { news = n.News, broadcast = b})
               .Where(b => b.broadcast != null && b.broadcast.NewsId == newsId)
               .OrderBy(n => n.news.DateOriginalPost)
               .Select(b => b.broadcast)
               .ToList();

            return query;
        }

        public void AddNews(News news)
        {
            _context.News.Add(news);
        }

        public void UpdateNews(News news)
        {
            // when saving, entity framework core will update the DB, thus no udpate action is required
            // for better reading and in case in the future another iplementation is required, this method is added empty
        }

        public void DeleteNews(News news)
        {
            _context.News.Remove(news);
        }

        public Attachment GetAttachmentForNews(int newsId, int attachmentId)
        {
            return _context.Attachment
                .Where(a => a.NewsId == newsId && a.Id == attachmentId).FirstOrDefault();
        }

        public IEnumerable<Attachment> GetAttachmentForNews(int newsId)
        {
            return _context.Attachment
                .Where(a => a.NewsId == newsId).ToList();
        }

        public bool NewsExists(int newsId)
        {
            return _context.News.Any(n => n.Id == newsId);
        }

        public News NewsExistsByExternalKey(string externalKey)
        {
            return _context.News.Where(n => n.ExternalKey == externalKey).FirstOrDefault();
        }

        public void AddAttachmentForNews(int newsId, Attachment attachment)
        {
            var news = GetNews(newsId, false);
            news.Attachment.Add(attachment);
        }

        public void UpdateAttachmentForNews(int newsId, Attachment attachment)
        {
            // when saving, entity framework core will update the DB, thus no udpate action is required
            // for better reading and in case in the future another iplementation is required, this method is added empty
        }

        public void DeleteAttachment(Attachment attachment)
        {
            _context.Attachment.Remove(attachment);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
        
    }
 }