using Exiled.API.Interfaces;
using System.ComponentModel;

namespace LabChaos
{
    public class Config : IConfig
    {
        [Description("Whether the plugin enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Time in seconds, which defines an interval between random events. Default = 250.")]
        public float TimeBetweenEvents { get; private set; } = 250f;
    }
}
