using CoreBot.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.Controllers
{
    public class Label
    {
        private readonly string _facebookAPI = "https://graph.facebook.com/v2.11";
        private readonly string _labelCreateEndpoint = "custom_labels";
        private readonly string _labelMatchEndpoint = "label";

        /*
        public async Task<Tuple<int, string>> checkIfLabelexists(string accessToken)
        {
            string _labelCreate = $"{_facebookAPI}/me/{_labelCreateEndpoint}?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                LabelModel labelModel = new LabelModel
                {
                    name = "test12"
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(labelModel);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync(_labelCreate, content).Result;
                var httpContent = await result.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)result.StatusCode,
                    httpContent
                    );
            }
        }

    */

        public async Task<Tuple<int, string>> getAllLabels()
        {
            string _getUserLabel = "https://graph.facebook.com/v2.11/me/custom_labels?fields=name&access_token=EAAFAza2eNqcBANMlxdcXMVDHZCIIEX20QsW1mVbzrXQTqZC9fdV5dZBES09RYthW8PIcrM5EmKykfSIhytxDxmgUbjewmLwBLFRM7lLXZA5ZCorI6BzdliFhs9m41VWZCZA0D5Ez5ZAYTHCPHZAuTcD6OSZA5mBcZAx56FDD8v389egVgZDZD";

            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage response = await httpClient.GetAsync(_getUserLabel);
                response.EnsureSuccessStatusCode();

                string httpContent = await response.Content.ReadAsStringAsync();



                return new Tuple<int, string>(
                    (int)response.StatusCode,
                    httpContent
                    );
            }
        }

        public async Task<Tuple<int, string>> getLabelCount(string labelId)
        {
            string api = "https://tommotmotbotapi.herokuapp.com/public/countbyLabel";

            string _getLabelCount = $"{api}/{labelId}";

            using (var httpClient = new HttpClient())
            {
                var username = "tommotmot";
                var password = "csYvyWRGo82*^32i";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                HttpResponseMessage response = await httpClient.GetAsync(_getLabelCount);
                response.EnsureSuccessStatusCode();

                string httpContent = await response.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)response.StatusCode,
                    httpContent
                    );
            }
        }


        public async Task<Tuple<int, string>> createUserLabelDB(string labelid, string PSID, string user)
        {
            string _getUserLabel = "https://tommotmotbotapi.herokuapp.com/public/createuser";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var username = "tommotmot";
                var password = "csYvyWRGo82*^32i";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                UserDBModel userDBModel = new UserDBModel()
                {
                    labelid = labelid,
                    psid = PSID,
                    username = user,
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(userDBModel);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync(_getUserLabel, content).Result;
                var httpContent = await result.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)result.StatusCode,
                    httpContent
                    );
            }
        }

        public async Task<Tuple<int, string>> CreateLabel(string accessToken, string labelid)
        {
            string _labelCreate = $"{_facebookAPI}/me/{_labelCreateEndpoint}?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                LabelModel labelModel = new LabelModel
                {
                    name = labelid
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(labelModel);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync(_labelCreate, content).Result;
                var httpContent = await result.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)result.StatusCode,
                    httpContent
                    );
            }
        }

        public async Task<Tuple<int, string>> MatchLabelWithUser(string PSID, string labelIdJson, string accessToken, string user)
        {
            var jobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(labelIdJson);
            var labelId= Uri.EscapeUriString(jobj["id"].ToString());

            string _labelMatch = $"{_facebookAPI}/{labelId}/{_labelMatchEndpoint}?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                LabelModel labelModel = new LabelModel
                {
                    user = PSID
                };

                await createUserLabelDB(labelId, PSID, user);


                var json = Newtonsoft.Json.JsonConvert.SerializeObject(labelModel);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync(_labelMatch, content).Result;
                var httpContent = await result.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)result.StatusCode,
                    httpContent
                    );
            }
        }
    }
}
