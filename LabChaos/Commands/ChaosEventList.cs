using CommandSystem;
using LabChaos.Methods;
using System;

namespace LabChaos.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ChaosEventList : ICommand
    {
        public string Command { get; } = "ChaosEventList";

        public string[] Aliases { get; } = new[] { "celist", "cels" };

        public string Description { get; } = "Displays list of all events with their respective IDs";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count > 0)
            {
                response = "This command takes no arguments!";
                return false;
            }

            response = "";
            ChaosEvents chaosEv = new ChaosEvents();

            for (int i = 0; i < chaosEv.events.Count; i++)
            {
                response += "\n" + chaosEv.events[i].Method.Name + " | id: " + i.ToString();
            }

            return true;
        }

    }
}
