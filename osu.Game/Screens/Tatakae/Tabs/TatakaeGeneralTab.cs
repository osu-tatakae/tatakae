using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Tatakae.Configuration;
using osuTK;

namespace osu.Game.Screens.Tatakae.Tabs
{
    public partial class TatakaeGeneralTab : TatakaeTabScreen
    {
        private TatakaeSettingsPanel panel = null!;

        public TatakaeGeneralTab() : base(TatakaeTabMode.General) { }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(panel = new TatakaeSettingsPanel()
            {
                RelativeSizeAxes = Axes.Both,
            });
        }


        public partial class TatakaeSettingsPanel : Container
        {
            private OsuScrollContainer scroll = null!;

            private FillFlowContainer flow = null!;

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colours, OsuConfigManager config)
            {
                Child = scroll = new OsuScrollContainer
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Y,
                    Width = 500,
                    Padding = new MarginPadding(15),
                    Child = flow = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Full,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Spacing = new Vector2(25),
                        Children = createItems(config),
                    }
                };
            }

            private Drawable[] createItems(OsuConfigManager config) => new Drawable[]
            {
                new FillFlowContainer
                {
                    Name = "Unsorted items",
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new SettingsItemV2(new FormEnumDropdown<TatakaeMode>
                        {
                            Caption = "Operation mode",
                            Current = config.GetBindable<TatakaeMode>(OsuSetting.TatakaeMode)
                        })
                    }
                },


            };
        }

    }
}
