// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Localisation;
using osu.Game.Overlays.Settings.Sections.General;
using osu.Game.Updater;

namespace osu.Game.Overlays.Settings.Sections
{
    public partial class GeneralSection : SettingsSection
    {
        public override LocalisableString Header => CommonStrings.General;

        private bool showOperatorOptions;
        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = OsuIcon.Settings
        };

        public GeneralSection(bool showOperatorOptions = false)
        {
            this.showOperatorOptions = showOperatorOptions;
        }

        [BackgroundDependencyLoader]
        private void load(UpdateManager? updateManager)
        {
            if (showOperatorOptions)
            {
                Add(new QuickActionSettings());
                if (RuntimeInfo.IsDesktop)
                    Add(new InstallationSettings());
            }
            Add(new LanguageSettings());
            // if (updateManager?.CanCheckForUpdate == true)
            // Add(new UpdateSettings());

        }
    }
}
