using Microsoft.EntityFrameworkCore;
using System;
using DorfInfoBot.API.Entities;

namespace DorfInfoBot.API.Contexts
{
    public class NewsContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<Attachment> Attachment { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Broadcast> Broadcast { get; set; }

        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>()
                 .HasData(
                new News()
                {
                    Id = 1,
                    Title = "Informationsveranstaltung Bahnanlage Dierikon",
                    Description = "Details zur Veranstaltung folgen im nächsten \"dierikon informiert.\" und sind auf der Homepage der Gemeinde bereits ab 20. August 2021 aufgeschaltet. Eine Anmeldung wird zwingend notwendig sein.",
                    FullText = "",
                    DateOriginalPost = new DateTime(2021, 7, 20),
                    LinkOriginalPost = "https://www.dierikon.ch/aktuellesinformationen/1308194",
                    ExternalKey = "InitKey1"
                },
                new News()
                {
                    Id = 2,
                    Title = "Mit der vbl die Stadt Luzern und die Agglomeration entdecken!",
                    Description = "Die virtuelle Entdeckungsreise in und um Luzern. Vom 1. bis 31. August 2021 täglich neue Ausflugstipps erhalten und an Gewinnspielen mitmachen.",
                    FullText = "",
                    DateOriginalPost = new DateTime(2021, 7, 29),
                    LinkOriginalPost = "https://www.dierikon.ch/aktuellesinformationen/1311797",
                    ExternalKey = "InitKey2"
                },
                new News()
                {
                    Id = 3,
                    Title = "Baugesuch Hans Mustermann",
                    Description = "Will einen Pool bauen, so wie alle.",
                    FullText = "",
                    DateOriginalPost = new DateTime(2021, 6, 29),
                    LinkOriginalPost = "https://www.dierikon.ch/publikationen/334790",
                    ExternalKey = "InitKey3"
                });


            modelBuilder.Entity<Attachment>()
              .HasData(
                new Attachment()
                {
                    Id = 1,
                    NewsId = 1,
                    Title = "Anhang-News-1.pdf",
                    PreviewImage = "The most visited urban park in the United States."

                },
                new Attachment()
                {
                    Id = 2,
                    NewsId = 1,
                    Title = "Anhang-News-1.pdf",
                    PreviewImage = "A 102-story skyscraper located in Midtown Manhattan."
                },
                  new Attachment()
                  {
                      Id = 3,
                      NewsId = 2,
                      Title = "Anhang-News-2.pdf",
                      PreviewImage = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                  },
                new Attachment()
                {
                    Id = 4,
                    NewsId = 2,
                    Title = "Anhang-News-2.pdf",
                    PreviewImage = "The the finest example of railway architecture in Belgium."
                },
                new Attachment()
                {
                    Id = 5,
                    NewsId = 3,
                    Title = "Anhang-News-3.pdf",
                    PreviewImage = "A wrought iron lattice tower on the Champ de Mars, Titled after engineer Gustave Eiffel."
                },
                new Attachment()
                {
                    Id = 6,
                    NewsId = 3,
                    Title = "Anhang-News-3.pdf",
                    PreviewImage = "The world's largest museum."
                });

            modelBuilder.Entity<Channel>()
                 .HasData(
                new Channel()
                {
                    Id = 1,
                    Name = "Telegram"
                });

            modelBuilder.Entity<Broadcast>()
              .HasData(
                new Broadcast()
                {
                    Id = 1,
                    NewsId = 1,
                    ChannelId = 1,
                    DateOfBroadcast = new DateTime(1981, 4, 3)

                },
                new Broadcast()
                {
                    Id = 2,
                    NewsId = 2,
                    ChannelId = 1,
                    DateOfBroadcast = new DateTime(1983, 8, 22)
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}