using System;
using System.Threading.Tasks;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.ViewModel
{

    public class NewsViewModel : BaseViewModel
    {
        private readonly AzureClient _client;
        private bool _isFavorite;
        private FavoriteModel _favorite;

        public NewsModel News { get; }
        public UserInfoModel UserInfo => App.UserInfo;

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { SetProperty(ref _isFavorite, value); OnPropertyChanged(propertyName: "FavoriteIcon"); }
        }

        public string FavoriteIcon => IsFavorite ? "favorite.png" : "nofavorite.png";

        public NewsViewModel(AzureClient client, NewsModel news)
        {
            IsFavorite = false;
            _client = client;
            News = news;
            FavoriteCommand = new Command<NewsModel>(ExecuteFavoriteCommand);
            Task.Run(() => FindFavorite());
        }

        public Command<NewsModel> FavoriteCommand { get; }

        private async Task FindFavorite()
        {
            _favorite = null;
            var findResult = await _client.Get<FavoriteModel>(x => x.NewsId == News.Id && x.UserId == Settings.UserId);
            if (string.IsNullOrWhiteSpace(findResult?.Id)) return;
            _favorite = findResult;
            IsFavorite = true;
        }

        private async void ExecuteFavoriteCommand(NewsModel news)
        {
            if (IsFavorite) return;
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);

                _favorite = new FavoriteModel
                {
                    NewsId = News.Id,
                    UserId = Settings.UserId,
                    News = News,
                    Id = Guid.NewGuid().ToString()
                };
                _client.Save(_favorite);
                IsFavorite = true;
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
