using System.Globalization;
using System.Xml.Linq;
using Core.ViewModels;

namespace DesafioDotNet
{
    public partial class AddProductForm : Form
    {
        private readonly AddProductViewModel _vm;
        private readonly BindingSource _bsc = new();
        private readonly BindingSource _bs = new();

        private string _previousPrice = "";
        public AddProductForm(AddProductViewModel vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            InitializeComponent();

            // BindingSource points to ViewModel (not model)
            _bs.DataSource = _vm;

            // Bindings: View control <-> ViewModel property
            NameTxt.DataBindings.Add("Text", _bs, nameof(AddProductViewModel.Name), false, DataSourceUpdateMode.OnPropertyChanged);

            // NumericUpDown works with decimal; Price is decimal
            Pricetxt.DecimalPlaces = 2;
            Pricetxt.Minimum = 0;
            Pricetxt.Maximum = 1000000;
            Pricetxt.DataBindings.Add("Value", _bs, nameof(AddProductViewModel.Price), true, DataSourceUpdateMode.OnPropertyChanged);

            // Category ComboBox: DataSource + SelectedItem binding
            CategoryCb.DataSource = _vm.Categories;
            CategoryCb.DropDownStyle = ComboBoxStyle.DropDownList;
            CategoryCb.DataBindings.Add("SelectedItem", _bs, nameof(AddProductViewModel.Category), true, DataSourceUpdateMode.OnPropertyChanged);

            // Quantity as integer using NumericUpDown (Value is decimal but will convert)
            StockQuantitytxt.DecimalPlaces = 0;
            StockQuantitytxt.Minimum = 0;
            StockQuantitytxt.Maximum = 100000;
            StockQuantitytxt.DataBindings.Add("Value", _bs, nameof(AddProductViewModel.Quantity), true, DataSourceUpdateMode.OnPropertyChanged);

            // Wire buttons to ViewModel commands (View triggers the command)
            AddButton.Click += (s, e) =>
            {
                if (_vm.AddCommand.CanExecute(null))
                {
                    _vm.AddCommand.Execute(null);
                }
            };

            CancelButton.Click += (s, e) =>
            {
                if (_vm.CancelCommand.CanExecute(null))
                {
                    _vm.CancelCommand.Execute(null);
                }
            };
            EditButton.Click += (s, e) =>
            {
                _bs.EndEdit();
                if (_vm.EditCommand.CanExecute(null))
                {
                    _vm.EditCommand.Execute(null);
                }
            };

            // Close the form when VM requests close
            _vm.RequestClose += (s, ev) =>
            {
                this.DialogResult = ev.Saved ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            };
            // Keep Save button enabled state in sync with command CanExecute by listening to property changes
            _vm.PropertyChanged += (s, e) =>
            {
                // re-evaluate can execute when relevant props change
                if (e.PropertyName == nameof(AddProductViewModel.Name) ||
                    e.PropertyName == nameof(AddProductViewModel.Price) ||
                    e.PropertyName == nameof(AddProductViewModel.Quantity))
                {
                    AddButton.Enabled = _vm.AddCommand.CanExecute(null);
                }
            };

            // initialize button enabled state
            AddButton.Enabled = _vm.AddCommand.CanExecute(null); _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            
        }
        public void Initialize(Guid id)
        {
            Title.Text = "Edit Product";
            AddButton.Visible = false;
            EditButton.Visible = true;
            _vm.Load(id);
        }

        private void InitializeComponent()
        {
            Title = new Label();
            NameTxt = new TextBox();
            Namelbl = new Label();
            Pricelbl = new Label();
            Categoryllbl = new Label();
            StockQuantitylbl = new Label();
            CategoryCb = new ComboBox();
            AddButton = new Button();
            CancelButton = new Button();
            StockQuantitytxt = new NumericUpDown();
            Pricetxt = new NumericUpDown();
            EditButton = new Button();
            ((System.ComponentModel.ISupportInitialize)StockQuantitytxt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Pricetxt).BeginInit();
            SuspendLayout();
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Segoe UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Title.Location = new Point(247, 9);
            Title.Name = "Title";
            Title.Size = new Size(217, 47);
            Title.TabIndex = 0;
            Title.Text = "Add Product";
            // 
            // NameTxt
            // 
            NameTxt.Location = new Point(92, 115);
            NameTxt.Name = "NameTxt";
            NameTxt.Size = new Size(100, 23);
            NameTxt.TabIndex = 1;
            // 
            // Namelbl
            // 
            Namelbl.AutoSize = true;
            Namelbl.Location = new Point(91, 98);
            Namelbl.Name = "Namelbl";
            Namelbl.Size = new Size(39, 15);
            Namelbl.TabIndex = 2;
            Namelbl.Text = "Name";
            // 
            // Pricelbl
            // 
            Pricelbl.AutoSize = true;
            Pricelbl.Location = new Point(229, 98);
            Pricelbl.Name = "Pricelbl";
            Pricelbl.Size = new Size(33, 15);
            Pricelbl.TabIndex = 4;
            Pricelbl.Text = "Price";
            // 
            // Categoryllbl
            // 
            Categoryllbl.AutoSize = true;
            Categoryllbl.Location = new Point(365, 98);
            Categoryllbl.Name = "Categoryllbl";
            Categoryllbl.Size = new Size(55, 15);
            Categoryllbl.TabIndex = 6;
            Categoryllbl.Text = "Category";
            // 
            // StockQuantitylbl
            // 
            StockQuantitylbl.AutoSize = true;
            StockQuantitylbl.Location = new Point(507, 98);
            StockQuantitylbl.Name = "StockQuantitylbl";
            StockQuantitylbl.Size = new Size(85, 15);
            StockQuantitylbl.TabIndex = 8;
            StockQuantitylbl.Text = "Stock Quantity";
            // 
            // CategoryCb
            // 
            CategoryCb.FormattingEnabled = true;
            CategoryCb.Location = new Point(365, 116);
            CategoryCb.Name = "CategoryCb";
            CategoryCb.Size = new Size(121, 23);
            CategoryCb.TabIndex = 9;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(606, 188);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(75, 23);
            AddButton.TabIndex = 10;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(517, 188);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(75, 23);
            CancelButton.TabIndex = 11;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            // 
            // StockQuantitytxt
            // 
            StockQuantitytxt.Location = new Point(507, 116);
            StockQuantitytxt.Name = "StockQuantitytxt";
            StockQuantitytxt.Size = new Size(120, 23);
            StockQuantitytxt.TabIndex = 12;
            // 
            // Pricetxt
            // 
            Pricetxt.Location = new Point(229, 117);
            Pricetxt.Name = "Pricetxt";
            Pricetxt.Size = new Size(120, 23);
            Pricetxt.TabIndex = 13;
            // 
            // EditButton
            // 
            EditButton.Location = new Point(606, 188);
            EditButton.Name = "EditButton";
            EditButton.Size = new Size(75, 23);
            EditButton.TabIndex = 14;
            EditButton.Text = "Save";
            EditButton.UseVisualStyleBackColor = true;
            EditButton.Visible = false;
            // 
            // AddProductForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(768, 222);
            Controls.Add(EditButton);
            Controls.Add(Pricetxt);
            Controls.Add(StockQuantitytxt);
            Controls.Add(CancelButton);
            Controls.Add(AddButton);
            Controls.Add(CategoryCb);
            Controls.Add(StockQuantitylbl);
            Controls.Add(Categoryllbl);
            Controls.Add(Pricelbl);
            Controls.Add(Namelbl);
            Controls.Add(NameTxt);
            Controls.Add(Title);
            Name = "AddProductForm";
            Text = "AddProduct";
            ((System.ComponentModel.ISupportInitialize)StockQuantitytxt).EndInit();
            ((System.ComponentModel.ISupportInitialize)Pricetxt).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label Title;
        private TextBox NameTxt;
        private Label Namelbl;
        private Label Pricelbl;
        private Label Categoryllbl;
        private Label StockQuantitylbl;
        private ComboBox CategoryCb;
        private Button AddButton;
        private Button CancelButton;
        private Button EditButton;

    }
}
