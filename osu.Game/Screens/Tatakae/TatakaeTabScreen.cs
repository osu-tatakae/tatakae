using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;

namespace osu.Game.Screens.Tatakae
{
    public abstract partial class TatakaeTabScreen : VisibilityContainer
    {
        public readonly TatakaeTabMode Type;
        private readonly Container content;

        protected TatakaeTabScreen(TatakaeTabMode type)
        {
            Type = type;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            InternalChild = content = new PopoverContainer { RelativeSizeAxes = Axes.Both };
        }

        protected override void PopIn() => this.FadeIn();

        protected override void PopOut() => this.FadeOut();
    }
}
