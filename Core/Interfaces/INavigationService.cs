namespace Core.Interfaces
{
    public interface INavigationService
    {
        void Show(string name);
        bool? ShowDialog(string name, object? parameter = null);
    }
}
