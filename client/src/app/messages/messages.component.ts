import { Component, OnInit } from '@angular/core';
import { Messages } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
messages:Messages[]=[];
pagination:Pagination;
container='Unread';
pageNumber=1;
pageSize=5;
loading=false;
  constructor(private messageService:MessageService) { }

  ngOnInit(): void {
   this.loadMessages();
  }
  loadMessages(){
    debugger;
    this.loading=true;
    this.messageService.getMessage(this.pageNumber,this.pageSize,this.container).subscribe(response=>{
      this.messages=response.result;
      this.pagination=response.pagination;
      this.loading=false;
    }
      
      )
      
  }
  pageChanged(event:any){
    if(this.pageNumber!=event.page){
      this.pageNumber=event.page;
      this.loadMessages();    
    }
    
  } 

}
