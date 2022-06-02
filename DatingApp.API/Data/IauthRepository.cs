using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IauthRepository
    {
         
          Task<M_DF01> Login (string codi_cli, string nrodoc_cli);
    }
}