using Library.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using File = Library.Entities.File;

namespace Library
{
    public class Extension
    {
        Dictionary<int?, Forum> forumDictionary = new Dictionary<int?, Forum>();
        int batchSize;

        public Extension(int batchSize = 1000)
        {
            this.batchSize = batchSize;
        }

        public void ParseAndLoad(string path)
        {
            List<Torrent> torrents = new List<Torrent>();
            List<File> files = new List<File>();
            List<Forum> forums = new List<Forum>();
            int i = 0;
            Console.WriteLine("Start");

            foreach (var torrent in GetTorrents(path))
            {
                i++;
                torrents.Add(torrent);
                foreach (var file in torrent.Files)
                    files.Add(file);
                if (torrent.Forum != null)
                    forums.Add(torrent.Forum);

                if (i % batchSize == 0)
                {
                    UploadToDataBase(files, torrents, forums);
                    Console.WriteLine("Added" + i);
                    torrents = new List<Torrent>();
                    files = new List<File>();
                    forums = new List<Forum>();
                }
            }
            UploadToDataBase(files, torrents, forums);
        }

        void UploadToDataBase(IList<File> files, IList<Torrent> torrents, IList<Forum> forums)
        {
            using (Repository context = new Repository())
            {
                context.BulkInsert(forums);
                context.BulkInsert(torrents, batchSize);
                context.BulkInsert(files, batchSize * 5);
            }
        }

        public IEnumerable<Torrent> GetTorrents(string path)
        {
            Torrent torrent = new Torrent();
            bool torrentswith = false;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var gZipStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var xmlReader = XmlReader.Create(gZipStream))
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xmlReader.Name == "torrent")
                            {
                                if (torrentswith)
                                {
                                    torrent.Hash = xmlReader.GetAttribute("hash");
                                    torrent.TrackerId = int.Parse(xmlReader.GetAttribute("tracker_id"));
                                    torrentswith = false;
                                    break;
                                }
                                torrent.Id = int.Parse(xmlReader.GetAttribute("id"));
                                torrent.RegistredAt = DateTimeOffset.Parse(xmlReader.GetAttribute("registred_at"));
                                torrent.Size = xmlReader.GetAttribute("size");
                                torrentswith = true;
                                break;
                            }
                            if (xmlReader.Name == "title")
                            {
                                torrent.Title = xmlReader.ReadElementContentAsString();
                                break;
                            }
                            if (xmlReader.Name == "content")
                            {
                                torrent.Content = xmlReader.ReadElementContentAsString();
                                break;
                            }
                            if (xmlReader.Name == "forum")
                            {
                                torrent.ForumId = int.Parse(xmlReader.GetAttribute("id"));
                                if (forumDictionary.ContainsKey(torrent.ForumId))
                                    break;
                                forumDictionary.Add(torrent.ForumId, torrent.Forum);
                                torrent.Forum = new Forum()
                                {
                                    Id = torrent.ForumId.Value,
                                    Value = xmlReader.ReadElementContentAsString()
                                };
                                break;
                            }
                            if (xmlReader.Name == "file")
                            {
                                torrent.Files.Add(new File { Name = xmlReader.GetAttribute("name"), Size = xmlReader.GetAttribute("size"),  TorrentId = torrent.Id });
                                break;
                            }
                            if (xmlReader.Name == "dir")
                                torrent.Dir_Name = xmlReader.GetAttribute("name");
                            break;

                        case XmlNodeType.EndElement:
                            if (xmlReader.Name == "torrent")
                            {
                                yield return torrent;
                                torrent = new Torrent();
                            }
                            break;
                    }
                }
            }
        }
    }
}

