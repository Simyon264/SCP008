/*
 * Made by Simyon#6969 
 * ==============
 * Made for DayLight Gaming discord.gg/RxzaN3jGeb
 * 
 * This plugin adds: 
 *      - SCP-008
 * ==================================================================================================================================================================
*/

using System;
using Exiled.API.Interfaces;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using Handlers = Exiled.Events.Handlers;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Random = System.Random;
using System.ComponentModel;

namespace SCP008
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        [Description("The time between damage ticks.")]
        public float TickRate { get; set; } = 2f;
        [Description("The damage 008 victims get every tick")]
        public float DamageAmount { get; set; } = 1.5f;

        [Description("The message when you get revived because of SCP-008")]
        public string SpawnMessage { get; set; } = "You were revieved because of SCP-008";

        [Description("The message when you get infected with SCP-008")]
        public string InfectMessage { get; set; } = "You were infected with SCP-008. Use a medkit or SCP-500 to heal yourself.";

        [Description("The message when you cure SCP-008")]
        public string CureMessage { get; set; } = "You cured SCP-008.";
        [Description("If you can get SCP-008 more then once. You get damage for all the stacks of SCP-008.")]
        public bool Stack008 { get; set; } = false;
    }
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "SCP008";
        public override string Prefix { get; } = "scp008";
        public override string Author { get; } = "Simyon";
        public override Version Version { get; } = new Version(1, 0, 1);
        public override PluginPriority Priority { get; } = PluginPriority.High;

        public List<Player> infectedPlayers = new List<Player>();

        private Random random = new Random();

        private bool IsInChance(int chance)
        {
            if (random == null)
                random = new Random();
            if (random.Next(100) < chance)
            {
                return true;
            }
            return false;
        }

        public override void OnEnabled()
        {
            infectedPlayers.Clear();
            Handlers.Player.Hurting += Hurting;
            Handlers.Player.UsedItem += UsingItem;
            Handlers.Player.Dying += Dying;
            Handlers.Server.RoundStarted += OnRoundStart;

            if (Round.IsStarted)
            {
                Timing.RunCoroutine(EventLoop(), "SCP008MainLoop");
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Handlers.Player.Hurting -= Hurting;
            Handlers.Player.UsedItem -= UsingItem;
            Handlers.Player.Dying -= Dying;
            Handlers.Server.RoundStarted -= OnRoundStart;

            Timing.KillCoroutines("SCP008MainLoop");

            base.OnDisabled();
        }

        public void OnRoundStart()
        {
            Timing.RunCoroutine(EventLoop(), "SCP008MainLoop");
        }

        public IEnumerator<float> EventLoop()
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(Config.TickRate);
                //Log.Info($"SCP008 Tick");
                foreach (Player player in infectedPlayers.ToArray())
                {
                    //Log.Info($"Damage tick for: {player.Nickname}");
                    if (player.Role.Team != Team.SCP)
                    {
                        player.Hurt(Config.DamageAmount, DamageType.Poison);
                    } else
                    {
                        infectedPlayers.Remove(player);
                    }
                }
            }
        }

        public void Heal008(Player player)
        {
            if (infectedPlayers.Contains(player))
            {
                //Log.Info($"Healed {player.Nickname}");
                infectedPlayers.Remove(player);
            }
        }

        public void Infect(Player player)
        {
            //Log.Info($"Infected {player.Nickname}");
            infectedPlayers.Add(player);
        }

        public void UsingItem(UsedItemEventArgs ev)
        {
            if (infectedPlayers.Contains(ev.Player))
            {
                if (ev.Item.Type == ItemType.SCP500)
                {
                    Heal008(ev.Player);
                    ev.Player.Broadcast(5, Config.CureMessage);
                }
                if (ev.Item.Type == ItemType.Medkit)
                {
                    if (IsInChance(50))
                    {
                        ev.Player.Broadcast(5, Config.CureMessage);
                        Heal008(ev.Player);
                    }
                }
            }
        }

        public void Dying(DyingEventArgs ev)
        {
            if (infectedPlayers.Contains(ev.Target))
            {
                ev.IsAllowed = false;
                Vector3 position = ev.Target.Position;
                ev.Target.DropItems();
                ev.Target.SetRole(RoleType.Scp0492, SpawnReason.Revived);
                Timing.CallDelayed(1f, () =>
                {
                    ev.Target.Teleport(position);
                    ev.Target.Broadcast(5, Config.SpawnMessage);
                    Heal008(ev.Target);
                });
            }
        }

        public void Hurting(HurtingEventArgs ev)
        {
            if (ev.Handler.Type == DamageType.Poison) return;
            if (ev.Attacker == null) return;
            if (ev.Attacker.Role == RoleType.Scp0492)
            {
                if (ev.Target.Role.Team != Team.SCP)
                {
                    if (infectedPlayers.Contains(ev.Target) && Config.Stack008 == false) return;
                    Infect(ev.Target);
                    ev.Target.Broadcast(5, Config.InfectMessage);
                }
            }
        }
    }
}