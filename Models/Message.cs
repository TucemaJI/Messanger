using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Messenger.Data.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
    public enum MessageType
    {
        Text,
        Image,
        File
    }
}
