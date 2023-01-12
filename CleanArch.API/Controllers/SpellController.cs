using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests.Asset;
using RPGOnline.Application.DTOs.Requests.Asset.Spell;
using RPGOnline.Application.Interfaces;

namespace RPGOnline.API.Controllers
{
    public class SpellController : CommonController
    {
        private readonly ISpell _spellService;

        public SpellController(ISpell spellService)
        {
            _spellService = spellService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpells([FromQuery] SearchAssetRequest searchSpellRequest, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _spellService.GetSpells(searchSpellRequest, cancellationToken);

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
        public async Task<IActionResult> GetSpellsForCharacter(int uId, [FromQuery] GetAssetForCharacterRequest getSpellRequest)
        {
            try
            {
                var result = await _spellService.GetSpellsForCharacter(uId, getSpellRequest);

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
    }
}
