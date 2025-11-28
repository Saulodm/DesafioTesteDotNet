using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Core.Collections;
using Core.Interfaces;
using Core.Models;

namespace Core.ViewModels
{
    public class ListProductViewModel : BaseViewModel
    {
        private readonly IProductService _service;

        private readonly INavigationService _navigation;

        public BindingListSafe<Product> Products { get; } = new BindingListSafe<Product>();

        private Product? _selected;
        public Product? Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand EditCommand { get; }

        public ListProductViewModel(IProductService service, INavigationService navigation)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            AddCommand = new RelayCommand(() => NavigateAdd());
            RemoveCommand = new RelayCommand(() => Remove(), () => Selected != null);
            EditCommand = new RelayCommand(() => Edit(), () => Selected != null);

            Load();
        }

        public async Task Load()
        {
            // Suspende notificações para evitar InvalidOperationException se a UI estiver em edição
            Products.SuspendListChangedNotification();
            try
            {
                Products.Clear();
                foreach (var p in await _service.GetAll())
                    Products.Add(p);
            }
            finally
            {
                // Retoma notificações e força um ResetBindings na UI
                Products.ResumeListChangedNotification();
            }

            // opcional: notificar propriedade (a instância não mudou, mas é seguro)
            OnPropertyChanged(nameof(Products));
        }

        public void NavigateAdd()
        {
            var result = _navigation.ShowDialog("AddProduct");
            if (result != null && result == true)
                Load();
        }

       

        private void Remove()
        {
            if (Selected == null) return;
            _service.Remove(Selected.Id);
            Selected = null;
            Load();
        }
        private void Edit()
        {
            var result = _navigation.ShowDialog("AddProduct", Selected.Id);
            if (result != null && result == true)
                Load();
        }

        


    }
}
