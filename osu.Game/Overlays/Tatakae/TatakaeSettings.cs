
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Overlays.Tatakae
{
    // TTKTODO: add back all the settings here
    // ideally they all set defaults and the user SettingsOverlay
    // can temporarily override them
    [Cached(typeof(TatakaeSettings))]
    public partial class TatakaeSettings : VisibilityContainer
    {
        public const double TRANSITION_DURATION = 300;

        protected override bool StartHidden => true;

        protected override void PopIn()
        {
            this.FadeIn(TRANSITION_DURATION, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.FadeOut(TRANSITION_DURATION, Easing.OutQuint);
        }

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            RelativeSizeAxes = Axes.Both;

            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Child = new OsuSpriteText
                {
                    Text = "hello!"
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Show();
        }
    }
}
