using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Entities
{
    public class Forum : IAggregateRoot
    {
        public int Id { get; set; }
        public string Value { get; set; }

        //public ICollection<Torrent> Torrents { get; set; }

        //public Forum()
        //{
        //    Torrents = new HashSet<Torrent>();
        //}
    }
}
