using Aplication.Services.Export;
using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Microsoft.Extensions.Logging;

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

        private readonly ILogger<ListProductsForm> _logger;

        private readonly IProductExportService _exportService;

        public ListProductsForm(ListProductViewModel vm, IProductExportService exportService, ILogger<ListProductsForm> logger)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
            InitializeComponent();

            _bs.DataSource = _vm.Products;
            grid.DataSource = _bs;

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
            ExportButton.Click += async (s, e) =>
            {
                await ExportProductsAsync().ConfigureAwait(false);
            };


        }
        private async System.Threading.Tasks.Task ExportProductsAsync()
        {
            // Evita reentrada
            if (!ExportButton.Enabled) return;

            try
            {
                ExportButton.Enabled = false;

                // Recupera propriedades públicas de Product via reflection
                var props = typeof(Product)
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .Select(p => p.Name)
                    .ToArray();

                // Abre diálogo de seleção de propriedades
                using var dlg = new PropertySelectionForm(props);
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    _logger.LogInformation("Exportação cancelada pelo usuário.");
                    return;
                }

                var selected = dlg.SelectedProperties;
                if (selected == null || selected.Count == 0)
                {
                    MessageBox.Show(this, "Selecione pelo menos uma propriedade para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Abre SaveFileDialog para escolher caminho
                using var sfd = new SaveFileDialog
                {
                    Title = "Salvar CSV",
                    Filter = "CSV files (*.csv)|*.csv|Todos os arquivos (*.*)|*.*",
                    FileName = $"products_{DateTime.Now:yyyyMMddHHmmss}.csv",
                    DefaultExt = "csv",
                    AddExtension = true
                };

                if (sfd.ShowDialog(this) != DialogResult.OK)
                {
                    _logger.LogInformation("Exportação cancelada (SaveFileDialog).");
                    return;
                }

                var filePath = sfd.FileName;

                // Opcional: forçar finalização de edição antes de coletar dados
                SafeEndEditAndReload();

                // Chama o serviço de exportação (execução assíncrona)
                _logger.LogInformation("Iniciando exportação para {Path} com {Count} colunas", filePath, selected.Count);
                // ProductExportService já faz a iteração e gravação
                await _exportService.ExportProductsAsync(selected, filePath).ConfigureAwait(false);

                // Retornar para a thread UI para exibir mensagem
                this.Invoke(() =>
                {
                    MessageBox.Show(this, $"Exportação concluída: {filePath}", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao exportar produtos");
                this.Invoke(() =>
                {
                    MessageBox.Show(this, $"Erro ao exportar: {ex.Message}", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
            finally
            {
                // Garante reativação do botão na UI thread
                if (!this.IsDisposed)
                {
                    this.Invoke(() => ExportButton.Enabled = true);
                }
            }
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
            this.ExportButton = new Button();
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
            // ExportButton
            // 
            this.ExportButton.Location = new Point(12, 91);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new Size(75, 23);
            this.ExportButton.TabIndex = 7;
            this.ExportButton.Text = "Export CSV";
            this.ExportButton.UseVisualStyleBackColor = true;
            // 
            // ListProductsForm
            // 
            ClientSize = new Size(684, 381);
            Controls.Add(this.ExportButton);
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
