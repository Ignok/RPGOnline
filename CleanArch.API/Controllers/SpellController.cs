using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Spell;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class SpellController : CommonController
    {
        private readonly ISpell _spellService;

        public SpellController(ISpell spellService)
        {
            _spellService = spellService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetSpells([FromQuery] SearchAssetRequest searchSpellRequest, CancellationToken cancellationToken)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";
                
                var result = await _spellService.GetSpells(searchSpellRequest, Int32.Parse(userId), cancellationToken);

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
        public async Task<IActionResult> GetSpellsForCharacter(int uId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _spellService.GetSpellsForCharacter(uId);

                if (result == null)
                {
                    return NotFound("No spells in database.");
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
        public async Task<IActionResult> PostSpell(PostSpellRequest postSpellRequest)
        {
            try
            {
                if (!IsSameId(postSpellRequest.UId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _spellService.PostSpell(postSpellRequest);

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

        [HttpDelete("delete/{spellId}")]
        public async Task<IActionResult> DeleteSpell(int spellId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                var result = await _spellService.DeleteSpell(spellId, Int32.Parse(userId), IsAdminRole());

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
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "a";

            return userId.Equals(id.ToString());
        }

        private bool IsAdminRole()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userRole = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value ?? "a";

            return userRole.Equals("admin");
        }
    }
}
