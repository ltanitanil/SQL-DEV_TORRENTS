using Library.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using File = Library.Entities.File;

namespace Library.Concrete
{
    class EFDbContext : DbContext
    {
        public DbSet<Torrent> Torrents { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Forum> Forums { get; set; }

        public EFDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=EPBYBREW5103\\TATYANASQL;Database=XML;Trusted_Connection=True;");//SQL-DEV_TORRENTS orig
        }
    }

    public class TorrentXML
    {
        public static void Set()
        {
            foreach (var a in GetTorrents())
            {
                using (EFDbContext torrentsEntities = new EFDbContext())
                {
                    //torrentsEntities.ChangeTracker.AutoDetectChangesEnabled = false;
                    torrentsEntities.BulkInsert(a, options => { options.IncludeGraph = true; });
                }
            }
        }

        private static List<int> list = new List<int>();

        private static IEnumerable<IEnumerable<Torrent>> GetTorrents()
        {
            List<Torrent> torrents = new List<Torrent>();
            Torrent torrent = new Torrent();
            bool torrentswith = false;
            int counter = 1;
            using (var fileStream = new FileStream(@"C:\Users\Tatsiana_Panasiuk\Downloads\rutracker-20190323.xml.gz", FileMode.Open, FileAccess.Read))
            using (var gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var xml = XmlReader.Create(@"D:\qwerty\rutracker-20190323.xml"))
            {
                while (xml.Read())
                {
                    switch (xml.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xml.Name == "torrent")
                            {
                                if (torrentswith)
                                {
                                    torrent.Hash = xml.GetAttribute("hash");
                                    torrent.TrackerId = int.Parse(xml.GetAttribute("tracker_id"));
                                    torrentswith = false;
                                    break;
                                }
                                torrent.Id = int.Parse(xml.GetAttribute("id"));
                                torrent.RegistredAt = DateTimeOffset.Parse(xml.GetAttribute("registred_at"));
                                torrent.Size = xml.GetAttribute("size");
                                torrentswith = true;
                                break;
                            }
                            if (xml.Name == "title")
                            {
                                torrent.Title = xml.ReadElementContentAsString();
                                break;
                            }
                            if (xml.Name == "content")
                            {
                                torrent.Content = xml.ReadElementContentAsString();
                                break;
                            }
                            if (xml.Name == "forum")
                            {
                                var id = int.Parse(xml.GetAttribute("id"));
                                if (list.Contains(id))
                                    break;
                                list.Add(id);
                                torrent.Forum = new Forum()
                                {
                                    Id = id,
                                    Value = xml.ReadElementContentAsString()
                                };
                                break;
                            }
                            if (xml.Name == "file")
                            {
                                torrent.Files.Add(new File { Name = xml.GetAttribute("name"), Size = xml.GetAttribute("size") });
                                break;
                            }
                            if (xml.Name == "dir")
                                torrent.Dir_Name = xml.GetAttribute("name");
                            break;

                        case XmlNodeType.EndElement:
                            if (xml.Name == "torrent")
                            {
                                torrents.Add(torrent);
                                torrent = new Torrent();
                                if (counter % 10000 == 0 && counter > 0)
                                {
                                    yield return (torrents);
                                    torrents = new List<Torrent>();
                                }
                                counter++;
                            }
                            break;
                    }
                }
                yield return torrents;
            }
        }

    }
}
