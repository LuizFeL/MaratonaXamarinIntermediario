﻿using System;
using NewsCentralizer.ViewModel;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.View
{
    public partial class SocialLoginView
    {
        private SocialLoginViewModel ViewModel => BindingContext as SocialLoginViewModel;
        private readonly AzureClient _client;

        public SocialLoginView(AzureClient client)
        {
            try
            {
                InitializeComponent();

                _client = client;

                var toolBarItem = new ToolbarItem("Fazer Login", "", () => { }, ToolbarItemOrder.Primary);
                toolBarItem.SetBinding(MenuItem.IconProperty, new Binding("UserInfo.ImageUri", BindingMode.OneWay));
                toolBarItem.SetBinding(MenuItem.TextProperty, new Binding("UserInfo.Name", BindingMode.OneWay));
                ToolbarItems.Add(toolBarItem);

                BindingContext = new SocialLoginViewModel(client);
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
