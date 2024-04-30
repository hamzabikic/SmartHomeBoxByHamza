using Microsoft.AspNetCore.Mvc;

namespace SmartHomeApi.Controllers
{
    [ApiController]
    [Route("[action]")]

    public class TimeController
    {
        [HttpGet]
        public int getMinute()
        {
            return DateTime.Now.TimeOfDay.Minutes;
        }
    }
}
