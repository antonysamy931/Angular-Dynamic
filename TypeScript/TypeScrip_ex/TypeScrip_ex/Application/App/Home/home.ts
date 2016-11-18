import { Component, OnInit } from '@angular/core';
import { HomeService } from './home.service';

@Component({
    moduleId: module.id,
    selector: 'home-app',
    templateUrl: "Index.html",
})

export class HomeComponent implements OnInit{
    data: string[];
    error: string;

    constructor(private homeService: HomeService) { }   
     
    ngOnInit(): void {
        this.homeService.GetValues().
        subscribe(                
            data => this.data = data                
            );        
    }

}
  