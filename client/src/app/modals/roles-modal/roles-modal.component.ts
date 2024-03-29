import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { EventEmitter } from '@angular/core'

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
 @Input() UpdateSelectedRoles=new EventEmitter();
 user:User;
 roles:any[];
  constructor(public bsModalRef:BsModalRef) 
  { }

  ngOnInit(): void {
  }
  updateRoles(){
    this.UpdateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }

}
