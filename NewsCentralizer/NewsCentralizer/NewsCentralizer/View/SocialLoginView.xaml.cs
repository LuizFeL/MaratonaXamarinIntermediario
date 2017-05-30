using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.View
{
    public partial class SocialLoginView
    {
        private SocialLoginViewModel ViewModel => BindingContext as SocialLoginViewModel;

        public SocialLoginView()
        {
            try
            {
                InitializeComponent();

                var toolBarItem = new ToolbarItem("Fazer Login", "", () => { }, ToolbarItemOrder.Primary);
                toolBarItem.SetBinding(MenuItem.IconProperty, new Binding("UserInfo.ImageUri", BindingMode.OneWay));
                toolBarItem.SetBinding(MenuItem.TextProperty, new Binding("UserInfo.Name", BindingMode.OneWay));
                ToolbarItems.Add(toolBarItem);

                var service = DependencyService.Get<IFelApiService>();
                BindingContext = new SocialLoginViewModel(service);
                ViewModel.IsBusy = false;
                ViewModel.ClearLoggedUser();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
