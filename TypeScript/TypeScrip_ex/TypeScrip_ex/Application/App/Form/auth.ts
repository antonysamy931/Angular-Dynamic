import { Component } from '@angular/core';

@Component({
    moduleId: module.id,
    selector: 'auth-app',
    templateUrl: "Auth.html",    
})

export class AuthComponent {
    ClickMe() {
        console.log("event fired");
    }
}
 