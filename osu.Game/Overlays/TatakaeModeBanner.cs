// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Tatakae.Configuration;

namespace osu.Game.Overlays
{
    public partial class TatakaeModeBanner : VisibilityContainer
    {

        private OsuSpriteText modeText = null!;

        private IBindable<TatakaeMode> tatakaeMode = null!;

        private bool devTextShowing;

        public TatakaeModeBanner(bool isDeployedBuild = true)
        {
            devTextShowing = !isDeployedBuild;
        }
        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            AutoSizeAxes = Axes.Both;

            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;

            Alpha = 0;

            if (devTextShowing)
            {
                Y = -12;
            }

            Add(modeText = new OsuSpriteText
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Font = OsuFont.Default.With(weight: FontWeight.Bold, size: 16),
                Colour = Colour4.White,
            });

            tatakaeMode = config.GetBindable<TatakaeMode>(OsuSetting.TatakaeMode);
        }

        private void updateText()
        {
            switch (tatakaeMode.Value)
            {
                case TatakaeMode.EventMode:
                    modeText.Text = "EVENT MODE";
                    break;
                case TatakaeMode.CreditMode:
                    throw new System.NotImplementedException();
            }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            tatakaeMode.BindValueChanged(_ => updateText(), true);
        }

        protected override void PopIn()
        {
            this.FadeIn(1400, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.FadeOut(500, Easing.OutQuint);
        }
    }
}
