using System.ComponentModel;

namespace osu.Game.Screens.Tatakae
{
    public enum TatakaeTabMode
    {
        [Description("tatakae! settings")]
        General,
        [Description("osu! settings and defaults")]
        Defaults,
        [Description("about")]
        About
    }
}
