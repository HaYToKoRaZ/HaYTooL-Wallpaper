using System;
using System.Collections.Generic;
using System.Drawing;

namespace Setting.Services
{
    public class AppTheme
    {
        public string Name { get; set; }
        public Color BackgroundColor { get; set; }
        public Color SurfaceColor { get; set; }
        public Color TextPrimary { get; set; }
        public Color TextSecondary { get; set; }
        public Color PrimaryColor { get; set; }
        public Color AccentColor { get; set; }
        public Color ButtonText { get; set; }
        public string FontFamily { get; set; } = "Segoe UI";
    }

    public static class ThemeFactory
    {
        public static readonly Dictionary<string, AppTheme> Themes = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Modern Minimalist"] = new AppTheme
            {
                Name = "Modern Minimalist",
                BackgroundColor = Color.FromArgb(248, 249, 250),
                SurfaceColor = Color.White,
                TextPrimary = Color.FromArgb(30, 41, 59),
                TextSecondary = Color.FromArgb(100, 116, 139),
                PrimaryColor = Color.FromArgb(79, 70, 229),
                AccentColor = Color.FromArgb(99, 102, 241),
                ButtonText = Color.White
            },
            ["Midnight Galaxy"] = new AppTheme
            {
                Name = "Midnight Galaxy",
                BackgroundColor = Color.FromArgb(22, 16, 34),
                SurfaceColor = Color.FromArgb(43, 30, 62),
                TextPrimary = Color.FromArgb(240, 238, 250),
                TextSecondary = Color.FromArgb(164, 144, 194),
                PrimaryColor = Color.FromArgb(124, 58, 237),
                AccentColor = Color.FromArgb(167, 139, 250),
                ButtonText = Color.White
            },
            ["Ocean Depths"] = new AppTheme
            {
                Name = "Ocean Depths",
                BackgroundColor = Color.FromArgb(15, 23, 42),
                SurfaceColor = Color.FromArgb(30, 41, 59),
                TextPrimary = Color.FromArgb(241, 245, 249),
                TextSecondary = Color.FromArgb(148, 163, 184),
                PrimaryColor = Color.FromArgb(14, 165, 233),
                AccentColor = Color.FromArgb(56, 189, 248),
                ButtonText = Color.White
            },
            ["Sunset Boulevard"] = new AppTheme
            {
                Name = "Sunset Boulevard",
                BackgroundColor = Color.FromArgb(28, 16, 24),
                SurfaceColor = Color.FromArgb(48, 24, 40),
                TextPrimary = Color.FromArgb(255, 241, 242),
                TextSecondary = Color.FromArgb(244, 114, 182),
                PrimaryColor = Color.FromArgb(225, 29, 72),
                AccentColor = Color.FromArgb(251, 146, 60),
                ButtonText = Color.White
            },
            ["Forest Canopy"] = new AppTheme
            {
                Name = "Forest Canopy",
                BackgroundColor = Color.FromArgb(18, 30, 24),
                SurfaceColor = Color.FromArgb(28, 46, 36),
                TextPrimary = Color.FromArgb(236, 253, 245),
                TextSecondary = Color.FromArgb(110, 231, 183),
                PrimaryColor = Color.FromArgb(16, 185, 129),
                AccentColor = Color.FromArgb(52, 211, 153),
                ButtonText = Color.White
            },
            ["Tech Innovation"] = new AppTheme
            {
                Name = "Tech Innovation",
                BackgroundColor = Color.FromArgb(11, 15, 25),
                SurfaceColor = Color.FromArgb(22, 27, 46),
                TextPrimary = Color.FromArgb(248, 250, 252),
                TextSecondary = Color.FromArgb(148, 163, 184),
                PrimaryColor = Color.FromArgb(6, 182, 212),
                AccentColor = Color.FromArgb(99, 102, 241),
                ButtonText = Color.White
            },
            ["Golden Hour"] = new AppTheme
            {
                Name = "Golden Hour",
                BackgroundColor = Color.FromArgb(30, 24, 16),
                SurfaceColor = Color.FromArgb(48, 36, 22),
                TextPrimary = Color.FromArgb(254, 243, 199),
                TextSecondary = Color.FromArgb(251, 191, 36),
                PrimaryColor = Color.FromArgb(217, 119, 6),
                AccentColor = Color.FromArgb(245, 158, 11),
                ButtonText = Color.White
            },
            ["Arctic Frost"] = new AppTheme
            {
                Name = "Arctic Frost",
                BackgroundColor = Color.FromArgb(240, 249, 255),
                SurfaceColor = Color.White,
                TextPrimary = Color.FromArgb(12, 74, 110),
                TextSecondary = Color.FromArgb(3, 105, 161),
                PrimaryColor = Color.FromArgb(2, 132, 199),
                AccentColor = Color.FromArgb(56, 189, 248),
                ButtonText = Color.White
            },
            ["Desert Rose"] = new AppTheme
            {
                Name = "Desert Rose",
                BackgroundColor = Color.FromArgb(255, 247, 246),
                SurfaceColor = Color.White,
                TextPrimary = Color.FromArgb(136, 19, 55),
                TextSecondary = Color.FromArgb(190, 24, 93),
                PrimaryColor = Color.FromArgb(219, 39, 119),
                AccentColor = Color.FromArgb(244, 114, 182),
                ButtonText = Color.White
            },
            ["Botanical Garden"] = new AppTheme
            {
                Name = "Botanical Garden",
                BackgroundColor = Color.FromArgb(242, 253, 245),
                SurfaceColor = Color.White,
                TextPrimary = Color.FromArgb(6, 78, 59),
                TextSecondary = Color.FromArgb(5, 150, 105),
                PrimaryColor = Color.FromArgb(16, 185, 129),
                AccentColor = Color.FromArgb(52, 211, 153),
                ButtonText = Color.White
            }
        };

        public static AppTheme GetTheme(string name)
        {
            if (string.IsNullOrEmpty(name) || !Themes.TryGetValue(name, out var theme))
            {
                return Themes["Modern Minimalist"];
            }
            return theme;
        }
    }
}
