using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DatingApp.API.Helpers;

namespace DatingApp.API
{
    public class Startup
    {

          public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LoginExtension.configuration=configuration;


        }
         public IConfiguration Configuration { get; }


      

       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);    
            services.AddDbContext<DataContext>(x=>x.UseOracle(Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors();
            services.AddScoped<IauthRepository,AuthRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(Options=>{
                    Options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:token").Value)),
                        ValidateIssuer=false,
                        ValidateAudience=false                         
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler(builder=> {
                        builder.Run(async context =>{
                                context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                                var error = context.Features.Get<IExceptionHandlerFeature>();
                                if (error!=null)
                                {
                                    context.Response.AddApplicationError(error.Error.Message);
                                    await context.Response.WriteAsync(error.Error.Message);
                                }

                        });
                } );
               // app.UseHsts();
            }

           // app.UseHttpsRedirection();
           app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
         
           app.UseAuthentication();
           app.UseDefaultFiles();
           app.UseStaticFiles();
            app.UseMvc(routes=>{
                routes.MapSpaFallbackRoute(
                    name:"spa-fallback",
                    defaults: new {controller= "Fallback", action="Index"}
                );
            });
        }
    }
}
