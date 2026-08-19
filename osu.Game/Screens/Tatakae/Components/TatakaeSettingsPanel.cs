// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Framework.Testing;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Overlays.Settings.Sections;
using osu.Game.Overlays.Settings.Sections.Input;
using osuTK.Graphics;

namespace osu.Game.Screens.Tatakae.Components
{
    // a bodge job of reworking SettingsPanel to work
    // outside of overlays
    // TODO: this could be reimplemented via reinheriting settingspanel
    // but I can't be bothered with making that work
    public partial class TatakaeSettingsPanel : Container
    {

        private const float sidebar_width = SettingsSidebar.EXPANDED_WIDTH;

        // extra right padding to give room to the revert-to-default button in settings controls.
        public static readonly MarginPadding CONTENT_PADDING = new MarginPadding { Left = 12, Right = 22 };

        public SettingsPanel.SettingsSectionsContainer SectionsContainer { get; private set; } = null!;

        protected Container<Drawable> ContentContainer = null!;
        protected override Container<Drawable> Content => ContentContainer;


        protected SeekLimitedSearchTextBox SearchTextBox { get; private set; } = null!;

        protected SettingsSidebar CategoriesSidebar = null!;
        private SidebarIconButton selectedSidebarButton = null!;

        private LoadingLayer loading = null!;

        private readonly List<SettingsSection> loadableSections = new List<SettingsSection>();

        private Task sectionsLoadingTask = null!;

        public IBindable<SettingsSection> CurrentSection = new Bindable<SettingsSection>();
        protected IEnumerable<SettingsSection> CreateSections()
        {
            var sections = new List<SettingsSection>
            {
                new GeneralSection(true),
                new SkinSection(),
                new InputSection(createSubPanel(new KeyBindingPanel())),
                new UserInterfaceSection(),
                new GameplaySection(),
                new RulesetSection(),
                new AudioSection(),
                new GraphicsSection(),
                new OnlineSection(),
                new MaintenanceSection(),
                new DebugSection()
            };

            return sections;
        }

        private readonly List<SettingsSubPanel> subPanels = new List<SettingsSubPanel>();

        private SettingsSubPanel lastOpenedSubPanel = null!;

        // protected override Drawable CreateFooter() => new OsuContextMenuContainer
        // {
        //     RelativeSizeAxes = Axes.X,
        //     AutoSizeAxes = Axes.Y,
        //     Child = new SettingsFooter()
        // };

        public override bool AcceptsFocus => lastOpenedSubPanel == null || lastOpenedSubPanel.State.Value == Visibility.Hidden;

        public void ShowAtControl<T>()
            where T : Drawable
        {
            // if search isn't cleared then the target control won't be visible if it doesn't match the query
            SearchTextBox.Current.SetDefault();

            Show();

            // wait for load of sections
            if (!SectionsContainer.Any())
            {
                Scheduler.Add(ShowAtControl<T>);
                return;
            }

            SectionsContainer.ScrollTo(SectionsContainer.ChildrenOfType<T>().Single());
        }

        private T createSubPanel<T>(T subPanel)
            where T : SettingsSubPanel
        {
            subPanel.Depth = 1;
            subPanel.Anchor = Anchor.TopRight;
            subPanel.State.ValueChanged += e => subPanelStateChanged(subPanel, e);

            subPanels.Add(subPanel);

            return subPanel;
        }

        private const double fade_in_duration = 500;

        private void loadSections()
        {
            if (sectionsLoadingTask != null)
                return;

            sectionsLoadingTask = LoadComponentsAsync(loadableSections, sections =>
            {
                SectionsContainer.AddRange(sections);
                SectionsContainer.Footer?.FadeInFromZero(fade_in_duration, Easing.OutQuint);
                SectionsContainer.SearchContainer.FadeInFromZero(fade_in_duration, Easing.OutQuint);

                loading.Hide();

                SearchTextBox.Current.BindValueChanged(term => SectionsContainer.SearchTerm = term.NewValue, true);

                loadSidebarButtons();
            });
        }

        private void loadSidebarButtons()
        {
            if (CategoriesSidebar == null)
                return;

            LoadComponentsAsync(createSidebarButtons(), buttons =>
            {
                float delay = 0;

                foreach (var button in buttons)
                {
                    CategoriesSidebar.Add(button);

                    button.FadeOut()
                          .Delay(delay)
                          .FadeInFromZero(fade_in_duration, Easing.OutQuint);

                    delay += 40;
                }

                SectionsContainer.SelectedSection.BindValueChanged(section =>
                {
                    if (selectedSidebarButton != null)
                        selectedSidebarButton.Selected = false;

                    selectedSidebarButton = CategoriesSidebar.Children.OfType<SidebarIconButton>().FirstOrDefault(b => b.Section == section.NewValue)!;

                    if (selectedSidebarButton != null)
                        selectedSidebarButton.Selected = true;
                }, true);
            });
        }
        private IEnumerable<SidebarIconButton> createSidebarButtons()
        {
            foreach (var section in SectionsContainer)
            {
                yield return new SidebarIconButton
                {
                    Section = section,
                    Action = () =>
                    {
                        if (!SectionsContainer.IsLoaded)
                            return;

                        SectionsContainer.ScrollTo(section);
                    },
                };
            }
        }

        private void subPanelStateChanged(SettingsSubPanel panel, ValueChangedEvent<Visibility> state)
        {
            switch (state.NewValue)
            {
                case Visibility.Visible:
                    CategoriesSidebar.Expanded.Value = false;
                    CategoriesSidebar.FadeColour(Color4.DarkGray, 300, Easing.OutQuint);

                    SectionsContainer.FadeOut(300, Easing.OutQuint);
                    Content.FadeOut(20);

                    lastOpenedSubPanel = panel;

                    break;

                case Visibility.Hidden:
                    CategoriesSidebar.Expanded.Value = true;
                    CategoriesSidebar.FadeColour(Color4.White, 300, Easing.OutQuint);

                    SectionsContainer.FadeIn(500, Easing.OutQuint);
                    Content.FadeIn(20);
                    break;
            }
        }

        protected virtual float ExpandedPosition => 0;

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colours)
        {
            // TTKTODO: ideally this should be refactored to not be nonmasked
            // but i can't for the life of me figure out how to make settingssidebar
            // scale with relativesizeaxes correctly, and i'm too scared to ask devs
            // as i'm already shitting up their codebase enough as is.
            InternalChild = ContentContainer = new NonMaskedContent
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = colours.Background4,
                        Alpha = 1,
                    },
                    loading = new LoadingLayer
                    {
                        State = { Value = Visibility.Visible }
                    }
                }
            };

            Add(new PopoverContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = SectionsContainer = new SettingsPanel.SettingsSectionsContainer
                {
                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = Color4.Black.Opacity(0),
                        Type = EdgeEffectType.Shadow,
                        Hollow = true,
                        Radius = 10
                    },
                    MaskingSmoothness = 0,
                    RelativeSizeAxes = Axes.Both,
                    SelectedSection = { BindTarget = CurrentSection! },
                    FixedHeader = new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding
                        {
                            Vertical = 6,
                            Left = CONTENT_PADDING.Left,
                            Right = CONTENT_PADDING.Right,
                        },
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Child = SearchTextBox = new SettingsSearchTextBox
                        {
                            RelativeSizeAxes = Axes.X,
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                        }
                    },
                    // Footer = CreateFooter().With(f => f.Alpha = 0)
                }


            });

            AddInternal(CategoriesSidebar = new SettingsSidebar(false)
            {
                BackButtonAction = Hide,
                Width = sidebar_width
            });

            CreateSections()?.ForEach(AddSection);

            foreach (var s in subPanels)
                Content.Add(s);

            Scheduler.Add(loadSections);

        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Content.Margin = new MarginPadding { Left = CategoriesSidebar?.DrawWidth ?? 0 };
        }

        protected void AddSection(SettingsSection section)
        {
            if (IsLoaded)
                // just to keep things simple. can be accommodated for if we ever need it.
                throw new InvalidOperationException("All sections must be added before the panel is loaded.");

            loadableSections.Add(section);
        }

        private partial class NonMaskedContent : Container<Drawable>
        {
            // masking breaks the pan-out transform with nested sub-settings panels.
            protected override bool ComputeIsMaskedAway(RectangleF maskingBounds) => false;
        }
    }
}
