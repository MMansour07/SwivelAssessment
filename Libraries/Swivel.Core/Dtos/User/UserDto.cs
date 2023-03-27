using Swivel.Core.Dtos.Job;
using Swivel.Core.Dtos.Role;
using Swivel.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Swivel.Core.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin
        {
            get
            {
                return this.Roles != null ? Roles.Any(x => x.RoleId == ERole.Admin) : false;
            }
        }
        public DateTime? LastActive { get; set; }
        public List<JobDto> Jobs { get; set; }
        public List<GetRoleDto> Roles { get; set; }
        public int JobsCount 
        { 
            get
            {
                return  this.Jobs != null ? this.Jobs.Count : 0;
            }
        }
    }
}