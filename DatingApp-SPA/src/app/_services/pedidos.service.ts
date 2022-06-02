import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PedidosService {

  baseUrl = environment.apiUrl+"Pedidos/";
  pedidos: any;
  codigoPaciente:any;
  codigoEncabezado:any;
  parametros:any={};
  secuencia: any;

   httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',     
      'Authorization': 'Bearer '+(localStorage.getItem('token'))
    })
  };

constructor(private http:HttpClient) { }


getPedidos (){




  this.codigoPaciente= localStorage.getItem('codigoPaciente');


  
  return this.http.get(this.baseUrl + 'GetPedidos?codigoPaciente=' +this.codigoPaciente);


 }


 getAutorizaciones()
 {
  

  this.codigoPaciente= localStorage.getItem('codigoPaciente');


  
  return this.http.get(this.baseUrl + 'GetAutorizacionesActivas?codigoPaciente=' +this.codigoPaciente);


 }


 getDomicilios(){

 
  this.codigoPaciente= localStorage.getItem('codigoPaciente');
  return this.http.get(this.baseUrl + 'GetDomicilios?codigoPaciente=' +this.codigoPaciente);





 }

 GuardarEncabezadoPedido(codigoPaciente:string, domicilio:string, autorizaciones:any)
 {

  this.parametros.codigoPaciente=codigoPaciente;
  this.parametros.domicilio=domicilio;
  this.parametros.autorizaciones=autorizaciones;


 return this.http.post(this.baseUrl+'GuardarEncabezadoPedido', this.parametros).pipe(
    map( (response:any)=>{
      const ped = response;
      if(ped){
        this.secuencia=ped;
   
          } 
    }

    )
  );

 }



}
