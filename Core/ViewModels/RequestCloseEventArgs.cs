namespace Core.ViewModels
{
    public class RequestCloseEventArgs : EventArgs
    {
        public bool Saved { get; }
        public RequestCloseEventArgs(bool saved) => Saved = saved;
    }
}
