namespace dotnet_rpg.Dtos.Character
{
    public class GetCharacterNameOnlyDto
    {
        public int Id { get; set; }
        // put in an equal sing and then the data type is for setting a default value
        public string Name { get; set; } = "Frodo";
    }
}
