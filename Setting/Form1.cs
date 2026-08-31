using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using Shared;

namespace Setting
{
    public partial class Form1 : Form
    {
        private ComboBox cbSource;
        private ComboBox cbCategory;
        private CheckBox chkStartup;
        private Button btnSave;
        private Label lblSource;
        private Label lblCategory;
        private ComboBox cbLanguage;
        private Label lblLanguage;

        private IniHelper ini;
        private string iniPath;
        private string appName = "HaYTooL-Wallpaper";

        // Language dictionaries
        private string lang = "TR"; // Default TR

        public Form1()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            iniPath = Path.Combine(exePath, "settings.ini");
            ini = new IniHelper(iniPath);
            lang = ini.Read("Language", "Settings", "TR");

            InitializeComponentUI();
            LoadSettings();
            UpdateLanguage();
        }

        private void InitializeComponentUI()
        {
            this.Size = new Size(350, 310);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblLanguage = new Label { Location = new Point(20, 15), AutoSize = true };
            cbLanguage = new ComboBox { Location = new Point(20, 35), Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
            cbLanguage.Items.AddRange(new[] { "Türkçe (TR)", "English (EN)" });
            cbLanguage.SelectedIndex = (lang == "EN") ? 1 : 0;
            cbLanguage.SelectedIndexChanged += (s, e) => { 
                lang = cbLanguage.SelectedIndex == 1 ? "EN" : "TR"; 
                UpdateLanguage(); 
            };

            lblSource = new Label { Location = new Point(20, 70), AutoSize = true };
            cbSource = new ComboBox { Location = new Point(20, 90), Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
            cbSource.Items.AddRange(new[] { "Wallhaven", "Bing günün manzarası", "Picsum", "Anime", "Cats", "Dogs" });
            cbSource.SelectedIndexChanged += CbSource_SelectedIndexChanged;

            lblCategory = new Label { Location = new Point(20, 125), AutoSize = true };
            cbCategory = new ComboBox { Location = new Point(20, 145), Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
            
            chkStartup = new CheckBox { Location = new Point(20, 185), AutoSize = true };

            btnSave = new Button { Location = new Point(20, 215), Width = 290, Height = 35 };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblLanguage);
            this.Controls.Add(cbLanguage);
            this.Controls.Add(lblSource);
            this.Controls.Add(cbSource);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cbCategory);
            this.Controls.Add(chkStartup);
            this.Controls.Add(btnSave);
        }

        private void UpdateLanguage()
        {
            if (lang == "EN")
            {
                this.Text = "HaYTooL Wallpaper Settings v1.0.0";
                lblLanguage.Text = "Language:";
                lblSource.Text = "Wallpaper Source:";
                lblCategory.Text = "Category (for Wallhaven):";
                chkStartup.Text = "Run on Windows startup";
                btnSave.Text = "Save & Apply";
            }
            else
            {
                this.Text = "HaYTooL Wallpaper Ayarları v1.0.0";
                lblLanguage.Text = "Dil Seçimi:";
                lblSource.Text = "Duvar Kağıdı Kaynağı:";
                lblCategory.Text = "Kategori (Wallhaven için):";
                chkStartup.Text = "Sistem açılışında çalıştır";
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
                    
                    // Reload category setting if it exists in valid items
                    string category = ini.Read("Category", "Settings", "Nature");
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
            string source = ini.Read("Source", "Settings", "Wallhaven");
            if (source == "Bing") source = "Bing günün manzarası";
            
            if (cbSource.Items.Contains(source)) 
                cbSource.SelectedItem = source;
            else 
                cbSource.SelectedIndex = 0;

            // Registry check
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (key != null)
                {
                    chkStartup.Checked = (key.GetValue(appName) != null);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ini.Write("Language", lang, "Settings");
            ini.Write("Source", cbSource.SelectedItem?.ToString() ?? "Wallhaven", "Settings");
            
            if (cbSource.SelectedItem?.ToString() == "Wallhaven")
            {
                ini.Write("Category", cbCategory.SelectedItem?.ToString() ?? "Nature", "Settings");
            }

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (chkStartup.Checked)
                    {
                        string targetExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HaYTooL-Wallpaper.exe");
                        key.SetValue(appName, $"\"{targetExe}\"");
                    }
                    else
                    {
                        key.DeleteValue(appName, false);
                    }
                }
                
                string targetExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HaYTooL-Wallpaper.exe");
                if (File.Exists(targetExePath))
                {
                    Process.Start(targetExePath);
                }

                string msg = lang == "EN" ? "Settings saved and wallpaper is updating!" : "Ayarlar kaydedildi ve duvar kağıdı güncelleniyor!";
                string title = lang == "EN" ? "Success" : "Başarılı";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string msg = lang == "EN" ? "Error writing to registry: " : "Kayıt defterine yazılırken hata oluştu: ";
                MessageBox.Show(msg + ex.Message, lang == "EN" ? "Error" : "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
