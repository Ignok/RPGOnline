using System;
using System.Collections.Generic;

namespace RPGOnline.Domain.Models
{
    public partial class Motivation
    {
        public int MotivationId { get; set; }
        public string Objective { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string WhatHappened { get; set; } = null!;
        public string WhereHappened { get; set; } = null!;
        public string HowHappened { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public string Language { get; set; } = null!;
    }
}
