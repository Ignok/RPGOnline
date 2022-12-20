using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.Asset
{
    public class SearchAssetRequest
    {
        public int Page { get; set; }
        public string? KeyValueName { get; set; }
        public string? Search { get; set; }
        public string[] PrefferedLanguage { get; set; } = null!;
    }
}
