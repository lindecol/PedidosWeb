using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    
    public class ValuesController : ControllerBase
    {
        private  DataContext _context { get;set; }

        public ValuesController(DataContext context)
        {
            _context = context;
        }

        // GET api/values
          [AllowAnonymous]
        [HttpPost]        
        public   ActionResult<List<valoresCurso>> Get()
        {

 

         //  var contador=         _context.valoresCursos.FromSql("SELECT * FROM valoresCursos").ToList();
            
                 var values =   _context.valoresCursos.GetAll();               
    
                return Ok(values);
  
              
              

           // throw new Exception ("Test Exception");
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<valoresCurso>> Get(int id)
        {
        var value = await _context.valoresCursos.GetById(id);

     


                return Ok(value);
                
        }

        // POST api/values
      
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _context.valoresCursos.DeletebyId(id);

               RedirectToAction("Get","ValuesController");



        }
    }
}
