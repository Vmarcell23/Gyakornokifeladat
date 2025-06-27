using AutoMapper;
using GyakorlatiFeladat.DataContext.Dtos;
using GyakorlatiFeladat.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, UserDto>();
            CreateMap<UserRegisterDto, User>();

            CreateMap<TaskItemCreateDto, TaskItem>()
                .ForMember(dest => dest.IsDone, opt => opt.MapFrom(src => false)); ;
            CreateMap<TaskItem, TaskItemDto>();

            CreateMap<ShoppingItem, ShoppingItemDto>()
                .ForMember(dest => dest.Votes,opt => opt.MapFrom(src => src.Votes.Count));
            CreateMap<ShoppingItemCreateDto, ShoppingItem>()
                .ForMember(dest => dest.IsNeeded, opt => opt.MapFrom(src => false));

            CreateMap<ShoppingItemCreateVoteDto, ShoppingItemVote>();
            CreateMap<ShoppingItemVote,ShoppingItemCreateVoteDto>();

            CreateMap<FamilyCreateDto, Family>();

            CreateMap<FamilyUsers, FamilyUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<Family, FamilyDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src =>
                    src.FamilyUsers.FirstOrDefault(fu => fu.Role == Roles.Owner).UserId))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.FamilyUsers));

            CreateMap<Family, FamilyDeatliedDto>()
                   .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src =>
                    src.FamilyUsers.FirstOrDefault(fu => fu.Role == Roles.Owner).UserId))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.FamilyUsers))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.TaskItems))
                .ForMember(dest => dest.ShoppingItems, opt => opt.MapFrom(src => src.ShoppingItems));

            CreateMap<FamilyInvite, FamilyInviteDto>();

            CreateMap<Recipe, RecipeDto>();
            CreateMap<Recipe, RecipeInMenuDto>();
            CreateMap<RecipeCreateDto, Recipe>();

            CreateMap<Menu, MenuDto>()
                .ForMember(dest => dest.Recipes, opt => opt.MapFrom(src => src.MenuRecipes.Select(mr => mr.Recipe).ToList()))
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes.Count)); ;
            CreateMap<MenuCreateDto, Menu>()
                .ForMember(dest => dest.isNeeded, opt => opt.MapFrom(src => false)); 

        }
    }
}
