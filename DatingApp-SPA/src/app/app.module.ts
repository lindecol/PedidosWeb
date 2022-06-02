import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';



import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import {FormsModule} from '@angular/forms';
import { AuthService } from './_services/auth.service';
import { BsDropdownModule, CollapseModule } from 'ngx-bootstrap';

import { AlertifyService } from './_services/alertify.service';
import { RegisterComponent } from './register/register.component';
import { PedidosComponent } from './pedidos/pedidos.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AutorizacionesComponent } from './autorizaciones/autorizaciones.component';

import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { HomeComponent } from './home/home.component';

import { ModalModule } from 'ngx-bootstrap/modal';

import { CarouselModule } from 'ngx-bootstrap/carousel';
import { LoadingComponent } from './loading/loading.component';
import { LoadingScreenInterceptor } from './_services/Loading.Interceptor';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { AuthGuard } from './_guards/auth.guard';
import { JwtModule } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';










export function obtenerToken(){
   return localStorage.getItem('token');
}


@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      RegisterComponent,
      AutorizacionesComponent,
      PedidosComponent,
      HomeComponent,
      LoadingComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      BsDropdownModule.forRoot(),
      ModalModule.forRoot(),
      CollapseModule.forRoot(),
      FormsModule,
      RouterModule.forRoot(appRoutes),
      CarouselModule.forRoot(),
      TooltipModule.forRoot(),
      JwtModule.forRoot({
         config :{
               tokenGetter: obtenerToken,
               whitelistedDomains : [environment.domain],
               blacklistedRoutes : [environment.domain+'/api/auth']
            
         }

      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard,
      {
         provide: HTTP_INTERCEPTORS,
         useClass: LoadingScreenInterceptor,
         multi: true
       }

   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
