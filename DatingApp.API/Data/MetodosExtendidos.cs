using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public static class MetodosExtendidos
    {

        public static List<valoresCurso> GetAll(this DbSet<valoresCurso> tabla)
        {
                  return new List<valoresCurso> {
new valoresCurso { Id=1, Name="asdasd"   }

                  };
        }

          public static async  Task<List<valoresCurso>> GetById(this DbSet<valoresCurso> tabla,int id)
        {
                     return  await tabla.FromSql("Select * from valoresCurso where id = {0}",id).ToListAsync();
        }

              public static async  Task<List<valoresCurso>> DeletebyId(this DbSet<valoresCurso> tabla,int id)
        {
                     return  await tabla.FromSql("Delete from valoresCurso where id = {0}",id).ToListAsync();
        }
        
    }
}