using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class LoginUserForDTO
    {
        [Required]
        public string   codigoPaciente{ get; set; }

         [Required]
        public string identificacion{ get; set; }
    }
}