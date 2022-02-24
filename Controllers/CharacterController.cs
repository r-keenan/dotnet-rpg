using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using dotnet_rpg.Services.CharacterService;
using dotnet_rpg.Controllers;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    //This means that the controller can be accessed by its name
    [Route("[controller]")]
    //ControllerBase is used when you do not want a View implemented. Controller is used when including a View.
    public class CharacterController : ControllerBase
    {

        //Constructor
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        //If decorater is not added, then you can get the "Ambiguous HTTP method for Action" error.
        [HttpGet("GetAll")]
        public ActionResult<List<Character>> Get()
        {
            //status code 200 for (Ok)
            return Ok(_characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetSingle(int id)
        {
            return Ok(_characterService.GetCharacterById(id));
        }

        [HttpPost]
        public ActionResult<List<Character>> AddCharacter(Character newCharacter)
        {
            return Ok(_characterService.AddCharacter(newCharacter));
        }
    }
}