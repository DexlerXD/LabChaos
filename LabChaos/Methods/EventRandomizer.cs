using System;

namespace LabChaos.Methods
{
    public class EventRandomizer
    {
        ChaosEvents chaosEv = new ChaosEvents();
        Random rand = new Random();

        public void InvokeRandomEvent()
        {
            chaosEv.events[rand.Next(chaosEv.events.Count)].Invoke();
        }
    }
}
