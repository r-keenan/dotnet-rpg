using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dotnet_rpg.Models;
using dotnet_rpg.Data;
using System.Linq;
using dotnet_rpg.Dtos.Character;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;

       private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;


        //Constructor
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        // Get the current logged in user
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters
                .Where(c => c.Id == GetUserId())
                .Select( c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<GetPagedCharacterDto> GetAllCharacters(int pageNumber, int pageSize)
        {
            //var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            const int maxPageSize = 100;
            
            //if (pageSize < maxPageSize)
            //{
                var PageSize = (double)pageSize;
                var pageCount = Math.Ceiling(_context.Characters.Count() / PageSize);

                var dbCharacters = await _context.Characters
                    .OrderBy(ow => ow.Name)
                    .Skip((pageNumber - 1) * (int)PageSize)
                    .Take((int)PageSize)
                    .Where(c => c.User.Id == GetUserId()).ToListAsync();
                    
                //var pagedCharacters = dbCharacters.OrderBy(ow => ow.Name).Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize).Take(ownerParameters.PageSize).ToList();
                var response = new GetPagedCharacterDto
                {
                    Characters = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList(),
                    CurrentPage = pageNumber,
                    Pages = (int)pageCount
                };

                
                //serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            //}
            //else
            //{
                //response = new GetPagedCharacterDto
                //{
                    //Characters = null,
                    //CurrentPage = 0,
                    //Pages = 0
                //};
            //}
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetFilter(int intelligenceLow, int intelligenceHigh)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            if (intelligenceLow != null && intelligenceHigh != null)
            {
                var dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId() && (c.Intelligence >= intelligenceLow && c.Intelligence <= intelligenceHigh)).ToListAsync();
                serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                return serviceResponse;
            }
            else
            {
                serviceResponse.Message = "You must either search with intelligence low and intelligence high, or you need to add in no search parameters.";
                serviceResponse.Success = false;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>(); 
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try{
            Character character = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

            if(character.User.Id == GetUserId())
            {
                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
             }
             else
             {
                serviceResponse.Success = false;
                serviceResponse.Message = "Character not found";
             }
            
            }

            catch(Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _context.Characters
                        .Where (c => c.User.Id == GetUserId())
                        .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterNameOnlyDto>>> GetCharacterNames()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterNameOnlyDto>>();

            try
            {
                var dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();
                serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterNameOnlyDto>(c)).ToList();
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }
    }
   
}