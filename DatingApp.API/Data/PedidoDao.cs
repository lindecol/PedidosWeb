
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;




namespace DatingApp.API.Data
{
    public static class PedidoDao
    {
        public static IConfiguration configuration;
        public static async Task<List<Pedido>> GetPedidos(string CodigoPaciente)
        {
            var strConexion = configuration.GetConnectionString("DefaultConnection");


            using (OracleConnection cnn = new OracleConnection(strConexion))
            {

                List<PedidoPlano> listaPedidos = new List<PedidoPlano>();
                List<Pedido> listaPedidosFinal = new List<Pedido>();

                using (OracleCommand cmd = cnn.CreateCommand())
                {
                    try
                    {
                        cnn.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = @"select 
                                            cab.docidnum,
                                            cab.docnro,
                                            to_char(cab.docfecha,'dd/mm/yyyy') docfecha ,                                          
                                            cab.doccliproid,
                                            (
                                            select deta101 from m_tabdes where codta01 =139 and codde01 =lin.estadoid) estado,
                                            cab.estadoid,
                                            cab.rutadoc,
                                            cab.doccomentario,
                                            lin.dclartid,
                                            docdirclipro,
                                            lin.dcldsc,
                                             LIN.DCLCNT/decode(nvl(lin.dclcapacidad,1),0,1,nvl(lin.dclcapacidad,1)) cantidad,
                                             to_char(CAB.docfecha2, 'dd/mm/yyyy') docfecha2
                                            from doccabezal cab
                                            inner join doclineas lin on cab.docidnum = lin.docidnum
                                            INNER JOIN M_CLDOMI DOM ON DOM.CLAVE_DOM= CAB.VNDIDNUM
                                            where CAB.wizid=158
                                            AND LIN.ESTADOID!=101
                                            AND SUBSTR(DCLARTID,0,1)=0
                                            and cab.doccliproid= LPAD(:p_cliente,7,'0')
                                            and to_char(cab.docfecha,'MM-YYYY')= to_char(SYSDATE,'MM-YYYY')                                           
                                            ORDER BY DOCFECHA2 DESC";

                        var p1 = new OracleParameter("p_cliente", OracleDbType.Varchar2);
                        p1.Value = CodigoPaciente;




                        cmd.Parameters.Add(p1);


                        OracleDataReader reader = cmd.ExecuteReader();


                        while (await reader.ReadAsync())
                        {
                            PedidoPlano obj = new PedidoPlano();
                            obj.Cantidad = (reader["Cantidad"].ToString());
                            obj.CodigoPedido = reader["docnro"].ToString();
                            obj.CodigoProducto = reader["dclartid"].ToString();
                            obj.Direccion = reader["docdirclipro"].ToString();
                            obj.Estado = reader["estado"].ToString();
                            obj.FechaEntrega = reader["docfecha2"].ToString();
                            obj.FechaTomaPedido = reader["docfecha"].ToString();
                            obj.NombreProducto = reader["dcldsc"].ToString();
                            obj.Observaciones = reader["doccomentario"].ToString();
                            obj.Ruta = reader["rutadoc"].ToString();
                            obj.Docidnum = reader["docidnum"].ToString();
                            obj.Cantidad = reader["Cantidad"].ToString();

                            listaPedidos.Add(obj);

                        }



                        /*    var distinctLista =
                            listaPedidos.GroupBy(o=> new {
                               o.Docidnum,
                               o.CodigoPedido,
                               o.Direccion,
                               o.Estado,
                               o.FechaEntrega,
                               o.FechaTomaPedido,                               
                               o.Observaciones,
                               o.Ruta
                               }).Select(o=>o.FirstOrDefault());*/


                        listaPedidosFinal = listaPedidos.GroupBy(o => new
                        {
                            o.Docidnum,
                            o.CodigoPedido,
                            o.Direccion,
                            o.Estado,
                            o.FechaEntrega,
                            o.FechaTomaPedido,
                            o.Observaciones,
                            o.Ruta
                        }).Select(o => o.FirstOrDefault()).Select(p => new Pedido
                        {
                            Docidnum = p.Docidnum,
                            CodigoPedido = p.CodigoPedido,
                            Direccion = p.Direccion,
                            Estado = p.Estado,
                            FechaEntrega = p.FechaEntrega,
                            FechaTomaPedido = p.FechaTomaPedido,
                            Observaciones = p.Observaciones,
                            Ruta = p.Ruta,
                            verDetalle = false

                        }).ToList();

                        foreach (var item in listaPedidosFinal)
                        {
                            item.Lineas = new List<LineaProducto>();
                            item.Lineas = listaPedidos.Where(p => p.Docidnum == item.Docidnum)
                                                    .Select(p => new LineaProducto
                                                    {
                                                        CodigoProducto = p.CodigoProducto,
                                                        NombreProducto = p.NombreProducto,
                                                        Cantidad = p.Cantidad
                                                    }).ToList();

                        }
                        return listaPedidosFinal;

                    }
                    catch (System.Exception)
                    {

                        throw;
                    }


                }

            }
        }


        public static async Task<List<Autorizacion>> GetAutorizaciones(string CodigoPaciente)
        {
            var strConexion = configuration.GetConnectionString("DefaultConnection");


            using (OracleConnection cnn = new OracleConnection(strConexion))
            {

                List<AutorizacionPlana> listaAutorizaciones = new List<AutorizacionPlana>();
                List<Autorizacion> listaAutorizacionesFinal = new List<Autorizacion>();

                using (OracleCommand cmd = cnn.CreateCommand())
                {
                    try
                    {
                        cnn.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = @"SELECT * FROM ROADNET.VW_PEDIDOSWEB_AUT
                                        WHERE IDPACIENTE   = LPAD(:P_ID_PACIENTE,7,'0')";

                        var p1 = new OracleParameter("P_ID_PACIENTE", OracleDbType.Varchar2);
                        p1.Value = CodigoPaciente;




                        cmd.Parameters.Add(p1);


                        OracleDataReader reader = cmd.ExecuteReader();


                        while (await reader.ReadAsync())
                        {
                            AutorizacionPlana obj = new AutorizacionPlana();
                            obj.FECHA_INI_AUT = (reader["FECHA_INI_AUT"].ToString());
                            obj.IDPACIENTE = (reader["IDPACIENTE"].ToString());
                            obj.MDDDSC = (reader["MDDDSC"].ToString());
                            obj.NOMBRE_ENTIDAD = (reader["NOMBRE_ENTIDAD"].ToString());
                            obj.NOMBRE_PACIENTE = (reader["NOMBRE_PACIENTE"].ToString());
                            obj.NUMERO_AUTORIZACION_ENTIDAD = (reader["NUMERO_AUTORIZACION_ENTIDAD"].ToString());
                            obj.NUM_ID_AUTORIZACION = (reader["NUM_ID_AUTORIZACION"].ToString());
                            obj.NUM_ID_DETALLE = (reader["NUM_ID_DETALLE"].ToString());
                            obj.NUM_ID_PAQUETE = (reader["NUM_ID_PAQUETE"].ToString());
                            obj.ARTDSCVEN = (reader["ARTDSCVEN"].ToString());
                            obj.ARTID = (reader["ARTID"].ToString());
                            obj.CANTIDAD_AUTORIZADA = double.Parse(reader["CANTIDAD_AUTORIZADA"].ToString());
                            obj.CANTIDAD_MAX_ASIGNAR = double.Parse(reader["CANTIDAD_MAX_ASIGNAR"].ToString());
                            obj.CANT_CONSUMO = double.Parse(reader["CANT_CONSUMO"].ToString());
                            obj.CLIENT_ENTIDAD = (reader["CLIENT_ENTIDAD"].ToString());
                            obj.ESTADO_AUT = (reader["estado"].ToString());
                            obj.FECHA_FIN_AUT = (reader["FECHA_FIN_AUT"].ToString());
                            obj.asignado=  int.Parse(reader["equipo_asignado"].ToString());

                            listaAutorizaciones.Add(obj);

                        }

                        listaAutorizacionesFinal = listaAutorizaciones.GroupBy(p => new
                        {
                            p.NUM_ID_AUTORIZACION,
                            p.NUMERO_AUTORIZACION_ENTIDAD,
                            p.IDPACIENTE,
                            p.NOMBRE_PACIENTE,
                            p.CLIENT_ENTIDAD,
                            p.NOMBRE_ENTIDAD,
                            p.FECHA_INI_AUT,
                            p.FECHA_FIN_AUT,
                            p.ESTADO_AUT
                        }).Select(p => p.FirstOrDefault()).Select(p =>
                         new Autorizacion
                         {
                             num_id_autorizacion = p.NUM_ID_AUTORIZACION,
                             numero_autorizacion_entidad = p.NUMERO_AUTORIZACION_ENTIDAD,
                             idpaciente = p.IDPACIENTE,
                             nombre_paciente = p.NOMBRE_PACIENTE,
                             client_entidad = p.CLIENT_ENTIDAD,
                             nombre_entidad = p.NOMBRE_ENTIDAD,
                             fecha_ini_aut = p.FECHA_INI_AUT,
                             fecha_fin_aut = p.FECHA_FIN_AUT,
                             estado_aut = p.ESTADO_AUT



                         }
                            ).ToList();

                        foreach (var item in listaAutorizacionesFinal)
                        {
                            item.LineasAutorizacion = new List<AutorizacionDetalle>();
                            item.LineasAutorizacion = listaAutorizaciones.Where(p => p.NUM_ID_AUTORIZACION == item.num_id_autorizacion)
                                                    .Select(p => new AutorizacionDetalle
                                                    {
                                                        num_id_autorizacion = p.NUM_ID_AUTORIZACION,
                                                        num_id_detalle = p.NUM_ID_DETALLE,
                                                        artid = p.ARTID,
                                                        mdddsc = p.MDDDSC,
                                                        artdscven = p.ARTDSCVEN,
                                                        cantidad_autorizada = p.CANTIDAD_AUTORIZADA,
                                                        canitad_max_asignar = p.CANTIDAD_MAX_ASIGNAR,
                                                        cant_consumo = p.CANT_CONSUMO,
                                                        seleccionado = false,
                                                        asignado=p.asignado
                                                    }).ToList();

                        }
                        return listaAutorizacionesFinal;

                    }
                    catch (System.Exception ex)
                    {

                        throw (ex);
                    }


                }

            }
        }



        public static async Task<List<Domicilio>> GetDomiciliosActivos(string CodigoPaciente)
        {
            var strConexion = configuration.GetConnectionString("DefaultConnection");


            using (OracleConnection cnn = new OracleConnection(strConexion))
            {

                List<Domicilio> ListaDomicilios = new List<Domicilio>();


                using (OracleCommand cmd = cnn.CreateCommand())
                {
                    try
                    {
                        cnn.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = @"select clave_dom,domici_dom from m_cldomi
                                            where client_dom =lpad(:p_client_dom,7,'0')
                                            and estado_aud ='A'";

                        var p1 = new OracleParameter("p_client_dom", OracleDbType.Varchar2);
                        p1.Value = CodigoPaciente;




                        cmd.Parameters.Add(p1);


                        OracleDataReader reader = cmd.ExecuteReader();


                        while (await reader.ReadAsync())
                        {
                            Domicilio obj = new Domicilio();
                            obj.clave_dom = (reader["clave_dom"].ToString());
                            obj.direccion = (reader["DOMICI_DOM"].ToString());


                            ListaDomicilios.Add(obj);

                        }

                        return ListaDomicilios;

                    }

                    catch (System.Exception ex)
                    {

                        throw (ex);
                    }


                }

            }

        }


        public static async Task<int> AlmacenarEncabezadoPedido(string CodigoPaciente, string domicilio)
        {
            var strConexion = configuration.GetConnectionString("DefaultConnection");


            using (OracleConnection cnn = new OracleConnection(strConexion))
            {

                List<Domicilio> ListaDomicilios = new List<Domicilio>();


                using (OracleCommand cmd = cnn.CreateCommand())
                {
                    try
                    {
                        cnn.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = @"ROADNET.pedidosWeb.INS_PSH_PRIMERSERVICIO";
                        cmd.CommandType = CommandType.StoredProcedure;


                        var p1 = new OracleParameter("IN_IDPACIENTE", OracleDbType.Varchar2);
                        p1.Value = CodigoPaciente;
                        var p2 = new OracleParameter("IN_DOCIDNUM", OracleDbType.Int64);
                        p2.Value = 0;
                        var p3 = new OracleParameter("IN_NUM_ID_AUTORIZACION", OracleDbType.Int64);
                        p3.Value = 0;

                        var p9 = new OracleParameter("IN_OPORTUNIDAD", OracleDbType.Date);
                        p9.Value = DateTime.Now;
                        var p10 = new OracleParameter("IN_TIPIFICACION_OPORTUNIDAD", OracleDbType.Int64);
                        p10.Value = 1;
                        var p11 = new OracleParameter("IN_TIPO_USUARIO", OracleDbType.Int64);
                        p11.Value = 1;
                        var p12 = new OracleParameter("IN_OBSERVACIONES_MALLA", OracleDbType.Varchar2);
                        p12.Value = "WEB";
                        var p14 = new OracleParameter("IN_USUARIO_AUD", OracleDbType.Int64);
                        p14.Value = 78;
                        var p15 = new OracleParameter("IN_ESTADO_REGISTRO", OracleDbType.Int64);
                        p15.Value = 1;

                        var p18 = new OracleParameter("IN_USUARIO_MODIFICACION", OracleDbType.Int64);
                        p18.Value = 78;
                        var p19 = new OracleParameter("OUT_ID", OracleDbType.Int64);
                        p19.Direction = ParameterDirection.Output;
                        p19.Value = 1;
                        var p20 = new OracleParameter("SUCURSAL", OracleDbType.Int64);
                        p20.Value = 1;
                        var p21 = new OracleParameter("DOMICILIO", OracleDbType.Int64);
                        p21.Value = domicilio;
                        var p22 = new OracleParameter("RUTADOC", OracleDbType.Varchar2);
                        p22.Value = "AU_0001";
                        var p23 = new OracleParameter("DOCSOLICITANTENOMBRE", OracleDbType.Varchar2);
                        p23.Value = "WEB";
                        var p24 = new OracleParameter("p_CPAPBPAP", OracleDbType.Int32);
                        p24.Value = 1;
                        var p25 = new OracleParameter("p_TIPO_MASCARA", OracleDbType.Int64);
                        p25.Value = 1;
                        var p26 = new OracleParameter("p_CMH2O", OracleDbType.Int64);
                        p26.Value = 1;
                        var p27 = new OracleParameter("p_TALLA", OracleDbType.Int64);
                        p27.Value = 1;
                        var p28 = new OracleParameter("P_VALOR_CUOTA", OracleDbType.Int64);
                        p28.Value = 0;


                        cmd.Parameters.Add(p1);
                        cmd.Parameters.Add(p2);
                        cmd.Parameters.Add(p3);
                        // cmd.Parameters.Add(p4); 
                        //cmd.Parameters.Add(p5); 
                        //cmd.Parameters.Add(p6); 
                        //cmd.Parameters.Add(p7); 
                        //md.Parameters.Add(p8); 
                        cmd.Parameters.Add(p9);
                        cmd.Parameters.Add(p10);
                        cmd.Parameters.Add(p11);
                        cmd.Parameters.Add(p12);
                        //cmd.Parameters.Add(p13); 
                        cmd.Parameters.Add(p14);
                        cmd.Parameters.Add(p15);
                        //cmd.Parameters.Add(p16); 
                        //cmd.Parameters.Add(p17); 
                        cmd.Parameters.Add(p18);
                        cmd.Parameters.Add(p19);
                        cmd.Parameters.Add(p20);
                        cmd.Parameters.Add(p21);
                        cmd.Parameters.Add(p22);
                        cmd.Parameters.Add(p23);
                        cmd.Parameters.Add(p24);
                        cmd.Parameters.Add(p25);
                        cmd.Parameters.Add(p26);
                        cmd.Parameters.Add(p27);
                        cmd.Parameters.Add(p28);

                        await cmd.ExecuteNonQueryAsync();

                        var seq = cmd.Parameters["OUT_ID"].Value.ToString();

                        return int.Parse(seq);
                    }

                    catch (System.Exception ex)
                    {

                        throw (ex);
                    }


                }

            }

        }


        public static async Task<Boolean> GuardarDetallePedido(int idDetalle, string codigoPaciente, string domicilio, int autorizacion, string producto)
        {
            var strConexion = configuration.GetConnectionString("DefaultConnection");


            using (OracleConnection cnn = new OracleConnection(strConexion))
            {

                List<Domicilio> ListaDomicilios = new List<Domicilio>();


                using (OracleCommand cmd = cnn.CreateCommand())
                {
                    try
                    {
                        cnn.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = @"ROADNET.pedidosWeb.INS_PSH_PRIMERSERVICIO_DET";
                        cmd.CommandType = CommandType.StoredProcedure;


                        var p1 = new OracleParameter("IN_IDPS", OracleDbType.Int64);
                        p1.Value = idDetalle;
                        var p2 = new OracleParameter("IN_DOCIDNUM", OracleDbType.Int64);
                        p2.Value = 0;
                        var p3 = new OracleParameter("IN_DCLNROLIN", OracleDbType.Int64);
                        p3.Value = 1;
                        var p4 = new OracleParameter("IN_GRPECID", OracleDbType.Int64);
                        p4.Value = 1;
                        var p5 = new OracleParameter("IN_EMPID", OracleDbType.Int64);
                        p5.Value = 21;
                        var p6 = new OracleParameter("IN_SCRID", OracleDbType.Int64);
                        p6.Value = 1;
                        var p7 = new OracleParameter("IN_WIZID", OracleDbType.Int64);
                        p7.Value = 158;
                        var p8 = new OracleParameter("IN_DOCSERIE", OracleDbType.Int64);
                        p8.Value = 1;
                        var p9 = new OracleParameter("IN_DOCNRO", OracleDbType.Int64);
                        p9.Value = 1;
                        var p10 = new OracleParameter("IN_DOCACR", OracleDbType.Int64);
                        p10.Value = 1;
                        var p11 = new OracleParameter("IN_DCLARTIDNUM", OracleDbType.Int64);
                        p11.Value = 1;
                        var p12 = new OracleParameter("IN_DCLARTID", OracleDbType.Varchar2);
                        p12.Value = producto;
                        var p13 = new OracleParameter("IN_DCLDSC", OracleDbType.Varchar2);
                        p13.Value = 1;
                        var p14 = new OracleParameter("IN_DCLCNT", OracleDbType.Int64);
                        p14.Value = 1;
                        var p15 = new OracleParameter("IN_DCLPRECIO", OracleDbType.Int64);
                        p15.Value = 1;
                        var p16 = new OracleParameter("IN_DCLDESC", OracleDbType.Int64);
                        p16.Value = 1;
                        var p17 = new OracleParameter("IN_DCLREC", OracleDbType.Int64);
                        p17.Value = 1;
                        var p18 = new OracleParameter("IN_DCLIMPORTE", OracleDbType.Int64);
                        p18.Value = 1;
                        var p19 = new OracleParameter("IN_DCLDEPORI", OracleDbType.Int64);
                        p19.Value = 1;
                        var p20 = new OracleParameter("IN_DCLDEPDES", OracleDbType.Int64);
                        p20.Value = 1;
                        var p21 = new OracleParameter("IN_DCLESTORI", OracleDbType.Int64);
                        p21.Value = 1;
                        var p22 = new OracleParameter("IN_DCLESTDES", OracleDbType.Int64);
                        p22.Value = 1;
                        var p23 = new OracleParameter("IN_DCLCNTMOVIDA", OracleDbType.Int64);
                        p23.Value = 1;
                        var p24 = new OracleParameter("IN_PCODE", OracleDbType.Varchar2);
                        p24.Value = "P";
                        var p25 = new OracleParameter("IN_MDDID", OracleDbType.Varchar2);
                        p25.Value = 1;
                        var p26 = new OracleParameter("IN_ESTADOID", OracleDbType.Varchar2);
                        p26.Value = 101;
                        var p27 = new OracleParameter("IN_DCLCAPACIDAD", OracleDbType.Int64);
                        p27.Value = 1;
                        var p28 = new OracleParameter("IN_NUM_ID_TIPOASIGNACION", OracleDbType.Int64);
                        p28.Value = 0;

                        var p29 = new OracleParameter("IN_NUM_ID_AUTORIZACION", OracleDbType.Int64);
                        p29.Value = autorizacion;
                        var p30 = new OracleParameter("IN_DCLCONDPAGO", OracleDbType.Int64);
                        p30.Value = 0;

                        cmd.Parameters.Add(p1);
                        cmd.Parameters.Add(p2);
                        cmd.Parameters.Add(p3);
                        cmd.Parameters.Add(p4);
                        cmd.Parameters.Add(p5);
                        cmd.Parameters.Add(p6);
                        cmd.Parameters.Add(p7);
                        cmd.Parameters.Add(p8);
                        cmd.Parameters.Add(p9);
                        cmd.Parameters.Add(p10);
                        cmd.Parameters.Add(p11);
                        cmd.Parameters.Add(p12);
                        cmd.Parameters.Add(p13);
                        cmd.Parameters.Add(p14);
                        cmd.Parameters.Add(p15);
                        cmd.Parameters.Add(p16);
                        cmd.Parameters.Add(p17);
                        cmd.Parameters.Add(p18);
                        cmd.Parameters.Add(p19);
                        cmd.Parameters.Add(p20);
                        cmd.Parameters.Add(p21);
                        cmd.Parameters.Add(p22);
                        cmd.Parameters.Add(p23);
                        cmd.Parameters.Add(p24);
                        cmd.Parameters.Add(p25);
                        cmd.Parameters.Add(p26);
                        cmd.Parameters.Add(p27);
                        cmd.Parameters.Add(p28);
                        cmd.Parameters.Add(p29);
                        cmd.Parameters.Add(p30);


                        await cmd.ExecuteNonQueryAsync();
                        return true;

                    }

                    catch (System.Exception ex)
                    {

                        throw (ex);
                    }


                }

            }

        }


        public static async Task<Object> ProcesarPedido(int idDetalle)
        {
            var strConexion = configuration.GetConnectionString("DefaultConnection");


            using (OracleConnection cnn = new OracleConnection(strConexion))
            {

                List<Domicilio> ListaDomicilios = new List<Domicilio>();


                using (OracleCommand cmd = cnn.CreateCommand())
                {
                    try
                    {
                        cnn.Open();
                        cmd.BindByName = true;
                        cmd.CommandText = @"ROADNET.pedidosWeb.PROCESARCARGUE";
                        cmd.CommandType = CommandType.StoredProcedure;


                        var p1 = new OracleParameter("IN_IDCARGUE", OracleDbType.Int64);
                        p1.Value = idDetalle;
                        var p2 = new OracleParameter("OUT_DOCIDNUM", OracleDbType.Int64);
                        p2.Direction = ParameterDirection.Output;
                        var p3 = new OracleParameter("OUT_DOCNRO", OracleDbType.Int64);
                        p3.Direction = ParameterDirection.Output;


                        cmd.Parameters.Add(p1);
                        cmd.Parameters.Add(p2);
                        cmd.Parameters.Add(p3);



                        await cmd.ExecuteNonQueryAsync();
                        return new
                        {
                          docnro = cmd.Parameters["OUT_DOCNRO"].Value

                        };

                    }

                    catch (System.Exception ex)
                    {

                        throw (ex);
                    }


                }

            }

        }


    }
}