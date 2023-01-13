using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.DTOs.Requests.Mail;
using RPGOnline.Application.Interfaces;
using System.Security.Claims;

namespace RPGOnline.API.Controllers
{
    [Authorize]
    public class MessageController : CommonController
    {

        private readonly IMessage _messageService;

        public MessageController(IMessage messageService)
        {
            _messageService = messageService;
        }


        // GET: api/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserMessages(int id, [FromQuery] GetMessageRequest getMessageRequest)
        {
            try
            {
                if (!IsSameId(id))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _messageService.GetUserMessages(id, getMessageRequest);
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

        // POST: api/{senderId}/send
        [HttpPost("{senderId}/send")]
        public async Task<IActionResult> PostMessage(int senderId, PostMessageRequest messageRequest)
        {
            try
            {
                if (!IsSameId(senderId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _messageService.PostMessage(senderId, messageRequest);

                if (result == null)
                {
                    return BadRequest("Something went wrong.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data);
            }
        }


        // DELETE: api/{uId}/delete/{messageId}
        [HttpDelete("{uId}/delete/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int uId, int messageId)
        {
            try
            {
                if (!IsSameId(uId))
                {
                    return BadRequest("Access denied - bad ID");
                }

                var result = await _messageService.DeleteMessage(uId, messageId);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch(Exception ex)
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
