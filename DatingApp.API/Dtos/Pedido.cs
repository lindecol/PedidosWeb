using System.Collections.Generic;

namespace DatingApp.API.Dtos
{
    public class Pedido
    {
         public string  Docidnum { get; set; }
        public string  CodigoPedido { get; set; }
        public string  FechaEntrega { get; set; }
         public string  FechaTomaPedido { get; set; }
        public string  Estado { get; set; }
        public string  Ruta { get; set; }
        public string Observaciones { get; set; }
        public string Autorizacion { get; set; }

        public List<LineaProducto> Lineas { get; set; }
        public string Direccion { get; set; }

        public bool verDetalle { get; set; }



    } 


 public class PedidoPlano
    {
        public string  Docidnum { get; set; }
        public string  CodigoPedido { get; set; }
        public string  FechaEntrega { get; set; }
         public string  FechaTomaPedido { get; set; }
        public string  Estado { get; set; }
        public string  Ruta { get; set; }
        public string Observaciones { get; set; }
        public string Autorizacion { get; set; }

        public string Direccion { get; set; }
        
        public string   CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Cantidad { get; set; }



    } 

    public class LineaProducto 
    {
        public string   CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Cantidad { get; set; }

    }
}