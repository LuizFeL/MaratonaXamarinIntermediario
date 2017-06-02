using System;
using System.Threading.Tasks;
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

                var loginBarItem = new ToolbarItem("Fazer Login", "", () => { ViewModel?.LogoutCommand?.Execute(null); }, ToolbarItemOrder.Primary);
                loginBarItem.SetBinding(MenuItem.IconProperty, new Binding("UserInfo.ImageUri", BindingMode.OneWay));
                loginBarItem.SetBinding(MenuItem.TextProperty, new Binding("UserInfo.Name", BindingMode.OneWay));
                ToolbarItems.Add(loginBarItem);

                ToolbarItems.Add(new ToolbarItem("Favoritos", "favorite_settings.png", () => { ViewModel?.FavoriteCommand?.Execute(null); }, ToolbarItemOrder.Primary));
                ToolbarItems.Add(new ToolbarItem("Preferências", "preferences.png", () => { ViewModel?.PreferencesCommand?.Execute(null); }, ToolbarItemOrder.Primary));

                Appearing += (sender, args) => Task.Run(async () =>
                {
                    await Task.Delay(500);
                    if (ViewModel == null || ViewModel.IsWorking) return;
                    ViewModel.IsBusy = false;
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }

}
