using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Core.ViewModels
{
    public class AddProductViewModel : BaseViewModel
    {
        private readonly IProductService _service;
        public BindingList<string> Categories { get; } = new BindingList<string>();

        private string _name = string.Empty;
        private decimal _price = 0;
        private int _quantity = 0;
        private string _category = string.Empty;
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }
        public event EventHandler<RequestCloseEventArgs>? RequestClose;

        public RelayCommand AddCommand { get; }

        public RelayCommand CancelCommand { get; }
        public RelayCommand EditCommand { get; }

        public AddProductViewModel(IProductService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            AddCommand = new RelayCommand(() => Add(), () => CanAdd());
            CancelCommand = new RelayCommand(() => Cancel());
            EditCommand = new RelayCommand(() => Update(), () => CanAdd());

            Load();
        }

        private void Add()
        {
            _service.Add(Name.Trim(), Price, Category, Quantity);
            AddCommand.NotifyCanExecuteChanged();
            RequestClose?.Invoke(this, new RequestCloseEventArgs(true));
        }
        private void Update()
        {
            _service.Update(Name.Trim(), Price, Category, Quantity);
            AddCommand.NotifyCanExecuteChanged();
            RequestClose?.Invoke(this, new RequestCloseEventArgs(true));
        }
        public void Load(Guid? id = null)
        {
            if (id != null)
                LoadProduct(id.Value);
            Categories.Clear();
            foreach (var t in _service.GetCategories())
                Categories.Add(t);
            OnPropertyChanged(nameof(Categories));
        }

        private async Task LoadProduct(Guid id)
        {
            var product = await _service.GetById(id);
            Id = id;
            Name = product.Name;
            Price = product.Price;
            Quantity = product.StockQuantity;
            Category = Enum.GetName(typeof(Category), product.Category);
        }

        private void Cancel()
        {
            _service.SetInMemoryProductNull();
            RequestClose?.Invoke(this, new RequestCloseEventArgs(false));
        }

        private bool CanAdd()
        {
            return (!String.IsNullOrEmpty(Name) && Price > 0 && Quantity > 0 && !String.IsNullOrEmpty(Category));
        }

        public void RaiseCanExecute()
        {
            AddCommand.NotifyCanExecuteChanged();
        }

    }
}
