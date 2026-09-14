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

using System;
using System.IO;
using Newtonsoft.Json;
using ShareX.ImageEditor.Integration;

namespace ShareX.ImageEditor.App
{
    // Standalone app has no host application config, so it keeps its own small JSON settings file.
    internal static class ImageEditorSettingsManager
    {
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ShareX", "ImageEditor", "ImageEditorSettings.json");

        public static ImageEditorOptions Load()
        {
            try
            {
                if (!File.Exists(SettingsFilePath))
                {
                    return new ImageEditorOptions();
                }

                string json = File.ReadAllText(SettingsFilePath);
                return JsonConvert.DeserializeObject<ImageEditorOptions>(json) ?? new ImageEditorOptions();
            }
            catch (Exception)
            {
                return new ImageEditorOptions();
            }
        }

        public static void Save(ImageEditorOptions options)
        {
            try
            {
                string? directory = Path.GetDirectoryName(SettingsFilePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonConvert.SerializeObject(options, Formatting.Indented);
                string tempFilePath = SettingsFilePath + ".tmp";
                File.WriteAllText(tempFilePath, json);
                File.Move(tempFilePath, SettingsFilePath, overwrite: true);
            }
            catch (Exception)
            {
                // Best effort: losing settings on a write failure shouldn't crash the app.
            }
        }
    }
}
