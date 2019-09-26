using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sevtixM
{
    class Help : BaseScript
    {
        public Help()
        {
            API.RegisterCommand("help", new Action<int, List<object>, string>((source, args, raw) =>
            {
                SendMessage("sevtixM - Hilfe", "F3 - Öffne Beta Menu", 0, 255, 255);
                SendMessage("sevtixM - Hilfe", "/vehicle - Spawnt ein Fahrzeug", 0, 255, 255);
                SendMessage("sevtixM - Hilfe", "/heal - Heilt dich", 0, 255, 255);
                SendMessage("sevtixM - Hilfe", "/repair - Repariert das aktuelle Fahrzeug", 0, 255, 255);
                SendMessage("sevtixM - Hilfe", "/godmode <on/off> - (De)Aktiviert den Godmode", 0, 255, 255);
                SendMessage("sevtixM - Hilfe", "/wanted <level> - Setzt die Fahndungsstufe", 0, 255, 255);
            }), false);
        }

        public void SendMessage(string title, string message, int r, int g, int b)
        {
            title = " " + title;
            var msg = new Dictionary<string, object>
            {
                ["color"] = new[] { r, g, b },
                ["args"] = new[] { title, message }
            };
            TriggerEvent("chat:addMessage", msg);
        }
    }
}
