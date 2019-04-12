using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Entities
{
    public class Torrent
    {
        [Key]
        [Column("Torrent_Id")]
        public int Id { get; set; }
        [Column("RegistredAt")]
        public DateTimeOffset RegistredAt { get; set; }
        [Column("Size")]
        public string Size { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Torrent_Hash")]
        public string Hash { get; set; }
        [Column("Torrent_TrackerId")]
        public int? TrackerId { get; set; }
        [Column("Content")]
        public string Content { get; set; }
        [Column("Dir_Name")]
        public string Dir_Name { get; set; }

        [Column("Forum_Id")]
        public int? ForumId { get; set; }
        public Forum Forum { get; set; }

        public ICollection<File> Files { get; set; }

        public Torrent()
        {
            Files = new HashSet<File>();
        }

    }
}
