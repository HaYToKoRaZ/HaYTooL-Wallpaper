using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Setting.Services
{
    public static class CustomMessageBox
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public static DialogResult Show(IWin32Window owner, string title, string message, AppTheme theme, string lang = "TR")
        {
            using var form = new Form();
            form.FormBorderStyle = FormBorderStyle.None;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowInTaskbar = false;
            form.Size = new Size(380, 240);
            form.BackColor = theme.BackgroundColor;

            // Form kenarlarına gölge / zarif çizgi efekti
            form.Paint += (s, e) =>
            {
                using var pen = new Pen(theme.AccentColor, 2);
                e.Graphics.DrawRectangle(pen, 1, 1, form.Width - 2, form.Height - 2);
            };

            // Başlık Çubuğu Paneli
            var titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = theme.SurfaceColor
            };

            var lblHeader = new Label
            {
                Text = title,
                ForeColor = theme.TextPrimary,
                Font = new Font(theme.FontFamily, 10.5f, FontStyle.Bold),
                Location = new Point(16, 12),
                AutoSize = true
            };

            var btnClose = new Button
            {
                Text = "✕",
                ForeColor = theme.TextSecondary,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Size = new Size(32, 32),
                Location = new Point(form.Width - 40, 6),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 239, 68, 68);
            btnClose.Click += (s, e) => { form.DialogResult = DialogResult.OK; form.Close(); };

            titlePanel.Controls.Add(lblHeader);
            titlePanel.Controls.Add(btnClose);

            // Başlık çubuğundan sürükleme desteği
            titlePanel.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(form.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };

            // İkon & Mesaj Paneli
            var contentPanel = new Panel
            {
                Location = new Point(20, 56),
                Size = new Size(340, 115),
                BackColor = Color.Transparent
            };

            var lblIcon = new Label
            {
                Text = "✨",
                Font = new Font("Segoe UI Emoji", 26f),
                ForeColor = theme.PrimaryColor,
                Location = new Point(4, 18),
                Size = new Size(54, 54),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblMsg = new Label
            {
                Text = message,
                ForeColor = theme.TextPrimary,
                Font = new Font(theme.FontFamily, 9.75f, FontStyle.Regular),
                Location = new Point(62, 16),
                Size = new Size(270, 85),
                TextAlign = ContentAlignment.MiddleLeft
            };

            contentPanel.Controls.Add(lblIcon);
            contentPanel.Controls.Add(lblMsg);

            // Onay Butonu
            var btnOk = new Button
            {
                Text = lang == "EN" ? "Awesome!" : "Tamam",
                Location = new Point(form.Width / 2 - 65, 180),
                Size = new Size(130, 38),
                BackColor = theme.PrimaryColor,
                ForeColor = theme.ButtonText,
                Font = new Font(theme.FontFamily, 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += (s, e) => { form.DialogResult = DialogResult.OK; form.Close(); };

            form.Controls.Add(btnOk);
            form.Controls.Add(contentPanel);
            form.Controls.Add(titlePanel);

            form.AcceptButton = btnOk;
            return form.ShowDialog(owner);
        }
    }
}
