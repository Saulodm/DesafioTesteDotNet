using System.Data;

namespace DesafioDotNet
{
    public partial class PropertySelectionForm : Form
    {
        private readonly CheckedListBox _clb;
        private readonly Button _btnOk;
        private readonly Button _btnCancel;

        public IReadOnlyList<string>? SelectedProperties { get; private set; }

        public PropertySelectionForm(IEnumerable<string> availableProperties)
        {
            if (availableProperties == null) throw new ArgumentNullException(nameof(availableProperties));

            this.Text = "Selecionar colunas para exportação";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Width = 420;
            this.Height = 420;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            _clb = new CheckedListBox
            {
                Left = 12,
                Top = 12,
                Width = this.ClientSize.Width - 24,
                Height = this.ClientSize.Height - 80,
                CheckOnClick = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            foreach (var p in availableProperties)
                _clb.Items.Add(p, true); // por padrão marcado

            _btnOk = new Button
            {
                Text = "OK",
                Left = this.ClientSize.Width - 200,
                Top = this.ClientSize.Height - 56,
                Width = 80,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            _btnCancel = new Button
            {
                Text = "Cancelar",
                Left = this.ClientSize.Width - 110,
                Top = this.ClientSize.Height - 56,
                Width = 80,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            _btnOk.Click += (s, e) =>
            {
                SelectedProperties = _clb.CheckedItems.Cast<string>().ToList();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            _btnCancel.Click += (s, e) =>
            {
                SelectedProperties = null;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(_clb);
            this.Controls.Add(_btnOk);
            this.Controls.Add(_btnCancel);
        }
    }
}
