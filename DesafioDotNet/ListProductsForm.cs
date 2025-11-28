using Core.Models;
using Core.ViewModels;

namespace DesafioDotNet
{
    public partial class ListProductsForm : Form
    {
        private readonly ListProductViewModel _vm;
        private readonly BindingSource _bs = new();
        private Button buttonAdd = new();
        private Button buttonRemove = new();
        private Button buttonEdit = new();
        private DataGridView grid = new();

        public ListProductsForm(ListProductViewModel vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            InitializeComponent();

            _bs.DataSource = _vm.Products;
            grid.DataSource = _bs;

            //// Bind textbox to NewTitle (one-way: view -> viewmodel)
            //textBoxNew.DataBindings.Add(new Binding("Text", _vm, nameof(ListProductViewModel.Name), false, DataSourceUpdateMode.OnPropertyChanged));

            // selection binding
            grid.SelectionChanged += (s, e) =>
            {
                if (grid.CurrentRow?.DataBoundItem is Product item)
                    _vm.Selected = item;
                else
                    _vm.Selected = null;

                buttonRemove.Enabled = _vm.Selected != null;
                buttonEdit.Enabled = _vm.Selected != null;

            };

            buttonAdd.Click += (s, e) =>
            {
                if (_vm.AddCommand.CanExecute(null))
                    _vm.AddCommand.Execute(null);

            };

            buttonRemove.Click += (s, e) =>
            {
                if (_vm.RemoveCommand.CanExecute(null))
                    _vm.RemoveCommand.Execute(null);
                SafeEndEditAndReload();

            };
            buttonEdit.Click += (s, e) =>
            {
                if (_vm.EditCommand.CanExecute(null))
                    _vm.EditCommand.Execute(null);
                SafeEndEditAndReload();

            };
        }

        private void SafeEndEditAndReload()
        {
            try { this.Validate(); } catch { }
            try { grid.EndEdit(); } catch { }
            try { _bs.EndEdit(); } catch { }

            // Recarrega a coleção do ViewModel
            _vm.Load();

            // Atualiza o BindingSource para notificar a UI
            _bs.ResetBindings(false);
        }

        private void InitializeComponent()
        {
            grid = new DataGridView();
            buttonAdd = new Button();
            buttonRemove = new Button();
            buttonEdit = new Button();
            Title = new Label();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            // grid
            // 
            grid.Location = new Point(12, 120);
            grid.Name = "grid";
            grid.Size = new Size(660, 249);
            grid.TabIndex = 0;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(597, 91);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(75, 23);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Add";
            // 
            // buttonRemove
            // 
            buttonRemove.Location = new Point(516, 91);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(75, 23);
            buttonRemove.TabIndex = 3;
            buttonRemove.Text = "Remove";
            // 
            // buttonEdit
            // 
            buttonEdit.Location = new Point(435, 91);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(75, 23);
            buttonEdit.TabIndex = 5;
            buttonEdit.Text = "Edit";
            buttonEdit.UseVisualStyleBackColor = true;
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Segoe UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Title.Location = new Point(194, 9);
            Title.Name = "Title";
            Title.Size = new Size(261, 47);
            Title.TabIndex = 6;
            Title.Text = "List of products";
            Title.UseMnemonic = false;
            // 
            // ListProductsForm
            // 
            ClientSize = new Size(684, 381);
            Controls.Add(Title);
            Controls.Add(buttonEdit);
            Controls.Add(grid);
            Controls.Add(buttonAdd);
            Controls.Add(buttonRemove);
            Name = "ListProductsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DesafioDotNet";
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
