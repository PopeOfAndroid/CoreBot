// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CoreBot;
using CoreBot.Controllers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples
{
    // This IBot implementation can run any type of Dialog. The use of type parameterization is to allows multiple different bots
    // to be run at different endpoints within the same project. This can be achieved by defining distinct Controller types
    // each with dependency on distinct IBot types, this way ASP Dependency Injection can glue everything together without ambiguity.
    // The ConversationState is used by the Dialog system. The UserState isn't, however, it might have been used in a Dialog implementation,
    // and the requirement is that all BotState objects are saved at the end of a turn.
    public class DialogBot<T> : ActivityHandler where T : Dialog 
    {
        /// <summary>
        /// The <see cref="DialogSet"/> that contains all the Dialogs that can be used at runtime.
        /// </summary>
        
        //new bot
        private readonly DialogSet _dialogs;
        private const string DialogId = "question";

        //oldbot
        protected readonly Dialog _dialog;
        protected readonly BotState _conversationState;
        protected readonly BotState _userState;
        protected readonly ILogger _logger;
        private Tuple<int, string> tuple;

        public DialogBot(ConversationState conversationState, UserState userState, T dialog, ILogger<DialogBot<T>> logger)
        {    
            //old bot
            _conversationState = conversationState;
            _userState = userState;
            _dialog = dialog;
            _logger = logger;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            const string welcomeOption = "Los Gehts";
            const string podcastOption = "Zum Podcast";
            const string homePageOption = "Zur Homepage";

            //get responseMessage from User
            var responseMessage = turnContext.Activity.Text;

            {
                var senderId = turnContext.Activity.ChannelData.ToString();
                Regex rx = new Regex("[0-9]{16}");

                MatchCollection matches = rx.Matches(senderId);
                string PSID = matches[0].Value;

                Label label = new Label();
                tuple = await label.CreateLabel("EAAFAza2eNqcBANMlxdcXMVDHZCIIEX20QsW1mVbzrXQTqZC9fdV5dZBES09RYthW8PIcrM5EmKykfSIhytxDxmgUbjewmLwBLFRM7lLXZA5ZCorI6BzdliFhs9m41VWZCZA0D5Ez5ZAYTHCPHZAuTcD6OSZA5mBcZAx56FDD8v389egVgZDZD");

                if ((int)tuple.Item1 == 200)
                {
                    var label_id = tuple.Item2;

                    await label.MatchLabelWithUser(PSID, label_id, "EAAFAza2eNqcBANMlxdcXMVDHZCIIEX20QsW1mVbzrXQTqZC9fdV5dZBES09RYthW8PIcrM5EmKykfSIhytxDxmgUbjewmLwBLFRM7lLXZA5ZCorI6BzdliFhs9m41VWZCZA0D5Ez5ZAYTHCPHZAuTcD6OSZA5mBcZAx56FDD8v389egVgZDZD");
                }


                // Initially the bot offers to showcase 3 Facebook features: Quick replies, PostBack and getting the Facebook Page Name.
                // Below we also show how to get the messaging_optin payload separately as well.
                switch (turnContext.Activity.Text)
                {
                    // By default we offer the users different actions that the bot supports, through quick replies.
                    case welcomeOption:
                        {
                            var reply = turnContext.Activity.CreateReply("Herzlich Willkommen bei Geheime Mentoren");
                            reply.SuggestedActions = new SuggestedActions()
                            {
                                Actions = new List<CardAction>()
                                {
                                    new CardAction() { Title = podcastOption, Type = ActionTypes.PostBack, Value = podcastOption },
                                    new CardAction() { Title = homePageOption, Type = ActionTypes.PostBack, Value = homePageOption },
                                },
                            };
                            await turnContext.SendActivityAsync(reply);
                            break;
                        }

                    case homePageOption:
                        {
                            var reply = turnContext.Activity.CreateReply($"https://geheime-mentoren.de/");
                            await turnContext.SendActivityAsync(reply);
                            break;
                        }

                    case podcastOption:
                        {
                            var reply = turnContext.Activity.CreateReply($"https://podcast-generationsbilder.podigee.io/5-neue-episode");
                            await turnContext.SendActivityAsync(reply);
                            break;
                        }
                }
            }

            // Save any state changes that might have occured during the turn.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running dialog with Message Activity.");

            // Run the Dialog with the new message Activity.
            await _dialog.Run(turnContext, _conversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
        }
    }
}
