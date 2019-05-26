using CoreBot.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.Controllers
{
    public class SettingsController : Controller
    {
        private Tuple<int, string> tuple;
        private Tuple<int, string> tuple2;

        [HttpGet]
        public IActionResult Index()
        {
            SettingsViewModel settings = new SettingsViewModel();
            settings.Message = "Test";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BroadcastAsync(SettingsViewModel settings)
        {
            PageSettings pageSettings = new PageSettings();

            if (ModelState.IsValid)
            {
                //check welche label es gibt
                Label label = new Label();
                tuple2 = await label.getAllLabels();

                if ((int)tuple2.Item1 == 200)
                {
                    //Checkt ob es schon ein Label gibt
                    var response = tuple2.Item2;
                    var labels = (RootObject2)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(RootObject2));

                    for (int i = 0; i < labels.data.Count; i++)
                    {
                        Broadcast broadcast = new Broadcast(pageSettings.token, pageSettings.PageId);
                        tuple = await broadcast.CreateBroadCastAsync(settings);

                        if ((int)tuple.Item1 == 200)
                        {
                            var message_id = tuple.Item2;

                            var jobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(message_id);
                            var something = Uri.EscapeUriString(jobj["message_creative_id"].ToString());

                            await broadcast.SendBroadCastAsync(something, labels.data[i].id);
                        }
                    }
                }
                return Redirect("https://www.facebook.com/");
            }
            return View("Index");
        }
    }
}
