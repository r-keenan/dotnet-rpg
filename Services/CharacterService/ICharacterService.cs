using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public interface ICharacterService
    {
        //Task implements Asynchronous calls, so you are not locking up threads.
        
         Task<List<Character>> GetAllCharacters();

         Task<Character> GetCharacterById(int id);

         Task<List<Character>> AddCharacter(Character newCharacter);
    }
}