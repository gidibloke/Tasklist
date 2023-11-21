using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webapp.Models;
using Webapp.ViewModels;

namespace Webapp.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tasks, TasksViewModel>().ReverseMap();
        }
    }
}