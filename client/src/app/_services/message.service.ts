import { HttpClient } from '@angular/common/http';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
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
     return getPaginationResult<Message[]>(this.baseUrl+'messages',params,this.http);
     
   }
}
