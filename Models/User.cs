using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Data.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public ICollection<Message> Messages { get; set; }
        public User() { Messages = new List<Message>(); }
    }
}
