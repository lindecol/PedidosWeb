import { Component, OnInit, Input } from "@angular/core";
import { AuthService } from "../_services/auth.service";
import { AlertifyService } from "../_services/alertify.service";
import { Router } from "@angular/router";


@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.css"]
})
export class NavComponent implements OnInit {
  model: any = {};
  isCollapsed = true;

  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}


  toggleState() { // manejador del evento
    let foo = this.isCollapsed;
    this.isCollapsed = foo === false ? true : false;
 

}

  ngOnInit() {}

  login() {
    debugger;
    this.authService.login(this.model).subscribe(
      next => {
        this.alertify.success("Ingreso Correcto");
      },
      error => {
        this.alertify.error("Ocurrio Un Error " + error);
      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("codigoPaciente");
    this.alertify.message("Ha Salido del Sistema");
    this.router.navigate(["/home"]);
  }


}
