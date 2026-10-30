using System.Collections.Generic;
using DorfInfoBot.API.Entities;

namespace DorfInfoBot.API.Services
{
    public interface INewsRepository
    {
        IEnumerable<News> GetNews();
        News GetNews(int newsId, bool includeAttachments);
        IEnumerable<Broadcast> GetBroadcastsForNews(int newsId);
        void AddNews(News news);
        void UpdateNews(News news);
        void DeleteNews(News news);
        Attachment GetAttachmentForNews(int newsId, int attachmentId);
        IEnumerable<Attachment> GetAttachmentForNews(int newsId);
        bool NewsExists(int newsId);
        News NewsExistsByExternalKey(string externalKey);
        void AddAttachmentForNews(int newsId, Attachment attachment);
        void UpdateAttachmentForNews(int newsId, Attachment attachment);
        void DeleteAttachment(Attachment attachment);
        bool Save();
    }
 }