using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public interface IChatMessageRepository
    {
        List<ChatMessage> GetListOfChatMessageByUnreadMessage(string receiverName, bool notRead);

        ChatMessage InsertMessage(ChatMessage newMessage);

        ChatMessage UpdateMessageByStatus(ChatMessage newMessage);

        IEnumerable<ChatMessage> GetListOfAllMessages();
    }
}
