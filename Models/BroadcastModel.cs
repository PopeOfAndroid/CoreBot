using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot.Models
{
    public class SendBroadcastModel
    {
        public string message_creative_id { get; set; }

        public string custom_label_id { get; set; }

        public string notification_type { get; set; }

        public string messaging_type { get; set; }

        public string tag { get; set; }
    }
}
