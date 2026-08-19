
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Screens.Tatakae.Components;

namespace osu.Game.Screens.Tatakae.Tabs
{
    public partial class TatakaeDefaultsTab : TatakaeTabScreen
    {

        private OsuScrollContainer scroll = null!;

        private FillFlowContainer flow = null!;

        private TatakaeSettingsPanel settings = null!;

        [Cached]
        private OverlayColourProvider colourProvider = new OverlayColourProvider(OverlayColourScheme.Red);

        public TatakaeDefaultsTab() : base(TatakaeTabMode.Defaults)
        {

        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = scroll = new OsuScrollContainer
            {
                Masking = false,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                RelativeSizeAxes = Axes.Y,
                Width = 600,
                Padding = new MarginPadding(15),
                Children = new Drawable[]
                {
                    settings = new TatakaeSettingsPanel()
                    {
                        X = -100,
                        RelativeSizeAxes = Axes.X,
                        Height = 670,
                    }
                },
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            settings.Show();
        }
    }
}
