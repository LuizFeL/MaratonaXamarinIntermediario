using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NewsCentralizer.Helpers;
using Xamarin.Forms;
using NewsCentralizer.Services;
using NewsCentralizer.View;

namespace NewsCentralizer.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected BaseViewModel()
        {

            LogoutCommand = new Command(ExecuteLogoutCommand);
        }

        public Command LogoutCommand { get; }

        private async void ExecuteLogoutCommand()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);
                if (Settings.IsLoggedIn)
                {
                    var confirmLogout = await DisplayAlert("Logout", "Deseja realmente sair?", "Sim", "Não");
                    if (!confirmLogout) return;
                    await App.AzureClient.LogoutAsync();
                    await LogoutNavigationAsync();
                    return;
                }

                await PushModalAsync<SocialLoginViewModel>();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro ao sair", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public bool IsWorking { get; set; }


        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public bool IsNotBusy => !IsBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public Page GetPage<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            var viewModelType = typeof(TViewModel);
            var viewModelTypeName = viewModelType.Name;
            var viewModelWordLength = "ViewModel".Length;
            var viewTypeName = "NewsCentralizer.View." + viewModelTypeName.Substring(0, viewModelTypeName.Length - viewModelWordLength) + "View";
            var viewType = Type.GetType(viewTypeName);

            var page = Activator.CreateInstance(viewType) as Page;

            if (viewModelType.GetTypeInfo().DeclaredConstructors.Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(AzureClient))))
            {
                var argsList = args.ToList();
                argsList.Insert(0, App.AzureClient);
                args = argsList.ToArray();
            }

            var viewModel = Activator.CreateInstance(viewModelType, args);
            if (page != null)
            {
                page.BindingContext = viewModel;
            }

            return page;
        }

        public async Task PushAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            await Application.Current.MainPage.Navigation.PushAsync(GetPage<TViewModel>(args));
        }

        public async Task PushModalAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(GetPage<TViewModel>(args));
        }

        public async Task LogoutNavigationAsync(params object[] args)
        {
            await Task.Delay(10).ConfigureAwait(true);
            Application.Current.MainPage = new NavigationPage(new SocialLoginView(App.AzureClient));
        }

        public virtual Task LoadAsync()
        {
            return Task.FromResult(0);
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
