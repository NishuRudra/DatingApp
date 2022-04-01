export interface Messages
{
    id:number;
    senderId:number;
    senderUsername:number;
    senderPhotoUrl:string;
    recipientId:number;
    recipientUsername:string;
    recipientPhotoUrl:string;
    content:string,
    dateRead?:Date;
    messageSent:Date;
}