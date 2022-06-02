import { Component, OnInit, TemplateRef } from '@angular/core';
import { PedidosService } from '../_services/pedidos.service';
import { AlertifyService } from '../_services/alertify.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';





@Component({
  selector: 'app-autorizaciones',
  templateUrl: './autorizaciones.component.html',
  styleUrls: ['./autorizaciones.component.css']
})
export class AutorizacionesComponent implements OnInit {

  
  autorizaciones: any;
  modalRef: BsModalRef;
  domicilios : any;
  productosSeleccionados:any;
  existenItemsSeleccionados:boolean=false;
  codigoPedido :any;
  codigoEntidadAnterior: any;
  cambioEntidad:any;


  domicilio: any;
  codigoPaciente: string;
  cantidadProductosAutorizados: any;

  modalAyuda: BsModalRef;
  modalConfirmacion:BsModalRef;

  constructor(private pedidosService:PedidosService, 
              private alertify:AlertifyService,
              private modalService: BsModalService
              ) { }


getCantidadProductos(){

  

if(this.cantidadProductosAutorizados>0){
  return false;
}
else{
  return true;
}

}


  ngOnInit() {
   
    this.cantidadProductosAutorizados=0;
    this.domicilio=0;
    this.codigoEntidadAnterior=0;
    this.cambioEntidad=0;

    this.pedidosService.getAutorizaciones().subscribe(
      data => { this.autorizaciones = data},
      err => this.alertify.error("Ocurrio un error cargando las autorizaciones "+err),
      () => {
        this.autorizaciones.forEach(element => {
          if (this.codigoEntidadAnterior!=0 && this.codigoEntidadAnterior!=element.client_entidad) {
            this.cambioEntidad=1;
          }
          this.codigoEntidadAnterior=element. client_entidad;
          element.lineasAutorizacion.forEach(det => {     
              this.cantidadProductosAutorizados++;
           
          });
      
      
        
      
      
      
      });


      }
    );

    this.pedidosService.getDomicilios().subscribe(
      data => { this.domicilios = data},
      err => this.alertify.error("Ocurrio un error cargando las autorizaciones "+err)
    );

    

  }

  openModal(template: TemplateRef<any>) {

    this.existenItemsSeleccionados=false;

    this.autorizaciones.forEach(element => {
      element.lineasAutorizacion.forEach(det => {
        if(det.seleccionado){
          this.existenItemsSeleccionados=true;
        }
      });
  
  });

  if(this.existenItemsSeleccionados){
    this.modalRef = this.modalService.show(template);
    this.domicilio=0;
  }else 
  {
this.alertify.message("Debe Seleccionar un Item de la lista de autorizaciones para generar el pedido");

  }
  }

  guardar(template: TemplateRef<any>): void {
   



    this.codigoPaciente = localStorage.getItem('codigoPaciente');
   
this.guardarEncabezado(template);



  }
 
  cancelar(): void {
  
    this.modalRef.hide();
  }

  abrirAyuda(template: TemplateRef<any>){
    
    this.modalAyuda = this.modalService.show(template);
  }

  abrirConfirmacionPedido(template: TemplateRef<any>)
  {

    this.modalConfirmacion=this.modalService.show(template);

  }


  seleccionaDomicilio(dom:any){
      this.domicilio=dom;
     

  }


  evaluarAsignacion(linea:any)
  {

    if(linea.asignado ==0)
    {
      linea.seleccionado=false;
      this.alertify.message("Producto sin asignacion");
    }


  }



  guardarEncabezado(template: TemplateRef<any>){
  
    this.codigoPaciente = localStorage.getItem('codigoPaciente');
    
    this.pedidosService.GuardarEncabezadoPedido(this.codigoPaciente,this.domicilio,this.autorizaciones).subscribe(
      next => {
       
      this.codigoPedido=this.pedidosService.secuencia.docnro.value;     
      
    this.modalRef.hide();
      this.abrirConfirmacionPedido(template);
      },
      error => {
        this.alertify.error("Ocurrio Un Error " + error);
        
      }
    );


  }

 


}
