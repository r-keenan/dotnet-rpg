using dotnet_rpg.Models;

namespace dotnet_rpg.Dtos.Character
{
    public class UpdateCharacterDto
    {
        public int Id {get; set;}
        // put in an equal sing and then the data type is for setting a default value
        public string Name { get; set; } = "Frodo";

        public int HitPoints {get; set;} = 100;

        public int Strength { get; set; } = 10;

        public int Defense { get; set; } = 10;

        public int Intelligence {get; set;} = 10;
        
        //Enum
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}