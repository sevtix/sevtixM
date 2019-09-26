using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using CitizenFX.Core.Native;

namespace sevtixM
{
    class sevtixM : BaseScript
    {
        public sevtixM() {
            Tick += OnTick;

            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;

        }

        public async Task OnTick()
        {
            if (Game.IsControlJustPressed(0, Control.SaveReplayClip))
            {
                if (!MenuController.IsAnyMenuOpen())
                {
                    open_sevtixM_menu();
                }
            }
        }

        public void open_sevtixM_menu()
        {
            Menu hauptmenu = new Menu("sevtixM", "Hauptmenu") { Visible = true };
            MenuController.AddMenu(hauptmenu);

            MenuItem fahrzeugeButton = new MenuItem("Fahrzeuge");
            MenuItem spielerButton = new MenuItem("Spieler");

            hauptmenu.AddMenuItem(fahrzeugeButton);
            hauptmenu.AddMenuItem(spielerButton);
            hauptmenu.AddMenuItem(new MenuItem("Schliessen"));
            hauptmenu.OnItemSelect += (_menu, _item, _index) =>
            {
                int index = _index;
                switch (index)
                {
                    case 2:
                        // OPTIONEN
                        hauptmenu.CloseMenu();
                        break;
                }
            };

            // ---------------------------------------------------------------------

            Menu fahrzeuge = new Menu("sevtixM", "Fahrzeuge") { Visible = false };
            MenuController.AddSubmenu(hauptmenu, fahrzeuge);
            MenuItem upgradesButton = new MenuItem("Upgrades", "");
            MenuItem reparierenButton = new MenuItem("Reparieren", "");
            MenuItem customFahrzeugeButton = new MenuItem("Custom Fahrzeuge", "");   

            if(!IsPedInVehicle())
            {
                reparierenButton.Enabled = false;
                upgradesButton.Enabled = false;
            } else
            {
                reparierenButton.Enabled = true;
                upgradesButton.Enabled = true;
            }

            fahrzeuge.AddMenuItem(reparierenButton);
            fahrzeuge.AddMenuItem(upgradesButton);
            fahrzeuge.AddMenuItem(customFahrzeugeButton);
            fahrzeuge.OnItemSelect += (_menu, _item, _index) =>
            {
                int index = _index;
                switch (index)
                {
                    case 0:
                        if(Game.PlayerPed.IsInVehicle())
                        {
                            Vehicle veh = Game.PlayerPed.CurrentVehicle;
                            veh.Repair();
                        }  else
                        {
                            SendMessageNoVehicleMessage();
                        }
                        break;
                }
            };
            // ---------------------------------------------------------------------

            // ---------------------------------------------------------------------
            Menu spieler = new Menu("sevtixM", "Spieler") { Visible = false };
            MenuController.AddSubmenu(hauptmenu, spieler);
            spieler.AddMenuItem(new MenuItem("Heilen"));
            spieler.AddMenuItem(new MenuItem("Fahndungslevel zurücksetzen"));
            MenuItem godmodeButton = new MenuItem("Godmode");
            spieler.AddMenuItem(godmodeButton);
            spieler.OnItemSelect += (_menu, _item, _index) =>
            {
                int index = _index;
                switch (index)
                {
                    case 0:
                        // Heilen
                        Game.PlayerPed.Health = Game.PlayerPed.MaxHealth;
                        SendMessage("sevtixM - Freeroam", "Du wurdest geheilt", 0, 255, 0);
                        break;
                    case 1:
                        // Fahndung entfernen
                        Game.Player.WantedLevel = 0;
                        SendMessage("sevtixM - Freeroam", "Dein Fahndungslevel wurde zurückgesetzt", 0, 255, 0);
                        break;
                }
            };

            // ---------------------------------------------------------------------
            Ped ped = Game.PlayerPed;
            Menu godmode = new Menu("sevtixM", "Godmode") { Visible = false };
            MenuController.AddSubmenu(spieler, godmode);
            godmode.AddMenuItem(new MenuItem("An"));
            godmode.AddMenuItem(new MenuItem("Aus"));
            godmode.OnItemSelect += (_menu, _item, _index) =>
            {
                int index = _index;
                switch (index)
                {
                    case 0:
                        // Heilen
                        API.SetCurrentPedWeapon(ped.Handle, 2725352035, true);
                        API.SetEntityInvincible(ped.Handle, true);
                        //API.SetEnableHandcuffs(ped.Handle, true);
                        API.SetPedCanSwitchWeapon(ped.Handle, false);
                        API.SetPoliceIgnorePlayer(Game.Player.Handle, true);
                        SendMessage("sevtixM - Freeroam", "Du bist nun Unverwundbar", 0, 255, 0);
                        break;
                    case 1:
                        API.SetEntityInvincible(ped.Handle, false);
                        //API.SetEnableHandcuffs(ped.Handle, false);
                        API.SetPedCanSwitchWeapon(ped.Handle, true);
                        API.SetPoliceIgnorePlayer(Game.Player.Handle, false);
                        SendMessage("sevtixM - Freeroam", "Du bist nicht mehr Unverwundbar", 255, 0, 0);
                        break;
                }
            };
            // ---------------------------------------------------------------------

            Menu upgradesMenu = new Menu("sevtixM", "Upgrades") { Visible = false };
            MenuController.AddSubmenu(fahrzeuge, upgradesMenu);

            List<string> engineUpgrades = new List<string>() { "Stock", "Level 1", "Level 2", "Level 3", "Level 4" };
            MenuListItem engineItem = new MenuListItem("Motor", engineUpgrades, GetModIndexOfVehicleOr0(11));
            upgradesMenu.AddMenuItem(engineItem);

            List<string> exhaustUpgrades = new List<string>() { "Stock", "Level 1", "Level 2", "Level 3", "Level 4" };
            MenuListItem exhaust = new MenuListItem("Auspuff", exhaustUpgrades, GetModIndexOfVehicleOr0(4));
            upgradesMenu.AddMenuItem(exhaust);

            upgradesMenu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
            {

                int itemIndex = _itemIndex;
                int listIndex = _listIndex;

                switch (itemIndex)
                {
                    case 0:
                        // ENGINE
                        if (Game.PlayerPed.IsInVehicle())
                        {
                            Vehicle veh = Game.PlayerPed.CurrentVehicle;
                            API.SetVehicleMod(veh.Handle, 11, listIndex - 1, false);
                        } else
                        {
                            SendMessageNoVehicleMessage();
                        }
                        break;

                    case 4:
                        // EXHAUST
                        if (Game.PlayerPed.IsInVehicle())
                        {
                            Vehicle veh = Game.PlayerPed.CurrentVehicle;
                            API.SetVehicleMod(veh.Handle, 4, listIndex - 1, false);
                        } else
                        {
                            SendMessageNoVehicleMessage();
                        }
                        break;
                }

            };


            // ---------------------------------------------------------------------

            Menu customfahrzeuge = new Menu("sevtixM", "Custom Fahrzeuge") { Visible = false };
            MenuController.AddSubmenu(fahrzeuge, customfahrzeuge);
            customfahrzeuge.AddMenuItem(new MenuItem("KTM SX-F 450 Supermotard", ""));
            customfahrzeuge.AddMenuItem(new MenuItem("KTM EXC 530 Supermotard", ""));
            customfahrzeuge.AddMenuItem(new MenuItem("Toyota Supra", ""));
            customfahrzeuge.AddMenuItem(new MenuItem("Nissan Skyline GTR R34", ""));
            customfahrzeuge.AddMenuItem(new MenuItem("Nissan GTR R35", ""));

            customfahrzeuge.OnItemSelect += async (_menu, _item, _index) =>
            {
                int index = _index;

                Vehicle spawned = null;

                switch (index)
                {

                    case 0:
                        spawned = await World.CreateVehicle(API.GetHashKey("sxf450sm"), Game.PlayerPed.Position);
                        MenuController.CloseAllMenus();
                        break;
                    case 1:
                        spawned = await World.CreateVehicle(API.GetHashKey("exc530sm"), Game.PlayerPed.Position);
                        MenuController.CloseAllMenus();
                        break;
                    case 2:
                        spawned = await World.CreateVehicle(API.GetHashKey("supra2"), Game.PlayerPed.Position);
                        MenuController.CloseAllMenus();
                        break;
                    case 3:
                        spawned = await World.CreateVehicle(API.GetHashKey("skyline"), Game.PlayerPed.Position);
                        MenuController.CloseAllMenus();
                        break;
                    case 4:
                        spawned = await World.CreateVehicle(API.GetHashKey("gtr"), Game.PlayerPed.Position);
                        MenuController.CloseAllMenus();
                        break;
                }

                spawned.NeedsToBeHotwired = false;
                API.SetPedIntoVehicle(Game.PlayerPed.Handle, spawned.Handle, -1);
            };

            // ---------------------------------------------------------------------

            MenuController.BindMenuItem(hauptmenu, fahrzeuge, fahrzeugeButton);
            MenuController.BindMenuItem(hauptmenu, spieler, spielerButton);

            MenuController.BindMenuItem(fahrzeuge, customfahrzeuge, customFahrzeugeButton);
            MenuController.BindMenuItem(fahrzeuge, upgradesMenu, upgradesButton);

            MenuController.BindMenuItem(spieler, godmode, godmodeButton);
        }

        public int GetModIndexOfVehicleOr0(int mod)
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                return API.GetVehicleMod(Game.PlayerPed.CurrentVehicle.Handle, mod) + 1;
            }
            else
            {
                return 0;
            }
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

        public void SendMessageNoVehicleMessage()
        {
            SendMessage("sevtixM - Freeroam", "Du bist in keinem Fahrzeug", 255, 0, 0);
        }

        public bool IsPedInVehicle()
        {
            return Game.PlayerPed.IsInVehicle();
        }
    }
}
