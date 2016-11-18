/*Basic component*/
import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule, JsonpModule } from '@angular/http';
import {
    LocationStrategy,
    HashLocationStrategy
} from '@angular/common';

/*Material design*/
import { MaterialModule } from '@angular/material'

/*Router*/
import { AppRoutingModule } from './app-routing.module';

/*App component*/
import { AppComponent }   from './app.component';
import { AuthComponent } from './Form/auth';
import { HomeComponent } from './Home/home';

/*Services*/
import { HomeService } from './Home/home.service';

@NgModule({
    imports: [
        BrowserModule,
        MaterialModule.forRoot(),
        AppRoutingModule,
        HttpModule,
        JsonpModule
    ],
    declarations: [
        AppComponent,
        AuthComponent,
        HomeComponent
    ],
    providers: [HomeService, { provide: LocationStrategy, useClass: HashLocationStrategy }],
    bootstrap: [AppComponent]
})
export class AppModule { }
