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

using CommunityToolkit.Mvvm.Input;
using ShareX.ImageEditor.Integration;

namespace ShareX.ImageEditor.Presentation.ViewModels;

public sealed class EditorSettingsDialogViewModel : ViewModelBase
{
    private readonly Action<ImageEditorOptions> _onSave;
    private readonly Action _onCancel;

    public EditorSettingsDialogViewModel(ImageEditorOptions source, Action<ImageEditorOptions> onSave, Action onCancel)
    {
        _onSave = onSave;
        _onCancel = onCancel;

        _rememberWindowState = source.RememberWindowState;
        _showExitConfirmation = source.ShowExitConfirmation;
        _zoomToFitOnOpen = source.ZoomToFitOnOpen;
        _quickCrop = source.QuickCrop;
        _autoCloseEditorOnTask = source.AutoCloseEditorOnTask;
        _autoCopyImageToClipboard = source.AutoCopyImageToClipboard;
        _showInsertImageDialog = source.ShowInsertImageDialog;
        _showNotifications = source.ShowNotifications;
        _showToolbarButtonCaptions = source.ShowToolbarButtonCaptions;

        OkCommand = new RelayCommand(Ok);
        CancelCommand = new RelayCommand(_onCancel);
    }

    public IRelayCommand OkCommand { get; }
    public IRelayCommand CancelCommand { get; }

    private bool _rememberWindowState;
    public bool RememberWindowState
    {
        get => _rememberWindowState;
        set => SetProperty(ref _rememberWindowState, value);
    }

    private bool _showExitConfirmation;
    public bool ShowExitConfirmation
    {
        get => _showExitConfirmation;
        set => SetProperty(ref _showExitConfirmation, value);
    }

    private bool _zoomToFitOnOpen;
    public bool ZoomToFitOnOpen
    {
        get => _zoomToFitOnOpen;
        set => SetProperty(ref _zoomToFitOnOpen, value);
    }

    private bool _quickCrop;
    public bool QuickCrop
    {
        get => _quickCrop;
        set => SetProperty(ref _quickCrop, value);
    }

    private bool _autoCloseEditorOnTask;
    public bool AutoCloseEditorOnTask
    {
        get => _autoCloseEditorOnTask;
        set => SetProperty(ref _autoCloseEditorOnTask, value);
    }

    private bool _autoCopyImageToClipboard;
    public bool AutoCopyImageToClipboard
    {
        get => _autoCopyImageToClipboard;
        set => SetProperty(ref _autoCopyImageToClipboard, value);
    }

    private bool _showInsertImageDialog;
    public bool ShowInsertImageDialog
    {
        get => _showInsertImageDialog;
        set => SetProperty(ref _showInsertImageDialog, value);
    }

    private bool _showNotifications;
    public bool ShowNotifications
    {
        get => _showNotifications;
        set => SetProperty(ref _showNotifications, value);
    }

    private bool _showToolbarButtonCaptions;
    public bool ShowToolbarButtonCaptions
    {
        get => _showToolbarButtonCaptions;
        set => SetProperty(ref _showToolbarButtonCaptions, value);
    }

    private void Ok()
    {
        ImageEditorOptions result = new()
        {
            RememberWindowState = RememberWindowState,
            ShowExitConfirmation = ShowExitConfirmation,
            ZoomToFitOnOpen = ZoomToFitOnOpen,
            QuickCrop = QuickCrop,
            AutoCloseEditorOnTask = AutoCloseEditorOnTask,
            AutoCopyImageToClipboard = AutoCopyImageToClipboard,
            ShowInsertImageDialog = ShowInsertImageDialog,
            ShowNotifications = ShowNotifications,
            ShowToolbarButtonCaptions = ShowToolbarButtonCaptions
        };

        _onSave(result);
    }
}
