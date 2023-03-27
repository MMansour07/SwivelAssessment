using AutoMapper;
using Swivel.Core.Model;
using Swivel.Core.Dtos.User;
using Swivel.Core.Dtos.UserInfo;
using System.Linq;
using Swivel.Core.Dtos.Job;
using Swivel.Core.Dtos.General;
using CloudinaryDotNet.Actions;
using Swivel.Core.Dtos.Media;
using Microsoft.AspNet.Identity.EntityFramework;
using Swivel.Core.Dtos.UserRole;
using Swivel.Core.Dtos.Role;

namespace Swivel.Service.Infrastructure
{
    public class MappingProfile : Profile
    {
        public static IMapper Mapper { get; private set; }
        public static MapperConfiguration MapperConfiguration { get; private set; }
        public static IMapper Init()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, RegisterUserDto>().ReverseMap();
                cfg.CreateMap<User, SigninUserDto>().ReverseMap();
                cfg.CreateMap<User, UserInfoDto>().ReverseMap();
                cfg.CreateMap<User, UserEditDto>().ReverseMap();
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Job, NewJobDto>().ReverseMap();
                cfg.CreateMap<IdentityUserRole, GetRoleDto>().ReverseMap();
                cfg.CreateMap<Job, EditJobDto>().ReverseMap();
                cfg.CreateMap<Job, EditJobPostDto>().ReverseMap();
                cfg.CreateMap<Media, MediaDto>().ReverseMap();
                cfg.CreateMap<IdentityUserRoleDto, IdentityUserRole>().ReverseMap();
                cfg.CreateMap<PagedList<Job>, TableModel<JobDto>>().ConvertUsing<TableModelConverter>();
                cfg.CreateMap<PagedList<User>, TableModel<UserDto>>().ConvertUsing<UserModelConverter>();
                cfg.CreateMap<Job, JobDto>().ReverseMap();
                cfg.CreateMap<VideoUploadResult, Media>();
                cfg.CreateMap<RegisterUserDto, SigninUserDto>().ReverseMap();
            });
            return Mapper = MapperConfiguration.CreateMapper();
        }
    }
    public class TableModelConverter : ITypeConverter<PagedList<Job>, TableModel<JobDto>>
    {
        public TableModel<JobDto> Convert(PagedList<Job> source, TableModel<JobDto> destination, ResolutionContext context)
        {
            var model = source;
            var vm = model.Select(m => MappingProfile.Mapper.Map<Job, JobDto>(m)).ToList();
            var data = new PagedList<JobDto>(vm, model.Count, model.CurrentPage, model.PageSize, model.TotalCount);
            return new TableModel<JobDto>() { meta = new Meta() { page = source.CurrentPage, pages = source.TotalPages, perpage = source.PageSize, total = source.TotalCount, totalFiltered = source.TotalCount }, data = data };
        }
    }

    public class UserModelConverter : ITypeConverter<PagedList<User>, TableModel<UserDto>>
    {
        public TableModel<UserDto> Convert(PagedList<User> source, TableModel<UserDto> destination, ResolutionContext context)
        {
            var model = source;
            var vm = model.Select(m => MappingProfile.Mapper.Map<User, UserDto>(m)).ToList();
            var data = new PagedList<UserDto>(vm, model.Count, model.CurrentPage, model.PageSize, model.TotalCount);
            return new TableModel<UserDto>() { meta = new Meta() { page = source.CurrentPage, pages = source.TotalPages, perpage = source.PageSize, total = source.TotalCount, totalFiltered = source.TotalCount }, data = data };
        }
    }
}
