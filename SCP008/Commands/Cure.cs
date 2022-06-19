using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using System.Text;

namespace SCP008.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Cure : ICommand
    {
        public string Command { get; } = "scp008_cure";
        public string[] Aliases { get; } = new[] { "cure" };
        public string Description { get; } = "Cure someone.";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("scp008.cure"))
            {
                if (arguments.Count == 0)
                {
                    Player target = Player.Get(((CommandSender)sender).SenderId);
                    Plugin.StaticInstance.Heal008(target);
                    response = $"Cured {target.Nickname}";
                    target.Broadcast(5, Plugin.StaticInstance.Config.CureMessage);
                    return true;
                }
                else
                {
                    if (arguments.At(0) == "*")
                    {
                        foreach (Player ply in Player.List)
                        {
                            Plugin.StaticInstance.Heal008(ply);
                            ply.Broadcast(5, Plugin.StaticInstance.Config.CureMessage);
                        }
                        response = "Cured everyone.";
                        return true;
                    }
                    Player target = Player.Get(arguments.At(0));
                    if (target != null)
                    {
                        Plugin.StaticInstance.Heal008(target);
                        response = $"Cured {target.Nickname}";
                        target.Broadcast(5, Plugin.StaticInstance.Config.CureMessage);
                        return true;
                    }
                    else
                    {
                        response = "Target not found.";
                        return false;
                    }
                }
            }
            else
            {
                response = "No Permissions";
                return false;
            }
        }
    }
}
