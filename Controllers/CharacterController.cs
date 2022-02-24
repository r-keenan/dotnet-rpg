using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models; 
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
        public async Task<ActionResult<ServiceResponse<List<Character>>>> Get()
        {
            //status code 200 for (Ok)
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Character>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> AddCharacter(Character newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }
    }
}