// DVLD — Luxury Dark + Gold Theme
// Windows Forms theme helper. Drop into any WinForms project (net6.0-windows or .NET Framework).
// Usage:
//   DvldTheme.Apply(this);                 // apply to a Form (recursively)
//   btn.BackColor = DvldTheme.Gold;        // use tokens directly
//   lbl.Font      = DvldTheme.DisplayFont(28);
//
// Color values mirror the web design tokens (oklch → sRGB approximations).

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Dvld.Theme
{
    public static class DvldTheme
    {
        // ---------- Core palette ----------
        public static readonly Color Background     = FromHex("#1B1712"); // app bg
        public static readonly Color Foreground     = FromHex("#F1EADD"); // text
        public static readonly Color Card           = FromHex("#251F19"); // panels
        public static readonly Color CardElevated   = FromHex("#2C251E");
        public static readonly Color Muted          = FromHex("#2B241D");
        public static readonly Color MutedForeground= FromHex("#A99B85");
        public static readonly Color Border         = FromHex("#3A322A");
        public static readonly Color Input          = FromHex("#231D17");

        // Gold family
        public static readonly Color Gold           = FromHex("#D4AF5A"); // primary
        public static readonly Color GoldSoft       = FromHex("#E8CC85");
        public static readonly Color GoldDeep       = FromHex("#9C7A34");

        // Status
        public static readonly Color Success        = FromHex("#5FBF7A");
        public static readonly Color Destructive    = FromHex("#C74A3B");

        // ---------- Fonts ----------
        // Cormorant Garamond / Playfair Display are not installed on stock Windows.
        // We fall back to Georgia for the display face, Segoe UI for the sans face.
        private const string DisplayFamily = "Cormorant Garamond";
        private const string DisplayFallback = "Georgia";
        private const string SansFamily = "Segoe UI";

        public static Font DisplayFont(float size, FontStyle style = FontStyle.Regular)
            => new Font(FontInstalled(DisplayFamily) ? DisplayFamily : DisplayFallback,
                        size, style, GraphicsUnit.Point);

        public static Font SansFont(float size, FontStyle style = FontStyle.Regular)
            => new Font(SansFamily, size, style, GraphicsUnit.Point);

        // ---------- Public API ----------
        public static void Apply(Form form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            form.BackColor = Background;
            form.ForeColor = Foreground;
            form.Font = SansFont(9.75f);
            ApplyToChildren(form);
        }

        private static void ApplyToChildren(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                StyleControl(c);
                if (c.HasChildren) ApplyToChildren(c);
            }
        }

        private static void StyleControl(Control c)
        {
            switch (c)
            {
                case Button b:
                    StyleButton(b);
                    break;
                case TextBox tb:
                    tb.BackColor = Input;
                    tb.ForeColor = Foreground;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case Label lbl:
                    lbl.ForeColor = lbl.Tag as string == "gold" ? Gold : Foreground;
                    lbl.BackColor = Color.Transparent;
                    break;
                case Panel p:
                    p.BackColor = Card;
                    p.ForeColor = Foreground;
                    break;
                case GroupBox gb:
                    gb.ForeColor = GoldSoft;
                    gb.BackColor = Card;
                    break;
                case ListView lv:
                    lv.BackColor = Card;
                    lv.ForeColor = Foreground;
                    lv.BorderStyle = BorderStyle.FixedSingle;
                    lv.GridLines = false;
                    break;
                case DataGridView dg:
                    StyleGrid(dg);
                    break;
                case MenuStrip ms:
                    ms.BackColor = Background;
                    ms.ForeColor = Foreground;
                    ms.Renderer = new ToolStripProfessionalRenderer(new DvldColorTable());
                    break;
                case StatusStrip ss:
                    ss.BackColor = Card;
                    ss.ForeColor = MutedForeground;
                    break;
                case ToolStrip ts:
                    ts.Renderer = new ToolStripProfessionalRenderer(new DvldColorTable());
                    break;
                case TabControl tc:
                    tc.DrawMode = TabDrawMode.OwnerDrawFixed;
                    tc.DrawItem += Tab_DrawItem;
                    break;
            }
        }

        // ---------- Styled controls ----------
        public static void StyleButton(Button b, bool primary = false)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.Cursor = Cursors.Hand;
            b.UseCompatibleTextRendering = false;
            b.Font = SansFont(9.5f, FontStyle.Bold);

            if (primary || (b.Tag as string) == "primary")
            {
                b.BackColor = Gold;
                b.ForeColor = FromHex("#1F1A12");
                b.FlatAppearance.BorderColor = GoldDeep;
                b.FlatAppearance.MouseOverBackColor = GoldSoft;
                b.FlatAppearance.MouseDownBackColor = GoldDeep;
            }
            else
            {
                b.BackColor = Card;
                b.ForeColor = Gold;
                b.FlatAppearance.BorderColor = WithAlpha(Gold, 90);
                b.FlatAppearance.MouseOverBackColor = FromHex("#312820");
                b.FlatAppearance.MouseDownBackColor = FromHex("#3A2F25");
            }
        }

        private static void StyleGrid(DataGridView dg)
        {
            dg.EnableHeadersVisualStyles = false;
            dg.BackgroundColor = Background;
            dg.GridColor = Border;
            dg.BorderStyle = BorderStyle.None;
            dg.RowHeadersVisible = false;

            dg.DefaultCellStyle.BackColor = Card;
            dg.DefaultCellStyle.ForeColor = Foreground;
            dg.DefaultCellStyle.SelectionBackColor = WithAlpha(Gold, 60);
            dg.DefaultCellStyle.SelectionForeColor = Foreground;
            dg.DefaultCellStyle.Font = SansFont(9.5f);

            dg.AlternatingRowsDefaultCellStyle.BackColor = CardElevated;

            dg.ColumnHeadersDefaultCellStyle.BackColor = FromHex("#1F1A14");
            dg.ColumnHeadersDefaultCellStyle.ForeColor = GoldSoft;
            dg.ColumnHeadersDefaultCellStyle.Font = SansFont(9.5f, FontStyle.Bold);
            dg.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dg.ColumnHeadersHeight = 34;
        }

        private static void Tab_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tc) return;
            var page = tc.TabPages[e.Index];
            bool selected = e.State == DrawItemState.Selected;
            using var bg = new SolidBrush(selected ? Card : Background);
            e.Graphics.FillRectangle(bg, e.Bounds);
            using var fg = new SolidBrush(selected ? Gold : MutedForeground);
            var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(e.Graphics, page.Text, SansFont(9.5f, FontStyle.Bold),
                e.Bounds, selected ? Gold : MutedForeground, flags);
        }

        // ---------- Custom paint helpers ----------
        /// <summary>
        /// Draws a luxury card background (radial gold highlight + hairline border).
        /// Call from a Panel's Paint handler.
        /// </summary>
        public static void PaintLuxeSurface(Graphics g, Rectangle bounds, int radius = 14)
        {
            using var path = RoundedRect(bounds, radius);
            using var fill = new LinearGradientBrush(bounds, CardElevated, Card, 90f);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(fill, path);

            using var pen = new Pen(WithAlpha(Gold, 60), 1f);
            g.DrawPath(pen, path);
        }

        public static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ---------- Utils ----------
        public static Color FromHex(string hex)
        {
            hex = hex.TrimStart('#');
            return Color.FromArgb(
                Convert.ToInt32(hex.Substring(0, 2), 16),
                Convert.ToInt32(hex.Substring(2, 2), 16),
                Convert.ToInt32(hex.Substring(4, 2), 16));
        }

        public static Color WithAlpha(Color c, int alpha) =>
            Color.FromArgb(Math.Clamp(alpha, 0, 255), c);

        private static bool FontInstalled(string family)
        {
            try
            {
                using var f = new Font(family, 10f);
                return f.Name.Equals(family, StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }
    }

    // Colors for MenuStrip / ToolStrip
    internal sealed class DvldColorTable : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => DvldTheme.Background;
        public override Color MenuStripGradientEnd   => DvldTheme.Background;
        public override Color MenuItemSelected       => DvldTheme.WithAlpha(DvldTheme.Gold, 40);
        public override Color MenuItemSelectedGradientBegin => DvldTheme.WithAlpha(DvldTheme.Gold, 40);
        public override Color MenuItemSelectedGradientEnd   => DvldTheme.WithAlpha(DvldTheme.Gold, 40);
        public override Color MenuItemBorder         => DvldTheme.WithAlpha(DvldTheme.Gold, 90);
        public override Color MenuItemPressedGradientBegin => DvldTheme.Card;
        public override Color MenuItemPressedGradientEnd   => DvldTheme.Card;
        public override Color ToolStripDropDownBackground  => DvldTheme.Card;
        public override Color ImageMarginGradientBegin => DvldTheme.Card;
        public override Color ImageMarginGradientMiddle => DvldTheme.Card;
        public override Color ImageMarginGradientEnd => DvldTheme.Card;
        public override Color SeparatorDark => DvldTheme.Border;
        public override Color SeparatorLight => DvldTheme.Border;
    }
}
