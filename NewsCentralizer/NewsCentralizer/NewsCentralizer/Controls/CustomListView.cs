using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NewsCentralizer.Controls
{
    public class CustomListView : ListView
    {
        public static readonly BindableProperty ItemTappedCommandProperty =
            BindableProperty.Create(
                "ItemTappedCommand",
                typeof(ICommand),
                typeof(CustomListView));

        public ICommand ItemTappedCommand { get; set; }

        public CustomListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            Initialize();
        }

        public CustomListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            Initialize();
        }

        public void Initialize()
        {
            ItemSelected += (sender, e) =>
            {
                if (ItemTappedCommand == null) return;
                if (ItemTappedCommand.CanExecute(e.SelectedItem)) ItemTappedCommand.Execute(e.SelectedItem);
            };
        }
    }


}
