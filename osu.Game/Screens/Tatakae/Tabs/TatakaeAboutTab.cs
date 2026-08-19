
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays.Settings;

namespace osu.Game.Screens.Tatakae.Tabs
{
    public partial class TatakaeAboutTab : TatakaeTabScreen
    {
        private OsuScrollContainer scroll = null!;
        public TatakaeAboutTab() : base(TatakaeTabMode.About)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(scroll = new OsuScrollContainer
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.Y,
                Width = 500,
                Padding = new MarginPadding(15),

                Children = new Drawable[]
                {
                    new OsuTextFlowContainer
                    {
                        Text = "Created by Lucy Faria, 2026.\n"
                    },
                    new SettingsFooter()
                }
            });
        }
    }
}
