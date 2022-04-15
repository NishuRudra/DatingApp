import { HttpClient } from '@angular/common/http';
import { Messages } from '../_models/message';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl=environment.apiUrl;
  constructor(private http:HttpClient) 
  {

   }
   getMessage(pageNumber,pageSize,container){
     debugger;
     let params=getPaginationHeaders(pageNumber,pageSize);
     params=params.append('Container',container);
     return getPaginationResult<Partial<Messages[]>>(this.baseUrl+'message',params,this.http);
     
   }
   getMessageThread(username:string){
     debugger;
     return this.http.get<Messages[]>(this.baseUrl+'message/thread/'+username);
   }
   sendMessage(username:string,content:string){
     return this.http.post<Messages>(this.baseUrl+'message',{recipientUsername:username,content})
   }
   deleteMessage(id:number)
   {
     return this.http.delete(this.baseUrl+'message/'+id);
   }
}
