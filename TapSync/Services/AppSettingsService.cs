using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TapSync.Services
{
    public enum BackgroundMode
    {
        Mica,
        Acrylic,
        Image
    }

    public enum AppTheme
    {
        System,
        Light,
        Dark
    }

    public sealed class AppSettings
    {
        public AppTheme Theme { get; set; } = AppTheme.System;
        public BackgroundMode BackgroundMode { get; set; } = BackgroundMode.Mica;
        public string? BackgroundImagePath { get; set; }
        public bool BlurEnabled { get; set; } = true;

        /// <summary>How strongly the theme colour is layered over the picture, 0 to 1.</summary>
        public double OverlayOpacity { get; set; } = 0.4;
    }

    [JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
    [JsonSerializable(typeof(AppSettings))]
    internal sealed partial class AppSettingsJsonContext : JsonSerializerContext
    {
    }

    /// <summary>
    /// Loads and saves user preferences to %LocalAppData%\TapSync\settings.json and
    /// raises <see cref="SettingsChanged"/> so the window can restyle itself live.
    /// </summary>
    public sealed class AppSettingsService
    {
        private static readonly Lazy<AppSettingsService> _instance = new(() => new AppSettingsService());

        public static AppSettingsService Instance => _instance.Value;

        public static string DataFolder { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TapSync");

        private static string SettingsPath => Path.Combine(DataFolder, "settings.json");
        private static string BackgroundsFolder => Path.Combine(DataFolder, "Backgrounds");

        public static readonly string[] SupportedImageTypes = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp" };

        public AppSettings Current { get; }

        public event EventHandler? SettingsChanged;

        private AppSettingsService()
        {
            Current = Load();
        }

        public void Update(Action<AppSettings> change)
        {
            change(Current);
            Current.OverlayOpacity = Math.Clamp(Current.OverlayOpacity, 0, 1);
            Save();
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Copies the chosen picture into the app's own folder so the background keeps
        /// working if the original file is moved or deleted. Returns the copy's path.
        /// </summary>
        public string ImportBackgroundImage(string sourcePath)
        {
            Directory.CreateDirectory(BackgroundsFolder);

            var extension = Path.GetExtension(sourcePath).ToLowerInvariant();
            var destination = Path.Combine(BackgroundsFolder, $"background-{DateTime.Now:yyyyMMdd-HHmmss}{extension}");
            File.Copy(sourcePath, destination, overwrite: true);

            DeleteOtherBackgrounds(keep: destination);
            return destination;
        }

        public void RemoveBackgroundImage()
        {
            Update(s =>
            {
                s.BackgroundImagePath = null;
                if (s.BackgroundMode == BackgroundMode.Image)
                {
                    s.BackgroundMode = BackgroundMode.Mica;
                }
            });
            DeleteOtherBackgrounds(keep: null);
        }

        public bool HasUsableImage =>
            !string.IsNullOrEmpty(Current.BackgroundImagePath) && File.Exists(Current.BackgroundImagePath);

        private static void DeleteOtherBackgrounds(string? keep)
        {
            if (!Directory.Exists(BackgroundsFolder)) return;

            foreach (var file in Directory.GetFiles(BackgroundsFolder))
            {
                if (keep != null && string.Equals(file, keep, StringComparison.OrdinalIgnoreCase)) continue;
                try { File.Delete(file); } catch (IOException) { } catch (UnauthorizedAccessException) { }
            }
        }

        private static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    return JsonSerializer.Deserialize(json, AppSettingsJsonContext.Default.AppSettings) ?? new AppSettings();
                }
            }
            catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
            {
                System.Diagnostics.Debug.WriteLine($"Could not read settings, using defaults: {ex.Message}");
            }

            return new AppSettings();
        }

        private void Save()
        {
            try
            {
                Directory.CreateDirectory(DataFolder);
                File.WriteAllText(SettingsPath, JsonSerializer.Serialize(Current, AppSettingsJsonContext.Default.AppSettings));
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                System.Diagnostics.Debug.WriteLine($"Could not save settings: {ex.Message}");
            }
        }
    }
}
