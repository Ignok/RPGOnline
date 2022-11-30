﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests
{
    public class CommentRequest
    {
        public int PostId { get; set; }
        public int? ResponseCommentId { get; set; }
        public int UId { get; set; }
        public string Content { get; set; } = null!;
    }
}