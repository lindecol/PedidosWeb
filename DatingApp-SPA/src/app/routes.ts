import {Routes} from '@angular/router';
import { AutorizacionesComponent } from './autorizaciones/autorizaciones.component';
import { PedidosComponent } from './pedidos/pedidos.component';

import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes:Routes=[

    {path: '',component: HomeComponent},
    {
     path: '' ,
     runGuardsAndResolvers  : 'always',
     canActivate: [AuthGuard],
     children:[
        {path: 'autorizaciones',component: AutorizacionesComponent},
        {path: 'pedidos',component: PedidosComponent}


     ]
    },

    {path: '**',redirectTo:'', pathMatch:'full' }
]
