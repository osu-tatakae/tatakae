
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Graphics;
using osu.Game.Screens.Tatakae.Components.Menus;
using osu.Game.Tatakae.Configuration;

namespace osu.Game.Screens.Tatakae
{
    public partial class TatakaeScreen : OsuScreen
    {

        public const float TOPBAR_HEIGHT = 40;
        private Bindable<TatakaeMode> tatakaeMode = null!;

        public readonly Bindable<TatakaeScreenTab> Tab = new Bindable<TatakaeScreenTab>();

        [Cached]
        private readonly OverlayColourProvider colours = new OverlayColourProvider(OverlayColourScheme.Orange);

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            tatakaeMode = config.GetBindable<TatakaeMode>(OsuSetting.TatakaeMode);

            AddInternal(new OsuContextMenuContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Container
                    {
                        Name = "Screen container",
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Top = TOPBAR_HEIGHT }
                    },
                    new Container
                    {
                        Name = "Topbar",
                        RelativeSizeAxes = Axes.X,
                        Height = TOPBAR_HEIGHT,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = colours.Background3
                            },
                            new TatakaeTabSwitcherControl
                            {
                                Anchor = Anchor.BottomLeft,
                                Origin = Anchor.BottomLeft,
                                Current = Tab
                            }
                        }
                    }
                }
            });

        }
    }
}
