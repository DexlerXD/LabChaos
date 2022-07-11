using LabChaos.Events;
using System;
using Exiled.API.Features;
using ServerEvent = Exiled.Events.Handlers.Server;

namespace LabChaos
{
    public class LabChaos : Plugin<Config>
    {
        private ServerHandler serverHandler;
        public static LabChaos Instance { get; set; }

        public override void OnEnabled()
        {
            RegisterEvents();
            Instance = this;
            Log.Info("<color=green>LabChaos has succesfully been loaded!</color>");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            serverHandler = new ServerHandler();

            ServerEvent.RoundStarted += serverHandler.OnRoundStarted;
        }

        private void UnregisterEvents()
        {
            ServerEvent.RoundStarted -= serverHandler.OnRoundStarted;

            serverHandler = null;
        }

        public override string Name => "LabChaos";
        public override string Author => "Dexler";
        public override Version Version => new Version(1, 0, 0);
    }
}
