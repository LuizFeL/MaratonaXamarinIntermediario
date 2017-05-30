using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.ViewModel
{

    public class TopNewsViewModel : BaseViewModel
    {
        private readonly AzureClient _client;
        private ObservableCollection<NewsModel> _topNews;

        public TopNewsViewModel(AzureClient client)
        {
            _client = client;
            Title = "Top News";            
                        
            GoToNewsCommand = new Command<NewsModel>(ExecuteGoToNewsCommand);
            LoadNewsCommand = new Command(ExecuteLoadNewsCommand);
            FavoriteCommand = new Command(ExecuteFavoriteCommand);

            Task.Run(() => LoadAsync());
        }

        public override async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);
                var news = await _client.GetTopNewsAsync();
                TopNews = new ObservableCollection<NewsModel>(news);
            }
            catch (Exception ex)
            {
                TopNews = new ObservableCollection<NewsModel>();
                await DisplayAlert("Erro ao buscar top news", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<NewsModel> TopNews
        {
            get { return _topNews; }
            set { SetProperty(ref _topNews, value); }
        }

        public UserInfoModel UserInfo => App.UserInfo;

        public Command LoadNewsCommand { get; }

        private async void ExecuteLoadNewsCommand()
        {
            await LoadAsync();
        }

        public Command<NewsModel> GoToNewsCommand { get; }

        private async void ExecuteGoToNewsCommand(NewsModel news)
        {
            if (news == null) return;
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);

                //TODO: Go to News
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        public Command FavoriteCommand { get; }

        private async void ExecuteFavoriteCommand()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);

                //TODO: Go to favorite
                await DisplayAlert("foi", "favoritos", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
    }

    public class ListZebraColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var iValue = (int)value;
                return (iValue % 2) != 0 ? Color.Transparent : Color.FromHex("#3399ff").MultiplyAlpha(0.2);
            }
            catch
            {
                return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; //Do nothing
        }
    }
}
