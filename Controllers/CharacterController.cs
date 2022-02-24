using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    //This means that the controller can be accessed by its name
    [Route("[controller]")]
    //ControllerBase is used when you do not want a View implemented. Controller is used when including a View.
    public class CharacterController : ControllerBase
    {
        private static Character knight = new Character();

        //If decorater is not added, then you can get the "Ambiguous HTTP method for Action" error.
        [HttpGet]
        public ActionResult<Character> Get()
        {
            //status code 200
            return Ok(knight);
        }
    }
}