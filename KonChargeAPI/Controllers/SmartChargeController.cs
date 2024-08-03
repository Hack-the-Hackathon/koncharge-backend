using KonChargeAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Models;

namespace KonChargeAPI.Controllers
{
    /// <summary>
    /// Uses openai api to generate smart charge settings
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SmartChargeController : ControllerBase
    {
        public const int MAX_PROMPT = 500;

        public SmartChargeController()
        { }

        [HttpPost("GenerateSmartSettings")]
        public async Task<IActionResult> Get([FromBody] string prompt)
        {
            if (String.IsNullOrEmpty(prompt))
                return NoContent();

            if (prompt.Length > 500)
                return Problem("Prompt too long");

            OpenAIAPI api = new OpenAIAPI(SecretKeys.OPENAI_API);

            var chat = api.Chat.CreateConversation();
            chat.Model = new Model("gpt-4o-mini")
            {
                OwnedBy = "openai"
            };
            chat.RequestParameters.Temperature = 0;

            chat.AppendSystemMessage("You are used to help the user to find the nearest ev charging station");

            chat.AppendUserInput(prompt);

            string response = await chat.GetResponseFromChatbotAsync();

            return Ok(response);
        }
    }
}
