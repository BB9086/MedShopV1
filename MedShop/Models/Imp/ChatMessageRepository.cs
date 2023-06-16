using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly CoreDBContext context;
        public ChatMessageRepository(CoreDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<ChatMessage> GetListOfAllMessages()
        {
            return context.ChatMessages;
        }

        public List<ChatMessage> GetListOfChatMessageByUnreadMessage(string receiverName, bool notRead)
        {
           return context.ChatMessages.Where(x => x.ToWhom__IsSending == receiverName && x.Status == notRead).ToList();
        }

        public ChatMessage InsertMessage(ChatMessage newMessage)
        {
            context.ChatMessages.Add(newMessage);
            context.SaveChanges();
            return newMessage;
        }

        public ChatMessage UpdateMessageByStatus(ChatMessage newMessage)
        {
            var mess = context.ChatMessages.Attach(newMessage);
            mess.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            context.SaveChanges();

            return newMessage;
        }
    }
}
