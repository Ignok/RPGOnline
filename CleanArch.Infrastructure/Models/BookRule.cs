using System;
using System.Collections.Generic;

namespace RPGOnline.Infrastructure.Models
{
    public partial class BookRule
    {
        public int BookRulesId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
