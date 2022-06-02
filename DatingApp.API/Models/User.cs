namespace DatingApp.API.Models
{
    public class M_DF01
    {


        public string client_cli { get; set; }
        public string nrodoc_cli { get; set; }

        public string razon__cli { get; set; }

        public byte[] PasswordHash { get; set; } 
        public byte[] PasswordSalt { get; set; }
       


    }
}