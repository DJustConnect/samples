using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ilvo.DataHub.Samples.Provider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationController : ControllerBase
    {
        [HttpPost("event")]
        public async Task<IActionResult> CreateAndPublishEvent([FromBody] Event e)
        {
            var topicCredentials = new TopicCredentials(e.Key);
            using (var client = new EventGridClient(topicCredentials))
                await client.PublishEventsAsync(new Uri(e.Topic).Host, CreateEvent(e));

            return Ok();
        }

        private static IList<EventGridEvent> CreateEvent(Event e)
        {
            return new List<EventGridEvent>
            {
                new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Data = e.Payload,
                    EventTime = DateTime.Now,
                    EventType = e.EventType,//The resource Id
                    Subject = e.Subject, //The farm number
                    DataVersion = "1.0"
                }
            };
        }
    }
}