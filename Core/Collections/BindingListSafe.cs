using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{
    public class BindingListSafe<T> : BindingList<T>
    {
        public BindingListSafe() : base() { }
        public BindingListSafe(IEnumerable<T> list) : base(new List<T>(list)) { }

        // Suspende eventos de ListChanged (protege a UI enquanto atualiza em lote)
        public void SuspendListChangedNotification() => this.RaiseListChangedEvents = false;

        // Retoma eventos e força um Reset (a UI irá redesenhar a lista inteira)
        public void ResumeListChangedNotification()
        {
            this.RaiseListChangedEvents = true;
            // força notificação de reset para a BindingSource/DataGridView
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
    }
}
