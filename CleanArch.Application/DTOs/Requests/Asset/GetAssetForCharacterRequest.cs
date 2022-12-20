using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGOnline.Application.DTOs.Requests.Asset
{
    public class GetAssetForCharacterRequest
    {
        public string KeyValueName { get; set; } = null!;
        public string[] PrefferedLanguage { get; set; } = null!;
    }
}
