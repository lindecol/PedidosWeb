import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import{JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper= new JwtHelperService();
  decodedToken: any;


constructor(private http:HttpClient) {

 }

 
 login ( model:any){



  const httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin':  'http://localhost:4200'
    })
  };



  return this.http.post(this.baseUrl+'Login',model)
  .pipe(
    map( (response:any)=>{
      const user = response;
      if(user){
        localStorage.setItem('token',user.token);
        localStorage.setItem('codigoPaciente',model.codigoPaciente);
        this.decodedToken=this.jwtHelper.decodeToken(user.token);
        
      }

    } 


    )
  );

 }

  loggedIn(){
    const token =localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

}
