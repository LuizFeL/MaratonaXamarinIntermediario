using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsCentralizer.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteView
    {
        private FavoriteViewModel ViewModel => BindingContext as FavoriteViewModel;

        public FavoriteView()
        {
            try
            {
                InitializeComponent();
                BindingContext = BindingContext ?? new TopNewsViewModel(App.AzureClient);
                var toolBarItem = new ToolbarItem("Fazer Login", "", () => { ViewModel?.LogoutCommand?.Execute(null); }, ToolbarItemOrder.Primary);
                toolBarItem.SetBinding(MenuItem.IconProperty, new Binding("UserInfo.ImageUri", BindingMode.OneWay));
                toolBarItem.SetBinding(MenuItem.TextProperty, new Binding("UserInfo.Name", BindingMode.OneWay));
                ToolbarItems.Add(toolBarItem);
                
                ViewModel.IsBusy = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }

}
