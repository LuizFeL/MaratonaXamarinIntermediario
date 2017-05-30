using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsCentralizer.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsView
    {
        private NewsViewModel ViewModel => BindingContext as NewsViewModel;

        public NewsView()
        {
            try
            {
                InitializeComponent();

                var favoriteToolBarItem = new ToolbarItem("Favorito", "", () => { ViewModel.FavoriteCommand.Execute(null); }, ToolbarItemOrder.Primary);
                favoriteToolBarItem.SetBinding(MenuItem.IconProperty, new Binding("FavoriteIcon", BindingMode.OneWay));
                ToolbarItems.Add(favoriteToolBarItem);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }

}
