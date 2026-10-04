using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Shared.Data;
using Shared.Services;
using Setting.Services;

namespace Setting
{
    public partial class Form1 : Form
    {
        private ComboBox cbSource;
        private ComboBox cbCategory;
        private CheckBox chkStartup;
        private CheckBox chkContextMenu;
        private Button btnSave;
        private Label lblSource;
        private Label lblCategory;
        private ComboBox cbLanguage;
        private Label lblLanguage;
        private Label lblTheme;
        private ComboBox cbTheme;
        private LinkLabel lblUpdate;
        private LinkLabel lblWebsite;

        private readonly SettingsRepository _settings;
        private string lang = "TR";

        public Form1()
        {
            _settings = new SettingsRepository();
            lang = _settings.Language;

            InitializeComponentUI();
            LoadSettings();
            UpdateLanguage();
            RunUpdateCheck();
        }

        private async void RunUpdateCheck()
        {
            var result = await UpdateService.CheckForUpdatesAsync();
            if (result.IsSuccess)
            {
                if (result.HasNewerVersion)
                {
                    Invoke(new Action(() => {
                        lblUpdate.Text = lang == "EN" ? $"New version available: {result.LatestVersion} (Click to download)" : $"Yeni sürüm mevcut: {result.LatestVersion} (İndirmek için tıklayın)";
                        lblUpdate.LinkArea = new LinkArea(0, lblUpdate.Text.Length);
                        lblUpdate.Visible = true;
                    }));
                }
                else
                {
                    Invoke(new Action(() => {
                        lblUpdate.Text = lang == "EN" ? "Your application is up to date." : "Uygulamanız güncel.";
                        lblUpdate.LinkArea = new LinkArea(0, 0);
                        lblUpdate.ForeColor = Color.Green;
                        lblUpdate.Visible = true;
                    }));
                }
            }
            else
            {
                Invoke(new Action(() => {
                    lblUpdate.Text = lang == "EN" ? "Version info could not be fetched." : "Sürüm bilgisi alınamadı.";
                    lblUpdate.LinkArea = new LinkArea(0, 0);
                    lblUpdate.ForeColor = Color.Gray;
                    lblUpdate.Visible = true;
                }));
            }
        }

        private void InitializeComponentUI()
        {
            this.Size = new Size(390, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            lblLanguage = new Label { Location = new Point(20, 15), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) };
            cbLanguage = new ComboBox { Location = new Point(20, 38), Width = 330, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            cbLanguage.Items.AddRange(new[] { "Türkçe (TR)", "English (EN)" });
            cbLanguage.SelectedIndex = (lang == "EN") ? 1 : 0;
            cbLanguage.SelectedIndexChanged += (s, e) => { 
                lang = cbLanguage.SelectedIndex == 1 ? "EN" : "TR"; 
                UpdateLanguage(); 
            };

            lblTheme = new Label { Location = new Point(20, 78), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) };
            cbTheme = new ComboBox { Location = new Point(20, 101), Width = 330, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            foreach (var tName in ThemeFactory.Themes.Keys)
            {
                cbTheme.Items.Add(tName);
            }
            cbTheme.SelectedIndexChanged += (s, e) => {
                string selectedThemeName = cbTheme.SelectedItem?.ToString() ?? "Modern Minimalist";
                ApplyTheme(selectedThemeName);
            };

            lblSource = new Label { Location = new Point(20, 141), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) };
            cbSource = new ComboBox { Location = new Point(20, 164), Width = 330, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            PopulateSources();
            cbSource.SelectedIndexChanged += CbSource_SelectedIndexChanged;

            lblCategory = new Label { Location = new Point(20, 204), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) };
            cbCategory = new ComboBox { Location = new Point(20, 227), Width = 330, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            
            chkStartup = new CheckBox { Location = new Point(20, 269), AutoSize = true, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9.5f) };
            chkStartup.CheckedChanged += (s, e) => {
                if (lang == "EN")
                {
                    chkStartup.Text = chkStartup.Checked ? "Run on Windows startup & close" : "Run on Windows startup & close (Off)";
                }
                else
                {
                    chkStartup.Text = chkStartup.Checked ? "Sistem açılışında çalıştır ve kapat" : "Sistem açılışında çalıştır ve kapat (Kapalı)";
                }
            };
            chkContextMenu = new CheckBox { Location = new Point(20, 299), AutoSize = true, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9.5f) };

            btnSave = new Button { 
                Location = new Point(20, 339), 
                Width = 330, 
                Height = 44, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            lblUpdate = new LinkLabel { Location = new Point(20, 400), AutoSize = true, Visible = false, Font = new Font("Segoe UI", 9.0f) };
            lblUpdate.LinkClicked += (s, e) => { Process.Start(new ProcessStartInfo("https://github.com/HaYToKoRaZ/HaYTooL-Wallpaper/releases") { UseShellExecute = true }); };

            lblWebsite = new LinkLabel { 
                Location = new Point(220, 400), 
                Width = 130, 
                TextAlign = ContentAlignment.MiddleRight, 
                Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            lblWebsite.LinkClicked += (s, e) => {
                Process.Start(new ProcessStartInfo("https://haytokoraz.github.io/HaYTooL-Wallpaper/") { UseShellExecute = true });
            };

            this.Controls.Add(lblLanguage);
            this.Controls.Add(cbLanguage);
            this.Controls.Add(lblTheme);
            this.Controls.Add(cbTheme);
            this.Controls.Add(lblSource);
            this.Controls.Add(cbSource);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cbCategory);
            this.Controls.Add(chkStartup);
            this.Controls.Add(chkContextMenu);
            this.Controls.Add(btnSave);
            this.Controls.Add(lblUpdate);
            this.Controls.Add(lblWebsite);
        }

        private void ApplyTheme(string themeName)
        {
            var theme = ThemeFactory.GetTheme(themeName);
            this.BackColor = theme.BackgroundColor;

            // Labels
            lblLanguage.ForeColor = theme.TextPrimary;
            lblTheme.ForeColor = theme.TextPrimary;
            lblSource.ForeColor = theme.TextPrimary;
            lblCategory.ForeColor = theme.TextPrimary;

            // CheckBoxes
            chkStartup.ForeColor = theme.TextPrimary;
            chkContextMenu.ForeColor = theme.TextPrimary;

            // Button
            btnSave.BackColor = theme.PrimaryColor;
            btnSave.ForeColor = theme.ButtonText;

            // Links
            lblWebsite.LinkColor = theme.AccentColor;
            lblWebsite.ActiveLinkColor = theme.PrimaryColor;
            lblUpdate.LinkColor = theme.AccentColor;
            lblUpdate.ActiveLinkColor = theme.PrimaryColor;
        }

        private void PopulateSources()
        {
            string currentSelected = cbSource?.SelectedItem?.ToString();
            cbSource?.Items.Clear();

            if (lang == "EN")
            {
                cbSource?.Items.AddRange(new[]
                {
                    "Wallhaven",
                    "Bing Daily Image",
                    "Picsum",
                    "Anime",
                    "Cats",
                    "Dogs",
                    "Foxes",
                    "NASA APOD",
                    "Unsplash Nature"
                });
            }
            else
            {
                cbSource?.Items.AddRange(new[]
                {
                    "Wallhaven",
                    "Bing Günün Manzarası",
                    "Picsum",
                    "Anime",
                    "Sevimli Kediler 🐱",
                    "Sadık Köpekler 🐶",
                    "Kurnaz Tilkiler 🦊",
                    "NASA APOD 🚀",
                    "Unsplash Doğa"
                });
            }

            // Önceki seçimi veya eşdeğerini koru
            if (!string.IsNullOrEmpty(currentSelected))
            {
                int matchedIndex = -1;
                for (int i = 0; i < cbSource.Items.Count; i++)
                {
                    string item = cbSource.Items[i].ToString();
                    if (item.Equals(currentSelected, StringComparison.OrdinalIgnoreCase) ||
                        (currentSelected.Contains("Cat") && item.Contains("Kedi")) ||
                        (currentSelected.Contains("Kedi") && item.Contains("Cat")) ||
                        (currentSelected.Contains("Dog") && item.Contains("Köpek")) ||
                        (currentSelected.Contains("Köpek") && item.Contains("Dog")) ||
                        (currentSelected.Contains("Fox") && item.Contains("Tilki")) ||
                        (currentSelected.Contains("Tilki") && item.Contains("Fox")) ||
                        (currentSelected.Contains("Bing") && item.Contains("Bing")) ||
                        (currentSelected.Contains("NASA") && item.Contains("NASA")) ||
                        (currentSelected.Contains("Unsplash") && item.Contains("Unsplash")))
                    {
                        matchedIndex = i;
                        break;
                    }
                }
                cbSource.SelectedIndex = matchedIndex >= 0 ? matchedIndex : 0;
            }
            else
            {
                cbSource.SelectedIndex = 0;
            }
        }

        private void UpdateLanguage()
        {
            if (lang == "EN")
            {
                this.Text = $"HaYTooL Wallpaper Settings {UpdateService.CurrentVersion}";
                lblLanguage.Text = "Language:";
                lblTheme.Text = "Color Theme (Theme Factory):";
                lblSource.Text = "Wallpaper Source:";
                lblCategory.Text = "Category (for Wallhaven):";
                chkStartup.Text = chkStartup.Checked ? "Run on Windows startup & close" : "Run on Windows startup & close (Off)";
                chkContextMenu.Text = "Add to Desktop right-click menu";
                btnSave.Text = "Save & Apply";
                lblWebsite.Text = "🌐 Web Site ↗";
            }
            else
            {
                this.Text = $"HaYTooL Wallpaper Ayarları {UpdateService.CurrentVersion}";
                lblLanguage.Text = "Dil Seçimi:";
                lblTheme.Text = "Renk Teması (Theme Factory):";
                lblSource.Text = "Duvar Kağıdı Kaynağı:";
                lblCategory.Text = "Kategori (Wallhaven için):";
                chkStartup.Text = chkStartup.Checked ? "Sistem açılışında çalıştır ve kapat" : "Sistem açılışında çalıştır ve kapat (Kapalı)";
                chkContextMenu.Text = "Masaüstü sağ tık menüsüne ekle";
                btnSave.Text = "Kaydet ve Uygula";
                lblWebsite.Text = "🌐 Web Sitesi ↗";
            }
            
            PopulateSources();
            CbSource_SelectedIndexChanged(null, null);
        }

        private void CbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSource.SelectedItem?.ToString() == "Wallhaven")
            {
                cbCategory.Enabled = true;
                if (cbCategory.Items.Count == 1 && (cbCategory.Items[0].ToString().StartsWith("Desteklenmiyor") || cbCategory.Items[0].ToString().StartsWith("Not supported") || cbCategory.Items[0].ToString().StartsWith("Bu kaynak")))
                {
                    cbCategory.Items.Clear();
                }
                
                if (cbCategory.Items.Count == 0)
                {
                    cbCategory.Items.AddRange(new[] { "Nature", "City", "Space", "Cars", "Cyberpunk", "Abstract" });
                    
                    string category = _settings.Category;
                    if (cbCategory.Items.Contains(category)) 
                        cbCategory.SelectedItem = category;
                    else 
                        cbCategory.SelectedIndex = 0;
                }
            }
            else
            {
                cbCategory.Items.Clear();
                cbCategory.Items.Add(lang == "EN" ? "Not supported for this source" : "Bu kaynak için desteklenmiyor");
                cbCategory.SelectedIndex = 0;
                cbCategory.Enabled = false;
            }
        }

        private void LoadSettings()
        {
            string themeName = _settings.Theme;
            if (cbTheme.Items.Contains(themeName))
                cbTheme.SelectedItem = themeName;
            else
                cbTheme.SelectedItem = "Modern Minimalist";

            ApplyTheme(cbTheme.SelectedItem?.ToString() ?? "Modern Minimalist");

            string source = _settings.Source;
            if (source == "Bing") source = lang == "EN" ? "Bing Daily Image" : "Bing Günün Manzarası";
            
            int matched = -1;
            for (int i = 0; i < cbSource.Items.Count; i++)
            {
                string item = cbSource.Items[i].ToString();
                if (item.Equals(source, StringComparison.OrdinalIgnoreCase) ||
                    (source.Contains("Cat") && item.Contains("Kedi")) ||
                    (source.Contains("Kedi") && item.Contains("Cat")) ||
                    (source.Contains("Dog") && item.Contains("Köpek")) ||
                    (source.Contains("Köpek") && item.Contains("Dog")) ||
                    (source.Contains("Fox") && item.Contains("Tilki")) ||
                    (source.Contains("Tilki") && item.Contains("Fox")) ||
                    (source.Contains("Bing") && item.Contains("Bing")) ||
                    (source.Contains("NASA") && item.Contains("NASA")) ||
                    (source.Contains("Unsplash") && item.Contains("Unsplash")))
                {
                    matched = i;
                    break;
                }
            }

            if (matched >= 0) 
                cbSource.SelectedIndex = matched;
            else 
                cbSource.SelectedIndex = 0;

            chkStartup.Checked = RegistryService.IsStartupEnabled();
            chkContextMenu.Checked = RegistryService.IsContextMenuEnabled();
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            string originalText = btnSave.Text;
            btnSave.Text = lang == "EN" ? "Saving..." : "Kaydediliyor...";

            try
            {
                _settings.Language = lang;
                _settings.Theme = cbTheme.SelectedItem?.ToString() ?? "Modern Minimalist";
                _settings.Source = cbSource.SelectedItem?.ToString() ?? "Wallhaven";
                
                if (cbSource.SelectedItem?.ToString() == "Wallhaven")
                {
                    _settings.Category = cbCategory.SelectedItem?.ToString() ?? "Nature";
                }

                RegistryService.SetStartup(chkStartup.Checked);
                RegistryService.SetContextMenu(chkContextMenu.Checked, lang);

                // Ayarlar kaydedildikten sonra yeni arka planı hemen uygula
                try
                {
                    await WallpaperManager.ExecuteAsync();
                }
                catch { }

                string successMsg = lang == "EN" ? "Settings saved!\nWallpaper applied successfully." : "Ayarlar kaydedildi!\nDuvar kağıdı başarıyla uygulandı.";
                string successTitle = lang == "EN" ? "Success" : "Başarılı";
                var currentTheme = ThemeFactory.GetTheme(cbTheme.SelectedItem?.ToString() ?? "Modern Minimalist");
                CustomMessageBox.Show(this, successTitle, successMsg, currentTheme, lang);
            }
            finally
            {
                btnSave.Text = originalText;
                btnSave.Enabled = true;
            }
        }
    }
}
