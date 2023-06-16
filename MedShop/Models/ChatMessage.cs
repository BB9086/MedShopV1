using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    [Table("tblChatMessages")]
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public string Who_IsSending { get; set; }
        public string ToWhom__IsSending { get; set; }
        public string MessageBody { get; set; }
        public DateTime DateOfSending { get; set; }
        public bool Status { get; set; }
    }
}
