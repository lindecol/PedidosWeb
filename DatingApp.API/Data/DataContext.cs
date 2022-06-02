using System.Collections.Generic;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;







namespace DatingApp.API.Data
{
    public class DataContext:DbContext
    {
  

        public DataContext(DbContextOptions<DataContext> options) :base(options){
        
    
        }

     
       protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("roadnet");

      

    }  
    
          public DbSet<valoresCurso> valoresCursos { get; set; }

          public DbSet<M_DF01> usuarios {get;set;}

          public DbSet<Pedido> Pedidos {get;set;}

          


      

      
        
    }
}