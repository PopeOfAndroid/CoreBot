using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot.Models
{
    public class CreateBroadCastModel
    {
        public class Button
        {
            public string type { get; set; }
            public string url { get; set; }
            public string title { get; set; }
        }

        public class Payload
        {
            public string template_type { get; set; }
            public string text { get; set; }
            public List<Button> buttons { get; set; }
        }

        public class Attachment
        {
            public string type { get; set; }
            public Payload payload { get; set; }
        }

        public class Message
        {
            public Attachment attachment { get; set; }
            public DynamicText dynamic_text { get; set; }
        }

        public class RootObject
        {
            public List<Message> messages { get; set; }

        }

        public class DynamicText
        {
            public string text { get; set; }
            public string fallback_text { get; set; }
        }
    }
}
