using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class AuthRepository : IauthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<M_DF01> Login(string codi_cli, string nrodoc_cli)
        {
            return await _context.usuarios.GetUser(codi_cli,nrodoc_cli);
        }

    }
}