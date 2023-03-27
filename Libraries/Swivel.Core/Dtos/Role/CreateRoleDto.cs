using System.ComponentModel.DataAnnotations;

namespace Swivel.Core.Dtos.Role
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }
    }
}