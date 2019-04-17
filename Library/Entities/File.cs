using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Entities
{
    public class File : IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }

        public int TorrentId { get; set; }
        ////public Torrent Torrent { get; set; }
    }
}
