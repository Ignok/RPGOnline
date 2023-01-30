using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Item;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

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
        public async Task<IActionResult> GetItems([FromQuery] SearchAssetRequest searchItemRequest, CancellationToken cancellationToken)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _itemService.GetItems(searchItemRequest, Int32.Parse(userId), cancellationToken);

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

        [Authorize]
        [HttpGet("character/{uId}")]
        public async Task<IActionResult> GetItemsForCharacter(int uId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _itemService.GetItemsForCharacter(uId);

                if (result == null)
                {
                    return NotFound("No items in database.");
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostItem(PostItemRequest postItemRequest)
        {
            try
            {
                if (!IsSameId(postItemRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _itemService.PostItem(postItemRequest);

                if (result == null)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool IsSameId(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId.Equals(id.ToString());
        }
    }
}
