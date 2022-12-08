using Microsoft.AspNetCore.Mvc;
using RPGOnline.Application.DTOs.Requests;
using RPGOnline.Application.Interfaces;

namespace RPGOnline.API.Controllers
{
    public class MessageController : CommonController
    {

        private readonly IMessage _messageService;

        public MessageController(IMessage messageService)
        {
            _messageService = messageService;
        }


        // GET: api/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserMessages(int id)
        {
            var result = await _messageService.GetUserMessages(id);

            if (result == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(result);
        }

        // POST: api/{senderId}/send
        [HttpPost("{senderId}/send")]
        public async Task<IActionResult> PostMessage(int senderId, MessageRequest messageRequest)
        {

            try
            {

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
    }
}
