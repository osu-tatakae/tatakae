
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Screens.Tatakae.Components.Menus;
using osu.Game.Tatakae.Configuration;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Threading;
using osu.Framework.Threading;
using System.Threading.Tasks;
using System.Linq;
using osu.Game.Screens.Tatakae.Tabs;

namespace osu.Game.Screens.Tatakae
{
    public partial class TatakaeScreen : OsuScreen
    {

        public const float TOPBAR_HEIGHT = 40;
        private Bindable<TatakaeMode> tatakaeMode = null!;

        private ScreenContainer screenContainer = null!;
        private TatakaeTabScreen? currentTabScreen;
        public readonly Bindable<TatakaeTabMode> Tab = new Bindable<TatakaeTabMode>();

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
                    new Box
                    {
                        Name = "bg",
                        RelativeSizeAxes = Axes.Both,
                        Colour = colours.Background3
                    },
                    screenContainer = new ScreenContainer
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
                        Child = new TatakaeTabSwitcherControl
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Current = Tab
                        }
                    }
                }
            });

        }
        protected override void LoadComplete()
        {
            base.LoadComplete();

            Tab.Value = TatakaeTabMode.General;
            Tab.BindValueChanged(onTabChanged, true);
        }
        private void onTabChanged(ValueChangedEvent<TatakaeTabMode> e)
        {
            var lastScreen = currentTabScreen;
            lastScreen?.Hide();
            try
            {
                if ((currentTabScreen = screenContainer.SingleOrDefault(s => s.Type == e.NewValue)) != null)
                {
                    screenContainer.ChangeChildDepth(currentTabScreen, lastScreen?.Depth + 1 ?? 0);

                    currentTabScreen.Show();
                    return;
                }

                switch (e.NewValue)
                {
                    case TatakaeTabMode.General:
                        currentTabScreen = new TatakaeGeneralTab();
                        break;
                    case TatakaeTabMode.Defaults:
                        currentTabScreen = new TatakaeDefaultsTab();
                        break;
                    case TatakaeTabMode.About:
                        currentTabScreen = new TatakaeAboutTab();
                        break;
                    default:
                        throw new InvalidOperationException("Tatakae menu bar switched to an unknown tab");
                }

                screenContainer.LoadComponentAsync(currentTabScreen, newScreen =>
                {
                    if (newScreen == currentTabScreen)
                    {
                        screenContainer.Add(newScreen);
                        newScreen.Show();
                    }
                });
            }
            finally
            {

            }
        }
        private partial class ScreenContainer : Container<TatakaeTabScreen>
        {
            public new Task LoadComponentAsync<TLoadable>([NotNull] TLoadable component, Action<TLoadable> onLoaded = null!, CancellationToken cancellation = default, Scheduler scheduler = null!)
                where TLoadable : Drawable
                => base.LoadComponentAsync(component, onLoaded, cancellation, scheduler);
        }
    }
}
