// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CoreBot;
using CoreBot.Controllers;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private Tuple<int, string> tuple2;
        private Tuple<int, string> tuple3;
        private Tuple<int, string> tuple4;

       

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
            const string welcomeOption2 = "Get%20Started";
            const string podcastOption = "Zum Podcast";
            const string homePageOption = "Zur Homepage";

            var userName = turnContext.Activity.From.Name;
            var PSID = turnContext.Activity.From.Id;
            var responsemessage = "";

            try
            {
                var jobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(turnContext.Activity.Text);
                responsemessage = Uri.EscapeUriString(jobj["__button_text__"].ToString());
            }
            catch
            {
                 responsemessage = turnContext.Activity.Text;
            }


            {
                // Initially the bot offers to showcase 3 Facebook features: Quick replies, PostBack and getting the Facebook Page Name.
                // Below we also show how to get the messaging_optin payload separately as well.
                switch (responsemessage)
                {

                    // By default we offer the users different actions that the bot supports, through quick replies.
                    case welcomeOption:
                       {                                               
                            var reply = turnContext.Activity.CreateReply("Herzlich Willkommen bei Geheime Mentoren.<br/>Ich freue mich sehr das Du hier bist 😊 <br/><br/>Hier im Messenger bist Du ein Teil von Geheime Mentoren.<br/>Ich nehme Dich hinter die Kulissen mit und gebe Dir Bescheid, sobald ich Live gehe, eine neue Podcastfolge online ist und sonstige Neuigkeiten zum verlauten sind.<br/>Du kannst mir hier direkt auch jederzeit schreiben.Ich melde mich so schnell ich kann bei dir 😊<br/><br/>Kennst Du bereits mein 1.Buch ? :) Ich schenke es allen, die Geheime Mentoren folgen. Auf meiner Homepage kannst Du es dir kostenfrei herunterladen. Bin sehr gespannt, wie es Dir gefällt! :)<br/><br/>Freue mich Dich kennen zu lernen. Alles Liebe<br/><br/>Dein Geheimer Mentor & Größter Fan<br/>Christoph");
                            reply.SuggestedActions = new SuggestedActions()
                            {
                                Actions = new List<CardAction>()
                                {
                                    new CardAction() { Title = podcastOption, Type = ActionTypes.PostBack, Value = podcastOption },
                                    new CardAction() { Title = homePageOption, Type = ActionTypes.PostBack, Value = homePageOption },
                                },
                            };
                            await turnContext.SendActivityAsync(reply);
                            await setLabelAsync(PSID, userName);
                            break;
                        }

                    case welcomeOption2:
                        {
                            var reply = turnContext.Activity.CreateReply("Herzlich Willkommen bei Geheime Mentoren.<br/>Ich freue mich sehr das Du hier bist 😊 <br/><br/>Hier im Messenger bist Du ein Teil von Geheime Mentoren.<br/>Ich nehme Dich hinter die Kulissen mit und gebe Dir Bescheid, sobald ich Live gehe, eine neue Podcastfolge online ist und sonstige Neuigkeiten zum verlauten sind.<br/>Du kannst mir hier direkt auch jederzeit schreiben.Ich melde mich so schnell ich kann bei dir 😊<br/><br/>Kennst Du bereits mein 1.Buch ? :) Ich schenke es allen, die Geheime Mentoren folgen. Auf meiner Homepage kannst Du es dir kostenfrei herunterladen. Bin sehr gespannt, wie es Dir gefällt! :)<br/><br/>Freue mich Dich kennen zu lernen. Alles Liebe<br/><br/>Dein Geheimer Mentor & Größter Fan<br/>Christoph");
                            reply.SuggestedActions = new SuggestedActions()
                            {
                                Actions = new List<CardAction>()
                                {
                                    new CardAction() { Title = podcastOption, Type = ActionTypes.PostBack, Value = podcastOption },
                                    new CardAction() { Title = homePageOption, Type = ActionTypes.PostBack, Value = homePageOption },
                                },
                            };
                            await turnContext.SendActivityAsync(reply);
                            await setLabelAsync(PSID, userName);
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
                            var reply = turnContext.Activity.CreateReply($"https://podcast-generationsbilder.podigee.io/");
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

        public async Task setLabelAsync(string PSID, string user)
        {
            PageSettings pageSettings = new PageSettings();

            //Überprüft was das Letzte Label ist
            Label label = new Label();
            tuple2 = await label.getAllLabels();

            if ((int)tuple2.Item1 == 200)
            {
                //Checkt ob es schon ein Label gibt
                var response = tuple2.Item2;
                var labels = (RootObject2)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(RootObject2));

                if (labels.data.Count == 0)
                {
                    //erstellt das 1. Label
                    createLabelAsync(PSID, labels.data.Count.ToString(), user);
                }
                else
                {
                    //bekommt das aktuelle Label (labelData.name = eigene LabelId! -> nicht die von Facebook)
                    var labelData = labels.data[0];
                    tuple3 = await label.getLabelCount(labelData.id);

                    if (tuple3.Item1 == 200)
                    {
                        var counter = tuple3.Item2;

                        Regex rx = new Regex("[0-9]");
                        MatchCollection matches = rx.Matches(counter);
                        string counterString = matches[0].Value;

                        int userCounter = Convert.ToInt32(counterString);

                        //10.000 -> maximale Anzahl User für einen Broadcast
                        if (userCounter < 10000)
                        {
                            await label.MatchLabelWithUser(PSID, labelData.id, pageSettings.token, user);
                        }

                        else
                        {   //erstellt neues Label wenn 10.000 Menschen erreicht sind
                            createLabelAsync(PSID, labels.data.Count.ToString(), user);
                        }
                    }
                }
            }
        }

        //labelName = label.data.Count -> zählt hoch
        public async void createLabelAsync(string PSID, string labelName, string user)
        {
            PageSettings pageSettings = new PageSettings();
            Label label = new Label();
            tuple4 = await label.CreateLabel(pageSettings.token, labelName);
            if ((int)tuple4.Item1 == 200)
            {
                var labelIdJson = tuple4.Item2;
                string labelId = Regex.Replace(labelIdJson, "[^.0-9]*", "").ToString();


                await label.MatchLabelWithUser(PSID, labelId, pageSettings.token, user);
            }
        }
    }
}
