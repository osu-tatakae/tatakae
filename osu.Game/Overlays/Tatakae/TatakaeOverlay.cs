using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Layout;
using osu.Game.Graphics.Containers;
using osu.Game.Input.Bindings;

namespace osu.Game.Overlays.Tatakae
{
    /// <summary>
    /// User-facing overlay to access usermode unchangeable settings.
    /// Adapted from SkinEditorOverlay.
    /// </summary>
    /// <see cref="SkinEditor.SkinEditorOverlay"/>
    public partial class TatakaeOverlay : OverlayContainer, IKeyBindingHandler<GlobalAction>
    {

        [Resolved]
        private OsuGame game { get; set; } = null!;

        private TatakaeSettings? tatakaeSettings;
        private readonly ScalingContainer scalingContainer;

        private readonly LayoutValue drawSizeLayout;


        public TatakaeOverlay(ScalingContainer scalingContainer)
        {
            this.scalingContainer = scalingContainer;
            RelativeSizeAxes = Axes.Both;

            AddLayout(drawSizeLayout = new LayoutValue(Invalidation.DrawSize));
        }

        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            if (e.Repeat)
                return false;

            switch (e.Action)
            {
                case GlobalAction.Back:
                    Hide();
                    return true;
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
        {
        }


        protected override void PopIn()
        {
            // have we loaded before?
            if (tatakaeSettings != null)
            {
                tatakaeSettings.Show();
                return;
            }

            // actually load
            var settings = new TatakaeSettings();

            settings.State.BindValueChanged(_ => updateVisibilityAndResizeScreen());

            tatakaeSettings = settings;

            LoadComponentAsync(settings, _ => AddInternal(settings));
        }

        private void updateScreenSizing()
        {
            if (tatakaeSettings?.State.Value != Visibility.Visible) return;

            const float padding = 10;

            float relativeSidebarWidth = (10 + padding) / DrawWidth;
            float relativeToolbarHeight = (10 + 10 + padding) / DrawHeight;

            var rect = new RectangleF(
                relativeSidebarWidth,
                relativeToolbarHeight,
                1 - relativeSidebarWidth * 2,
                1f - relativeToolbarHeight - padding / DrawHeight);

            scalingContainer.SetCustomRect(rect, true);
        }

        private void updateVisibilityAndResizeScreen()
        {
            Debug.Assert(tatakaeSettings != null);

            if (tatakaeSettings.State.Value == Visibility.Visible)
            {
                Scheduler.AddOnce(updateScreenSizing);

                // game.Toolbar.Hide();
            }
            else
            {
                scalingContainer.SetCustomRect(null);

                // if (lastTargetScreen?.HideOverlaysOnEnter != true)
                //     game.Toolbar.Show();
            }
        }
        protected override void PopOut()
        {
            tatakaeSettings?.Hide();
        }
    }
}
