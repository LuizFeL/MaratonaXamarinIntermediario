using System;
using NewsCentralizer.Model;
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
                SetToollbarItems();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void SetToollbarItems()
        {
            var favoriteToolBarItem = new ToolbarItem("Favorito", "", () => { ViewModel.FavoriteCommand.Execute(null); }, ToolbarItemOrder.Primary);
            favoriteToolBarItem.SetBinding(MenuItem.IconProperty, new Binding("FavoriteIcon", BindingMode.OneWay));
            ToolbarItems.Add(favoriteToolBarItem);
        }

        public NewsView(NewsModel model)
        {
            try
            {
                InitializeComponent();
                BindingContext = new NewsViewModel(App.AzureClient, model);
                SetToollbarItems();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (App.NewsNotification == null || Navigation.NavigationStack.Count > 1)
                return base.OnBackButtonPressed();

            App.NewsNotification = null;
            App.SetMainPage();
            return true;
        }

    }

}
