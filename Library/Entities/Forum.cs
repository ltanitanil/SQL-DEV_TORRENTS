using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Entities
{
    public class Forum : IAggregateRoot
    {
        [Column("ForumId")]
        public int Id { get; set; }
        [Column("ForumValue")]
        public string Value { get; set; }

        //public ICollection<Torrent> Torrents { get; set; }

        //public Forum()
        //{
        //    Torrents = new HashSet<Torrent>();
        //}
    }
}
