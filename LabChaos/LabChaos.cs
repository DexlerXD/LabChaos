using LabChaos.Events;
using System;
using Exiled.API.Features;
using ServerEvent = Exiled.Events.Handlers.Server;
using Exiled.API.Enums;

namespace LabChaos
{
    public class LabChaos : Plugin<Config>
    {
        private ServerHandler serverHandler;
        public static LabChaos Instance { get; private set; }

        public override void OnEnabled()
        {
            RegisterEvents();
            Instance = this;
            base.OnEnabled();
            string welcomeText = @"
             _          _      ____ _                     
            | |    __ _| |__  / ___| |__   __ _  ___  ___ 
            | |   / _` | '_ \| |   | '_ \ / _` |/ _ \/ __|
            | |__| (_| | |_) | |___| | | | (_| | (_) \__ \
            |_____\__,_|_.__/ \____|_| |_|\__,_|\___/|___/                                   
            ";
            Log.Info(welcomeText);
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            Instance = null;
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            serverHandler = new ServerHandler();

            ServerEvent.RoundStarted += serverHandler.OnRoundStarted;
            ServerEvent.RoundEnded += serverHandler.OnRoundEnded;
        }

        private void UnregisterEvents()
        {
            ServerEvent.RoundStarted -= serverHandler.OnRoundStarted;
            ServerEvent.RoundEnded -= serverHandler.OnRoundEnded;

            serverHandler = null;
        }

        public override string Name => "LabChaos";
        public override string Author => "Dexler";
        public override Version Version => new Version(1, 0, 0);
        public override PluginPriority Priority => PluginPriority.Last;
    }
}
