using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using LabChaos.Methods;

namespace LabChaos.Events
{
    public class ServerHandler
    {
        private EventRandomizer er = new EventRandomizer();

        public void OnRoundStarted()
        {
            Timing.RunCoroutine(EventCoroutine());
            Log.Info("Starting event coroutine! Time set to: " + LabChaos.Instance.Config.TimeBetweenEvents);
        }

        private IEnumerator<float> EventCoroutine()
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(LabChaos.Instance.Config.TimeBetweenEvents);
                er.RandomizeEvent();
            }
        }
    }
}
