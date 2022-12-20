using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.Interfaces;

namespace RPGOnline.API.Controllers
{
    public class ItemController : CommonController
    {
        private readonly IItem _itemService;

        public ItemController(IItem itemService)
        {
            _itemService = itemService;
        }


        [HttpGet]
        public async Task<IActionResult> GetRaces([FromQuery] SearchAssetRequest searchItemRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _itemService.GetItems(searchItemRequest, cancellationToken);

                return Ok(new
                {
                    result.Item1,
                    result.pageCount
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
