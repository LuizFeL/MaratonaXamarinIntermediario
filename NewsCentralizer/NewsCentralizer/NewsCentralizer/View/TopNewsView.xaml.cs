using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsCentralizer.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopNewsView
    {
        private TopNewsViewModel ViewModel => BindingContext as TopNewsViewModel;

        public TopNewsView()
        {
            try
            {
                InitializeComponent();
                BindingContext = BindingContext ?? new TopNewsViewModel(App.AzureClient);
                var toolBarItem = new ToolbarItem("Fazer Login", "", () => { }, ToolbarItemOrder.Primary);
                toolBarItem.SetBinding(MenuItem.IconProperty, new Binding("UserInfo.ImageUri", BindingMode.OneWay));
                toolBarItem.SetBinding(MenuItem.TextProperty, new Binding("UserInfo.Name", BindingMode.OneWay));
                ToolbarItems.Add(toolBarItem);

                var toolBarItem2 = new ToolbarItem("Favoritos", "favorite.png", () => { ViewModel.FavoriteCommand.Execute(null); }, ToolbarItemOrder.Primary);
                ToolbarItems.Add(toolBarItem2);

                ViewModel.IsBusy = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }

}
