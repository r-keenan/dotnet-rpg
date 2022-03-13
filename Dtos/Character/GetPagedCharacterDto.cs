using Microsoft.EntityFrameworkCore;
using dotnet_rpg.Models;

namespace dotnet_rpg.Dtos.Character
{
    public class GetPagedCharacterDto
    {

        public List<GetCharacterDto>? Characters { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }

    }
}
