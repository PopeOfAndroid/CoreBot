using CoreBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static CoreBot.Models.CreateBroadCastModel;

namespace CoreBot.Controllers
{
    public class Broadcast
    {
        private readonly string _pageID;
        private readonly string _facebookAPI = "https://graph.facebook.com/v3.2/";
        private readonly string _broadCastCreateEndpoint = "message_creatives";
        private readonly string _broadCastSendEndpoint = "broadcast_messages";
        private readonly string _broadCastUrlCreate;
        private readonly string _broadCastUrlSend;

        public Broadcast(string accessToken, string pageID)
        {
            _pageID = pageID;
            _broadCastUrlCreate = $"{_facebookAPI}{pageID}/{_broadCastCreateEndpoint}?access_token={accessToken}";
            _broadCastUrlSend = $"{_facebookAPI}{pageID}/{_broadCastSendEndpoint}?access_token={accessToken}";
        }

        public async Task<Tuple<int, string>> CreateBroadCastAsync(string broadCastText)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = LiveStreamSerializer(broadCastText).ToString();

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync(_broadCastUrlCreate, content).Result;
                var httpContent = await result.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)result.StatusCode,
                    httpContent
                    );
            }
        }        

        public async Task<Tuple<int, string>> SendBroadCastAsync(string message_create_id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                SendBroadcastModel broadcastModel = new SendBroadcastModel
                {
                    message_creative_id = message_create_id,
                    notification_type = "REGULAR",
                    messaging_type = "MESSAGE_TAG",
                    tag = "NON_PROMOTIONAL_SUBSCRIPTION",
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(broadcastModel);

                var contentSend = new StringContent(json, Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync(_broadCastUrlSend, contentSend).Result;
                var httpContent = await result.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)result.StatusCode,
                    httpContent);
            }
        }

        private string LiveStreamSerializer(string broadCastText)

        {
            List<Button> buttons = new List<Button>()
            {
                new Button()
                {

                    type = "web_url",
                    url = "https://www.facebook.com/",
                    title = "YES! Auf jeden Fall"
                }
            };
        
            Payload payload = new Payload
            {
                template_type = "button",
                text = broadCastText,
                buttons = buttons
                
            };

            Attachment attachment = new Attachment
            {
                type = "template",
                payload = payload
            };

        List<Message> messages = new List<Message>()
            {
                        new Message()
                        {
                            attachment = attachment
                        }
            };
                    

            RootObject rootObject = new RootObject
            {
                messages = messages
            };

            return JsonConvert.SerializeObject(rootObject);
        }

        public string StandardSerializer(string broadCastText)

        {
            DynamicText dynamicTexts = new DynamicText();
            dynamicTexts.text = broadCastText;

            List<Message> messages = new List<Message>()
                    {
                        new Message()
                        {
                            dynamic_text = dynamicTexts
                        }
                    };

            RootObject rootObject = new RootObject
            {
                messages = messages
            };

            return JsonConvert.SerializeObject(rootObject);
        }
    }
}
