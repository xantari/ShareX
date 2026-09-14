#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2026 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using Avalonia;
using Avalonia.Controls;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Presentation.Controls
{
    // Wraps a toolbar control and, when enabled, shows a caption below it derived from its ToolTip.
    public sealed class ToolbarItemHost : ContentControl
    {
        public static readonly StyledProperty<string?> CaptionProperty =
            AvaloniaProperty.Register<ToolbarItemHost, string?>(nameof(Caption));

        public static readonly StyledProperty<string?> EffectiveCaptionProperty =
            AvaloniaProperty.Register<ToolbarItemHost, string?>(nameof(EffectiveCaption));

        public static readonly StyledProperty<bool> IsCaptionVisibleProperty =
            AvaloniaProperty.Register<ToolbarItemHost, bool>(nameof(IsCaptionVisible));

        private Control? _observedChild;

        static ToolbarItemHost()
        {
            ContentProperty.Changed.AddClassHandler<ToolbarItemHost>((host, _) => host.OnContentChanged());
            CaptionProperty.Changed.AddClassHandler<ToolbarItemHost>((host, _) => host.RefreshCaption());
        }

        public string? Caption
        {
            get => GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public string? EffectiveCaption
        {
            get => GetValue(EffectiveCaptionProperty);
            private set => SetValue(EffectiveCaptionProperty, value);
        }

        public bool IsCaptionVisible
        {
            get => GetValue(IsCaptionVisibleProperty);
            private set => SetValue(IsCaptionVisibleProperty, value);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            RefreshCaption();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ToolbarCaptionOptions.ShowCaptionsProperty)
            {
                RefreshCaption();
            }
        }

        private void OnContentChanged()
        {
            if (_observedChild is not null)
            {
                _observedChild.PropertyChanged -= OnChildPropertyChanged;
            }

            _observedChild = Content as Control;

            if (_observedChild is not null)
            {
                _observedChild.PropertyChanged += OnChildPropertyChanged;
            }

            RefreshCaption();
        }

        private void OnChildPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == ToolTip.TipProperty)
            {
                RefreshCaption();
            }
        }

        private void RefreshCaption()
        {
            string? caption = Caption;

            if (string.IsNullOrEmpty(caption) && _observedChild is not null)
            {
                caption = ToolTip.GetTip(_observedChild)?.ToString();
            }

            EffectiveCaption = caption;

            bool visible = ToolbarCaptionOptions.GetShowCaptions(this) && !string.IsNullOrEmpty(caption);
            IsCaptionVisible = visible;
            PseudoClasses.Set(":captions", visible);
        }
    }
}
