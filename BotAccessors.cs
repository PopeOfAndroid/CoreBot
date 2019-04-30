using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot
{
    public class BotAccessors
    {
        // Conversation state is of type DialogState. Under the covers this is a serialized dialog stack.
        public IStatePropertyAccessor<DialogState> ConversationDialogState { get; set; }
    }
}
