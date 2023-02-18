using Exiled.API.Interfaces;
using System.ComponentModel;

namespace LabChaos
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not debug messages should be shown in the console.")]
        public bool Debug { get; set; }

        [Description("Time in seconds, which defines an interval between random events. Default = 120.")]
        public float TimeBetweenEvents { get; private set; } = 120f;
    }
}
