using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SQL_DEV_TORRENTS
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            foreach (var a in GetTorrent())
            {
                Set(a);
                Console.WriteLine(i += 10000);
            }
            Console.WriteLine("The end");
            Console.ReadKey();
        }


        public static void Set(IEnumerable<Torrent> temp)
        {
            using (TorrentsEntities torrentsEntities = new TorrentsEntities())
            {
                torrentsEntities.Configuration.AutoDetectChangesEnabled = false;
                torrentsEntities.Configuration.ValidateOnSaveEnabled = false;

                torrentsEntities.BulkInsert(temp, options => { options.IncludeGraph = true; });
            }
        }

        public static List<int> list = new List<int>();

        static IEnumerable<IEnumerable<Torrent>> GetTorrent()
        {
            List<Torrent> torrents = new List<Torrent>();
            Torrent torrent = new Torrent();
            File file = new File();
            bool torrentswith = false;
            int counter = 1;
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
                                    torrent.Torrent_Hash = xml.GetAttribute("hash");
                                    torrent.Torrent_TrackerId = int.Parse(xml.GetAttribute("tracker_id"));
                                    torrentswith = false;
                                    break;
                                }
                                torrent.Torrent_Id = int.Parse(xml.GetAttribute("id"));
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
                                var a = int.Parse(xml.GetAttribute("id"));
                                if (!list.Contains(a))
                                {
                                    list.Add(a);
                                    torrent.Forum = new Forum()
                                    {
                                        ForumId = a,
                                        ForumValue=xml.ReadElementContentAsString()
                                    };
                                }
                                break;
                            }
                            if (xml.Name == "file")
                            {
                                torrent.Files.Add(new File { Name = xml.GetAttribute("name"), Size = xml.GetAttribute("size") });
                                break;
                            }
                            if (xml.Name == "dir")
                            {
                                torrent.Dir_Name = xml.GetAttribute("name");
                                break;
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if (xml.Name == "torrent")
                            {
                                torrents.Add(torrent);
                                torrent = new Torrent();
                                if (counter % 10000 == 0 && counter > 0)
                                {
                                    yield return torrents;
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
