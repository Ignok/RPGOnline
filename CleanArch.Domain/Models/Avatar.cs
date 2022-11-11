using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Avatar
    {
        public Avatar()
        {
            Characters = new HashSet<Character>();
        }

        public int AvatarId { get; set; }
        public int? AuthorUId { get; set; }
        public string AvatarName { get; set; } = null!;
        public string? Picture { get; set; }

        public virtual User? AuthorU { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
