﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
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
                await PushAsync<NewsViewModel>(news);
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
                await PushAsync<FavoriteViewModel>();
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
}