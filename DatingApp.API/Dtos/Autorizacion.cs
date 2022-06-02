using System.Collections.Generic;

namespace DatingApp.API.Dtos
{
    public class Autorizacion
    {
        
        public string num_id_autorizacion {get;set;}
public string  numero_autorizacion_entidad {get;set;}
public string idpaciente {get;set;}
public string nombre_paciente {get;set;}
public string client_entidad {get;set;}
public string nombre_entidad {get;set;}
public string fecha_ini_aut {get;set;}
public string fecha_fin_aut {get;set;}
public string estado_aut  {get;set;}

public List<AutorizacionDetalle> LineasAutorizacion  {get;set;}
        
    }

    public class AutorizacionPlana
    {
     
public string NUM_ID_AUTORIZACION {get;set;}
public string NUM_ID_DETALLE  {get;set;}
public string  NUMERO_AUTORIZACION_ENTIDAD {get;set;}
public string IDPACIENTE {get;set;}
public string NOMBRE_PACIENTE {get;set;}
public string CLIENT_ENTIDAD {get;set;}
public string NOMBRE_ENTIDAD {get;set;}
public string FECHA_INI_AUT {get;set;}
public string FECHA_FIN_AUT {get;set;}
public string NUM_ID_PAQUETE {get;set;}
public string ARTID {get;set;}
public string MDDDSC {get;set;}
public string ARTDSCVEN {get;set;}
public double  CANTIDAD_AUTORIZADA {get;set;}
public double  CANTIDAD_MAX_ASIGNAR {get;set;}
public double CANT_CONSUMO {get;set;}
public string ESTADO_AUT  {get;set;}

public int  asignado { get; set; } 
        
    }

    public class AutorizacionDetalle
    {

        
public string num_id_autorizacion {get;set;}
public string num_id_detalle  {get;set;}
public string artid {get;set;}
public string mdddsc {get;set;}
public string artdscven {get;set;}
public double  cantidad_autorizada {get;set;}
public double  canitad_max_asignar {get;set;}
public double cant_consumo {get;set;}
public bool seleccionado { get; set; }
public int asignado {get;set;}

        
    }


}