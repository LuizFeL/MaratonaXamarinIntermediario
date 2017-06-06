using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.View
{
    public partial class SocialLoginView
    {
        private SocialLoginViewModel ViewModel => BindingContext as SocialLoginViewModel;
        private readonly AzureClient _client;

        public SocialLoginView()
        {
            try
            {
                InitializeComponent();
                _client = App.AzureClient;
                SetInitialObjects();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public SocialLoginView(AzureClient client)
        {
            try
            {
                InitializeComponent();
                _client = client;
                SetInitialObjects();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void SetInitialObjects()
        {
            var toolBarItem = new ToolbarItem("Fazer Login", "", () => { }, ToolbarItemOrder.Primary);
            ToolbarItems.Add(toolBarItem);

            BindingContext = new SocialLoginViewModel(_client);
            ViewModel.IsBusy = false;
            ViewModel.ClearLoggedUser();
        }
    }
}
