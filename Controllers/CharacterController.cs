using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models; 
using dotnet_rpg.Services.CharacterService;
using dotnet_rpg.Controllers;
using dotnet_rpg.Dtos.Character;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace dotnet_rpg.Controllers
{
    [Authorize]
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
        /// <summary>
        /// Gets the list of all Characters owned by user.
        /// </summary>
        /// <returns>Returns list of authenticated user's character(s).</returns>
        // GET: api/Employee
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetPagedCharacterDto>>>> Get(int pageNumber, int pageSize)
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //status code 200 for (Ok)
            return Ok(await _characterService.GetAllCharacters(pageNumber, pageSize));
        }

        [HttpGet("Filter")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetFilter(int intelligenceLow, int intelligenceHigh)
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            return Ok(await _characterService.GetFilter(intelligenceLow, intelligenceHigh));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("CharacterNames")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterNameOnlyDto>>>> GetCharacterNames()
        { return Ok(await _characterService.GetCharacterNames()); }
    }
}