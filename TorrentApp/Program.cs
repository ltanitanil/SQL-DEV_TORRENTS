using Library;
using Library.Concrete;
using Library.Entities;
using System;
using System.Collections.Generic;

namespace TorrentApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Extension(10000).ParseAndLoad(@"C:\Users\Tatsiana_Panasiuk\Downloads\rutracker-20190323.xml.gz");
        }


        #region

        //static void Main(string[] args)
        //{
        //    List<Torrent> torrents = new List<Torrent>();
        //    List<File> files = new List<File>();
        //    List<Forum> forums = new List<Forum>();
        //    int i = 0;
        //    Console.WriteLine("Start " + DateTime.Now);

        //    foreach (var torrent in Extension.GetTorrents(@"C:\Users\Tatsiana_Panasiuk\Downloads\rutracker-20190323.xml.gz"))
        //    {
        //        i++;
        //        torrents.Add(torrent);
        //        foreach (var file in torrent.Files)
        //            files.Add(file);
        //        if (torrent.Forum != null)
        //            forums.Add(torrent.Forum);

        //        if (i % 10000 == 0)
        //        {
        //            Console.WriteLine("Popadanie " + DateTime.Now);
        //            UploadToDataBase(files, torrents, forums);
        //            Console.WriteLine("dobavleno" + i + "     " + DateTime.Now);
        //            torrents = new List<Torrent>();
        //            files = new List<File>();
        //            forums = new List<Forum>();
        //        }
        //    }
        //    UploadToDataBase(files, torrents, forums);
        //}

        //public static void UploadToDataBase(IList<File> files, IList<Torrent> torrents, IList<Forum> forums)
        //{
        //    using (Repository context = new Repository())
        //    {
        //        context.BulkInsert(forums);
        //        context.BulkInsert(torrents, 10100);
        //        context.BulkInsert(files, 500000);
        //    }
        //}
        #endregion
    }
}
