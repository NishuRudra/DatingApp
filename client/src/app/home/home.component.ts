import { HttpClient } from '@angular/common/http';
import { Component, OnInit,Input,Output,EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
registerMode=false;

  constructor() { }

  ngOnInit(): void {
    // this.getUsers();
  }
registerToggle(){
  this.registerMode=!this.registerMode;
}
// getUsers(){
//   this.http.get('https://localhost:5000/api/users').subscribe(users=>this.users=users);
// }
cancelRegisterMode(event:boolean)
{
this.registerMode=event;
}
}
