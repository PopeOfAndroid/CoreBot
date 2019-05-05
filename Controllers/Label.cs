using CoreBot.Models;
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

        public async Task<Tuple<int, string>> CreateLabel(string accessToken)
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

        public async Task<Tuple<int, string>> MatchLabelWithUser(string PSID, string labelId, string accessToken)
        {
            string _labelMatch = $"{_facebookAPI}/{labelId}/{_labelMatchEndpoint}?access_token={accessToken}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                LabelModel labelModel = new LabelModel
                {
                    user = PSID
                };

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
