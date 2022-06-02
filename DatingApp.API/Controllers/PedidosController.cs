using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Controllers
{
 [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {

        public IConfiguration _config { get; }

        public PedidosController(IConfiguration config)
        {

            _config = config;
        }




       
        [HttpGet("GetAutorizacionesActivas")]
        public async Task<ActionResult> GetAutorizacionesActivas(string codigoPaciente)
        {

            PedidoDao.configuration = _config;
            List<Autorizacion> lista = new List<Autorizacion>();
            lista = await PedidoDao.GetAutorizaciones(codigoPaciente);

            return Ok(lista);


        }


        [HttpGet("GetProductoAsignado")]
        public async Task<ActionResult> GetProductoAsignado(string codigoPaciente,string codigoProducto)
        {

           var obj =  new {
                mensaje = "el producto 0101403 no asignado"

           };

            return Ok(obj);


        }



     
        [HttpGet("GetPedidos")]
        public async Task<ActionResult> GetPedidos(string codigoPaciente)
        {

            PedidoDao.configuration = _config;
            List<Pedido> lista = new List<Pedido>();
            lista = await PedidoDao.GetPedidos(codigoPaciente);

            return Ok(lista);

        }


        [HttpGet("GetDomicilios")]
        public async Task<ActionResult> GetDomicilios(string codigoPaciente)
        {

            PedidoDao.configuration = _config;
            List<Domicilio> lista = new List<Domicilio>();
            lista = await PedidoDao.GetDomiciliosActivos(codigoPaciente);


            return Ok(lista);

        }

        [HttpPost("GuardarEncabezadoPedido")]
        public async Task<ActionResult> GuardarEncabezadoPedido(EncabezadoPedido enc )
        {
            
            try
            {
                
            
            PedidoDao.configuration = _config;
            var seq = await PedidoDao.AlmacenarEncabezadoPedido(enc.codigoPaciente, enc.domicilio);
            foreach( var item in enc.autorizaciones){
                var aut = item.num_id_autorizacion;
              foreach (var item2 in item.LineasAutorizacion)
              {
                  if (item2.seleccionado)
                  {
                       await PedidoDao.GuardarDetallePedido(seq,enc.codigoPaciente,enc.domicilio,int.Parse(aut),item2.artid);
                  }
              }

            }

          var retorno=  await PedidoDao.ProcesarPedido(seq);


            
            return Ok(retorno);



        }
            catch (System.Exception ex)
            {
                
                return BadRequest(ex);
            
            }

           



        }

        [HttpPost("GuardarDetallePedido")]
        public async Task<ActionResult> GuardarDetallePedido(int seq, string codigoPaciente, string domicilio, int autorizacion, string producto)
        {
            PedidoDao.configuration = _config;
            var rsta = await PedidoDao.GuardarDetallePedido(seq, codigoPaciente, domicilio, autorizacion, producto);

            return Ok(rsta);

        }

    
        [HttpPost("ProcesarPedido")]
        public async Task<ActionResult> ProcesarPedido(int seq)
        {
            PedidoDao.configuration = _config;
            var rsta = await PedidoDao.ProcesarPedido(seq);
            return Ok(rsta);




        }

    }
}



