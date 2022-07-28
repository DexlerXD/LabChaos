using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using LabChaos.Methods;
using Exiled.Events.EventArgs;

namespace LabChaos.Events
{
    public class ServerHandler
    {
        private EventRandomizer er = new EventRandomizer();

        public void OnRoundStarted()
        {
            Timing.RunCoroutine(EventsSequence());
            Log.Info("Starting event sequence! Time set to: " + LabChaos.Instance.Config.TimeBetweenEvents);
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Timing.KillCoroutines();
            Server.FriendlyFire = false;
        }

        private IEnumerator<float> EventsSequence()
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(LabChaos.Instance.Config.TimeBetweenEvents);
                Log.Info("Firing an event!");
                er.InvokeRandomEvent();
            }
        }
    }
}
