using CommandSystem;
using System;
using LabChaos.Methods;

namespace LabChaos.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ChaosEvent : ICommand
    {
        public string Command { get; } = "ChaosEvent";

        public string[] Aliases { get; } = new[] { "ce" };

        public string Description { get; } = "Fires ChaosEvent with specified ID";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Bad arguments! usage: ChaosEvent/ce <id>";
                return false;
            }

            ChaosEvents chaosEv = new ChaosEvents();
            chaosEv.events[Convert.ToInt32(arguments.At(0))].Invoke();

            response = "Event fired successfully!";
            return true;
        }
    }
}
