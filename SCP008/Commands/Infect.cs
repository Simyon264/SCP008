using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using System.Text;

namespace SCP008.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Infect : ICommand
    {
        public string Command { get; } = "scp008_infect";
        public string[] Aliases { get; } = new[] { "infect" };
        public string Description { get; } = "Infect someone.";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("scp008.infect"))
            {
                if (arguments.Count == 0)
                {
                    Player target = Player.Get(((CommandSender)sender).SenderId);
                    Plugin.StaticInstance.Infect(target);
                    response = $"Infected {target.Nickname}";
                    target.Broadcast(5, Plugin.StaticInstance.Config.InfectMessage);
                    return true;
                }
                else
                {
                    if (arguments.At(0) == "*")
                    {
                        foreach (Player ply in Player.List)
                        {
                            Plugin.StaticInstance.Infect(ply);
                            ply.Broadcast(5, Plugin.StaticInstance.Config.InfectMessage);
                        }
                        response = "Infected everyone.";
                        return true;
                    }
                    Player target = Player.Get(arguments.At(0));
                    if (target != null)
                    {
                        Plugin.StaticInstance.Infect(target);
                        response = $"Infected {target.Nickname}";
                        target.Broadcast(5, Plugin.StaticInstance.Config.InfectMessage);
                        return true;
                    }
                    else
                    {
                        response = "Target not found.";
                        return false;
                    }
                }
            } else
            {
                response = "No Permissions";
                return false;
            }
        }
    }
}
