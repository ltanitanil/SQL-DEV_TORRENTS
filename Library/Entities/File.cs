using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Entities
{
    public class File
    {
        [Column("FileId")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Size")]
        public string Size { get; set; }

        [Column("Torrent_Id")]
        public int TorrentId { get; set; }
        public Torrent Torrent { get; set; }
    }
}
