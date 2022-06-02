using System.Collections.Generic;

namespace DatingApp.API.Dtos
{
    public class EncabezadoPedido
    {
        public string codigoPaciente { get; set; } 
        public string domicilio { get; set; }  

        public List<Autorizacion> autorizaciones{get;set;}
    }
}