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
            CreateMap<UserDto, User>();

            CreateMap<TaskItemCreateDto, TaskItem>()
                .BeforeMap((src, dest) => dest.IsDone = false);
            CreateMap<TaskItem, TaskItemDto>();

            CreateMap<ShoppingItem, ShoppingItemDto>();
            CreateMap<ShoppingItemCreateDto, ShoppingItem>()
                .BeforeMap((src, dest) => dest.IsNeeded = false);
        }
    }
}
