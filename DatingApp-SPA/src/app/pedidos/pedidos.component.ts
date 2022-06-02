import { Component, OnInit, Input } from "@angular/core";
import { PedidosService } from "../_services/pedidos.service";
import { AlertifyService } from "../_services/alertify.service";

@Component({
  selector: "app-pedidos",
  templateUrl: "./pedidos.component.html",
  styleUrls: ["./pedidos.component.css"]
})
export class PedidosComponent implements OnInit {
  @Input() isLogged: boolean;

  pedidos: any;
  contadorPedidos : any;

  constructor(
    private pedidosService: PedidosService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.contadorPedidos=0;
    
    this.pedidosService.getPedidos().subscribe(
      data => {
        this.pedidos = data;
        
      },
      err =>
        this.alertify.error("Ocurrio un error cargando los pedidos " + err),
      () => {
        this.pedidos.forEach(element => {
          this.contadorPedidos++;
        }
        );
        this.alertify.success("Pedidos Pendientes");
      }
    );


  }

  tienePedidos()  {

   
    if(this.contadorPedidos>0)
      return true;
    else
      return false;


  }

  establecerDetalle(pedido: any) {
    pedido.verDetalle = !pedido.verDetalle;
  }
}
