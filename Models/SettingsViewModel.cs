using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot.Models
{
    public class SettingsViewModel
    {
        [Required(ErrorMessage = "Bitte gebe eine Nachricht ein!")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Bitte gebe ein Call-To-Action ein!")]
        public string ButtonText { get; set; }
        public string Url { get; set; }
    }
}
