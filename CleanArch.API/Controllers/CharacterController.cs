﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Character;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class CharacterController : CommonController
    {
        private readonly ICharacter _characterService;

        public CharacterController(ICharacter characterService)
        {
            _characterService = characterService;
        }

        [AllowAnonymous]
        [HttpGet("get/{characterId}")]
        public async Task<IActionResult> GetCharacter(int characterId)
        {
            try
            {
                var result = await _characterService.GetCharacter(characterId);

                if (result == null)
                {
                    return NotFound("No such character in database.");
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

        [AllowAnonymous]
        [HttpGet("{type}")]
        public async Task<IActionResult> GetCharacters(string type, [FromQuery] SearchAssetRequest searchAssetRequest, CancellationToken cancellationToken)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _characterService.GetCharacters(type, searchAssetRequest, Int32.Parse(userId), cancellationToken);

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

        [AllowAnonymous]
        [HttpGet("character/motivation")]
        public async Task<IActionResult> GetRandomMotivation()
        {
            try
            {
                var result = await _characterService.GetRandomMotivation();

                if (result == null)
                {
                    return NotFound("No available motivations in database.");
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

        [AllowAnonymous]
        [HttpGet("character/characteristics")]
        public async Task<IActionResult> GetRandomCharacteristics()
        {
            try
            {
                var result = await _characterService.GetRandomCharacteristics();

                if (result == null)
                {
                    return NotFound("No available characteristics in database.");
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

        [AllowAnonymous]
        [HttpGet("character/attributes")]
        public async Task<IActionResult> GetRandomAttributes()
        {
            try
            {
                var result = await _characterService.GetRandomAttributes();

                if (result == null)
                {
                    return NotFound("Something went wrong.");
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
        public async Task<IActionResult> PostCharacter(PostCharacterRequest postCharacterRequest)
        {
            try
            {
                if (!IsSameId(postCharacterRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _characterService.PostCharacter(postCharacterRequest);

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
