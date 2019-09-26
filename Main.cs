using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;

namespace sevtixM
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["playerSpawned"] += new Action(OnPlayerJoin);

            // COMMAND EXAMPLE
            /*RegisterCommand("command", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                SendMessage("csharp", "Test Message", 0, 255, 0);
            }), false);*/

            API.RegisterCommand("godmode", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if(args.Count == 1)
                {
                    Ped ped = Game.PlayerPed;

                    switch (args[0])
                    {
                        case "on":
                            API.SetCurrentPedWeapon(ped.Handle, 2725352035, true);
                            API.SetEntityInvincible(ped.Handle, true);
                            //API.SetEnableHandcuffs(ped.Handle, true);
                            API.SetPedCanSwitchWeapon(ped.Handle, false);
                            API.SetPoliceIgnorePlayer(Game.Player.Handle, true);

                            SendMessage("sevtixM - Freeroam" , "Du bist nun Unverwundbar" , 0, 255,0);
                            break;

                        case "off":
                            API.SetEntityInvincible(ped.Handle, false);
                            //API.SetEnableHandcuffs(ped.Handle, false);
                            API.SetPedCanSwitchWeapon(ped.Handle, true);
                            API.SetPoliceIgnorePlayer(Game.Player.Handle, false);
                            SendMessage("sevtixM - Freeroam", "Du bist nicht mehr Unverwundbar", 255, 0, 0);
                            break;
                    }
                } else
                {
                    SendMessage("sevtixM - Freeroam", "/godmode <on/off>", 255, 127, 0);
                }
            }), false);

            API.RegisterCommand("vehicle", new Action<int, List<object>, string>(async(source, args, raw) =>
            {
                Ped ped = Game.PlayerPed;
                if (args.Count == 1)
                {
                    Model model = new Model(API.GetHashKey((string)args[0]));
                    Vehicle veh = await World.CreateVehicle(model, Game.PlayerPed.Position);
                    veh.NeedsToBeHotwired = false;
                    API.SetPedIntoVehicle(ped.Handle, veh.Handle, -1);
                    SendMessage("sevtixM - Freeroam", (string)args[0]+" wurde gespawnt", 0, 255, 0);
                } else
                {
                    SendMessage("sevtixM - Freeroam", "/vehicle <fahrzeugname>", 255, 127, 0);
                }
            }), false);

            API.RegisterCommand("heal", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Ped ped = Game.PlayerPed;
                ped.Health = ped.MaxHealth;
            }), false);

            API.RegisterCommand("repair", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Ped ped = Game.PlayerPed;
                if (Game.PlayerPed.IsInVehicle())
                {
                    Vehicle veh = Game.PlayerPed.CurrentVehicle;
                    veh.Repair();
                }
            }), false);

            API.RegisterCommand("windows", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Ped ped = Game.PlayerPed;
                if (args.Count == 1)
                {

                    if(Game.PlayerPed.IsInVehicle())
                    {
                        Vehicle veh = Game.PlayerPed.CurrentVehicle;


                        if ((string)args[0] == "down")
                        {
                            API.RollDownWindows(veh.Handle);
                        }

                        if ((string)args[0] == "up")
                        {
                            API.RollUpWindow(veh.Handle, 0);
                            API.RollUpWindow(veh.Handle, 1);
                            API.RollUpWindow(veh.Handle, 2);
                            API.RollUpWindow(veh.Handle, 3);
                        }
                    }
                    
                }
                else
                {
                    SendMessage("sevtixM - Freeroam", "/vehicle <fahrzeugname>", 255, 127, 0);
                }
            }), false);

            API.RegisterCommand("wanted", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                if (args.Count == 1)
                {
                    Game.Player.WantedLevel = Int32.Parse((string)args[0]);
                }
                else
                {
                    SendMessage("sevtixM - Freeroam", "/wanted <level>", 255, 127, 0);
                }
            }), false);

        }

        public void OnPlayerJoin()
        {
            foreach(WeaponHash weapon in Enum.GetValues(typeof(WeaponHash))) {
                if(!Game.PlayerPed.Weapons.HasWeapon(weapon))
                {
                    Game.PlayerPed.Weapons.Give(weapon, 999, false, false);
                }
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
    }
}
