using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewsCentralizer.View
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreferencesView
    {
        private PreferencesViewModel ViewModel => BindingContext as PreferencesViewModel;

        public PreferencesView()
        {
            try
            {
                InitializeComponent();

                BindingContext = BindingContext ?? new PreferencesViewModel(App.AzureClient);

                var toolBarItem = new ToolbarItem("User", "usericon", () => { ViewModel?.LogoutCommand?.Execute(null); }, ToolbarItemOrder.Primary);
                ToolbarItems.Add(toolBarItem);
                DisplayAlert("TESte", ViewModel.UserInfo.Image, "OK");
                ToolbarItems.Add(new ToolbarItem("Favoritos", "favorite_settings.png", () => { ViewModel?.FavoriteCommand?.Execute(null); }, ToolbarItemOrder.Primary));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }

}
