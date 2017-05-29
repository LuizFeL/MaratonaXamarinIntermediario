using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NewsCentralizer.Model
{
    public class ObservableBaseObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (string.IsNullOrWhiteSpace(name) || PropertyChanged == null) return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
