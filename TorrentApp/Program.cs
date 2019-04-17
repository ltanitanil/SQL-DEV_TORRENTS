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
    }
}
