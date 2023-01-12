﻿using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Item;
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
        public async Task<IActionResult> GetItems([FromQuery] SearchAssetRequest searchItemRequest, CancellationToken cancellationToken)
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

        [HttpGet("character/{uId}")]
        public async Task<IActionResult> GetItemsForCharacter(int uId, [FromQuery] GetAssetForCharacterRequest getItemRequest)
        {
            try
            {
                var result = await _itemService.GetItemsForCharacter(uId, getItemRequest);

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


        [HttpPost]
        public async Task<IActionResult> PostItem(PostItemRequest postItemRequest)
        {
            try
            {
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
    }
}
