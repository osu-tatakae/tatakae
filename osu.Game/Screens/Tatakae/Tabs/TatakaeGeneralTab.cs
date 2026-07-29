using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays;

namespace osu.Game.Screens.Tatakae.Tabs
{
    public partial class TatakaeGeneralTab : TatakaeTabScreen
    {
        public TatakaeGeneralTab() : base(TatakaeTabMode.General) { }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colours)
        {
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colours.Background3
                }
            };
        }
    }
}
