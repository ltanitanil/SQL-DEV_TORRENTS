using Library.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;
using System.Xml;
using EFCore.BulkExtensions;
using File = Library.Entities.File;
using System.Collections;

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
            optionsBuilder.UseSqlServer("Server=EPBYBREW5103\\TATYANASQL;Database=XML1;Trusted_Connection=True;");//SQL-DEV_TORRENTS orig
        }
    }
}
