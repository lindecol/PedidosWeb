
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace DatingApp.API.Data{
    public static class LoginExtension
    {

public static IConfiguration configuration; 

          public static async Task<M_DF01> GetUser(this DbSet<M_DF01> tabla, string codi_cli, string nrodoc_cli )
        {

                var strConexion =configuration.GetConnectionString("DefaultConnection");


                 using (OracleConnection cnn = new  OracleConnection(strConexion))
                 {

                     using (OracleCommand cmd = cnn.CreateCommand())
                     {
                         try
                         {
                             cnn.Open();
                             cmd.BindByName=true;
                             cmd.CommandText=@"
                                                    Select 
                                                    client_cli,
                                                    nrodoc_cli,
                                                    razon__cli
                                                    from m_df01
                                                    where client_cli =lpad(:client_cli,7,'0')  and nrodoc_cli =:nrodoc_cli ";
                             
                             var p1 = new OracleParameter("client_cli", OracleDbType.Varchar2);
                             p1.Value=codi_cli;
                             var p2 = new OracleParameter("nrodoc_cli", OracleDbType.Varchar2);
                             p2.Value=nrodoc_cli;



                            cmd.Parameters.Add(p1);                            
                            cmd.Parameters.Add(p2);

                            OracleDataReader reader= cmd.ExecuteReader();

                            M_DF01 obj = new  M_DF01();
                            while (await reader.ReadAsync())
                            {
                                obj.client_cli=reader["client_cli"].ToString();
                                obj.nrodoc_cli=reader["nrodoc_cli"].ToString();
                                obj.razon__cli=reader["razon__cli"].ToString();

                            }

                            return obj;



                         }
                         catch (System.Exception)
                         {
                             
                             throw;
                         }



                     }
                     
                 }          

         
                                                    
        }
    }
}