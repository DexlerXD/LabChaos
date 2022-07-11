using System;

namespace LabChaos.Methods
{
    public class EventRandomizer
    {
        ChaosEvents chaosEv = new ChaosEvents();
        Random rand = new Random();

        public void RandomizeEvent()
        {
            chaosEv.events[rand.Next(chaosEv.events.Count)].Invoke();
        }
    }
}
