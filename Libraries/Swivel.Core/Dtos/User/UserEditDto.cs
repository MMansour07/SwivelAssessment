using System.ComponentModel.DataAnnotations;

namespace Swivel.Core.Dtos.User
{
    public class UserEditDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }

        [Required(ErrorMessage = "Required Field")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string Username => (this.Email);
    }
}