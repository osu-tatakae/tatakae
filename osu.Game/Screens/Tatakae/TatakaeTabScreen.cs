using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Screens.Tatakae
{
    public abstract partial class TatakaeTabScreen : VisibilityContainer
    {
        public readonly TatakaeTabMode Type;

        protected TatakaeTabScreen(TatakaeTabMode type)
        {
            Type = type;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
        }

        protected override void PopIn() => this.FadeIn();

        protected override void PopOut() => this.FadeOut();
    }
}
