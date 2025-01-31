import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpService } from './http.service';



@NgModule({
  imports: [
    BrowserModule, HttpClientModule
  ],
  providers: [HttpService, HttpClientModule]
})
export class AppModule { }
