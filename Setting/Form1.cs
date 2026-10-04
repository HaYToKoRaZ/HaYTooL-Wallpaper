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
        private LinkLabel lblUpdate;

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
            this.Size = new Size(390, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(248, 249, 250);

            lblLanguage = new Label { Location = new Point(20, 15), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            cbLanguage = new ComboBox { Location = new Point(20, 38), Width = 330, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            cbLanguage.Items.AddRange(new[] { "Türkçe (TR)", "English (EN)" });
            cbLanguage.SelectedIndex = (lang == "EN") ? 1 : 0;
            cbLanguage.SelectedIndexChanged += (s, e) => { 
                lang = cbLanguage.SelectedIndex == 1 ? "EN" : "TR"; 
                UpdateLanguage(); 
            };

            lblSource = new Label { Location = new Point(20, 78), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            cbSource = new ComboBox { Location = new Point(20, 101), Width = 330, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            cbSource.Items.AddRange(new[] { "Wallhaven", "Bing günün manzarası", "Picsum", "Anime", "Cats", "Dogs" });
            cbSource.SelectedIndexChanged += CbSource_SelectedIndexChanged;

            lblCategory = new Label { Location = new Point(20, 141), AutoSize = true, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 41, 59) };
            cbCategory = new ComboBox { Location = new Point(20, 164), Width = 330, Height = 32, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            
            chkStartup = new CheckBox { Location = new Point(20, 206), AutoSize = true, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9.5f) };
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
            chkContextMenu = new CheckBox { Location = new Point(20, 236), AutoSize = true, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9.5f) };

            btnSave = new Button { 
                Location = new Point(20, 276), 
                Width = 330, 
                Height = 44, 
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(79, 70, 229),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            lblUpdate = new LinkLabel { Location = new Point(20, 335), AutoSize = true, Visible = false, Font = new Font("Segoe UI", 9.0f) };
            lblUpdate.LinkClicked += (s, e) => { Process.Start(new ProcessStartInfo("https://github.com/HaYToKoRaZ/HaYTooL-Wallpaper/releases") { UseShellExecute = true }); };

            this.Controls.Add(lblLanguage);
            this.Controls.Add(cbLanguage);
            this.Controls.Add(lblSource);
            this.Controls.Add(cbSource);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cbCategory);
            this.Controls.Add(chkStartup);
            this.Controls.Add(chkContextMenu);
            this.Controls.Add(btnSave);
            this.Controls.Add(lblUpdate);
        }

        private void UpdateLanguage()
        {
            if (lang == "EN")
            {
                this.Text = $"HaYTooL Wallpaper Settings {UpdateService.CurrentVersion}";
                lblLanguage.Text = "Language:";
                lblSource.Text = "Wallpaper Source:";
                lblCategory.Text = "Category (for Wallhaven):";
                chkStartup.Text = chkStartup.Checked ? "Run on Windows startup & close" : "Run on Windows startup & close (Off)";
                chkContextMenu.Text = "Add to Desktop right-click menu";
                btnSave.Text = "Save & Apply";
            }
            else
            {
                this.Text = $"HaYTooL Wallpaper Ayarları {UpdateService.CurrentVersion}";
                lblLanguage.Text = "Dil Seçimi:";
                lblSource.Text = "Duvar Kağıdı Kaynağı:";
                lblCategory.Text = "Kategori (Wallhaven için):";
                chkStartup.Text = chkStartup.Checked ? "Sistem açılışında çalıştır ve kapat" : "Sistem açılışında çalıştır ve kapat (Kapalı)";
                chkContextMenu.Text = "Masaüstü sağ tık menüsüne ekle";
                btnSave.Text = "Kaydet ve Uygula";
            }
            
            // Re-trigger source change to update category box language if disabled
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
            string source = _settings.Source;
            if (source == "Bing") source = "Bing günün manzarası";
            
            if (cbSource.Items.Contains(source)) 
                cbSource.SelectedItem = source;
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

                string successMsg = lang == "EN" ? "Settings saved! Wallpaper applied successfully." : "Ayarlar kaydedildi! Duvar kağıdı başarıyla uygulandı.";
                string successTitle = lang == "EN" ? "Success" : "Başarılı";
                MessageBox.Show(successMsg, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                btnSave.Text = originalText;
                btnSave.Enabled = true;
            }
        }
    }
}
