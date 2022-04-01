using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
     [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageController(IUserRepository userRepository,IMessageRepository messageRepository,IMapper mapper )
        {
            _userRepository = userRepository;
           _messageRepository = messageRepository;
           _mapper=mapper;
        }

        // public Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessageDTO)
        // {
        //    /// return CreateMessage(createMessageDTO, _mapper);
        // }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>>CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var username=User.getUserName();
           if( username==createMessageDTO.RecipientUsername.ToLower())
           return BadRequest("You cannot send messages to yourself");
           var sender=await _userRepository.GetUserByUsernameAsync(username);
           var recipient=await _userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUsername);
           if(recipient==null) return NotFound();
           var message=new Message{
               Sender=sender,
               Recipient=recipient,
               SenderUsername=sender.UserName,
               RecipientUsername=recipient.UserName,
               Content=createMessageDTO.Content
           };
           _messageRepository.AddMessage(message);
           if(await _messageRepository.SaveAllAsync()) 
           return Ok(_mapper.Map<MessageDTO>(message));
           return BadRequest("Failed to send message");
           
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>>GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username=User.getUserName();
            var messages=await _messageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage,messages.PageSize,messages.TotalCount,messages.TotalPages);
            return messages;
        }
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>>GetMessageThread(string username)
        {
            var currentUsername=User.getUserName();
            return Ok(await _messageRepository.GetMessageThread(currentUsername,username));
        }

    }
}