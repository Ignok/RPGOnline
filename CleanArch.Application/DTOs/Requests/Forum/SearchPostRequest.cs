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
    public class SearchPostRequest
    {
        public int Page { get; set; }
        public string? Category { get; set; }
        public string? Search { get; set; }
        public bool OnlyFollowed { get; set; } = false;
        public bool OnlyFavourite { get; set; } = false;
    }
}
