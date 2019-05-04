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
            if (ModelState.IsValid)
            {
                Broadcast broadcast = new Broadcast("EAAFAza2eNqcBANMlxdcXMVDHZCIIEX20QsW1mVbzrXQTqZC9fdV5dZBES09RYthW8PIcrM5EmKykfSIhytxDxmgUbjewmLwBLFRM7lLXZA5ZCorI6BzdliFhs9m41VWZCZA0D5Ez5ZAYTHCPHZAuTcD6OSZA5mBcZAx56FDD8v389egVgZDZD", "572005723217414");
                tuple = await broadcast.CreateBroadCastAsync(settings);

                if ((int)tuple.Item1 == 200)
                {
                    var message_id = tuple.Item2;

                    var jobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(message_id);
                    var something = Uri.EscapeUriString(jobj["message_creative_id"].ToString());

                    await broadcast.SendBroadCastAsync(something);
                }
                return Redirect("https://www.facebook.com/messages/t/Vegane.KochApp");
            }
            return View("Index");
        }
    }
}
