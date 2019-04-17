using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Entities
{
    public class Torrent : IAggregateRoot
    {
        public int Id { get; set; }
        public DateTimeOffset RegistredAt { get; set; }
        public string Size { get; set; }
        public string Title { get; set; }
        public string Hash { get; set; }
        public int? TrackerId { get; set; }
        public string Content { get; set; }
        public string Dir_Name { get; set; }

        public int? ForumId { get; set; }
        public Forum Forum { get; set; }

        public ICollection<File> Files { get; set; }

        public Torrent()
        {
            Files = new HashSet<File>();
        }

    }
}
