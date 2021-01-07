using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using raitmq.api.UserCreationSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

       private readonly IUserCreationSender _userCreationSender;
        public OrderController(IUserCreationSender userCreationSender)
        {
            _userCreationSender = userCreationSender;
        }

        [HttpGet("GetOrders")]

        public string Get()
        {
            var a = "Ahmad";
            _userCreationSender.Send(a);
            return a;
           
        }
    }
}
