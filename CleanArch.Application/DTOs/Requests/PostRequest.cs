using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests
{
    [BindProperties]
    public class PostRequest
    {
        public int page { get; set; }
        public string? category { get; set; }
        public string? search { get; set; }
    }
}
