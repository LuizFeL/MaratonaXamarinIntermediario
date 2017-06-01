using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.ViewModel
{

    public class FavoriteViewModel : BaseViewModel
    {
        private readonly AzureClient _client;
        private ObservableCollection<FavoriteModel> _favorites;

        public FavoriteViewModel(AzureClient client)
        {
            _client = client;
            Title = "Meus Favoritos";

            GoToNewsCommand = new Command<string>(ExecuteGoToNewsCommand);
            LoadFavoritesCommand = new Command(ExecuteLoadFavoritesCommand);
            DeleteFavoriteCommand = new Command<string>(ExecuteDeleteFavoriteCommand);

            Task.Run(() => LoadAsync());
        }

        public Command<string> DeleteFavoriteCommand { get; }

        private async void ExecuteDeleteFavoriteCommand(string favoriteId)
        {
            var confirm = await DisplayAlert("Atenção", "Deseja realmente excluir o favorito?", "Sim", "Não");
            if (!confirm || string.IsNullOrWhiteSpace(favoriteId)) return;
            try
            {
                IsBusy = true;
                var favorite = await _client.Get<FavoriteModel>(favoriteId);
                if (string.IsNullOrWhiteSpace(favorite?.Id))
                {
                    await DisplayAlert("Erro", "Noticia não encontrada", "OK");
                    return;
                }
                await Task.Delay(100).ConfigureAwait(true);
                await _client.Delete(favorite);
                await LoadAsync();
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

        public override async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                //await Task.Delay(100).ConfigureAwait(true);
                var favorites = (await _client.GetList<FavoriteModel>(x=>x.UserId == Settings.UserId)).ToArray();
                var ix = 0;
                foreach (var favorite in favorites)
                {
                    favorite.Index = ix++;
                    favorite.News = await _client.Get<NewsModel>(favorite.NewsId);
                }
                Favorites = new ObservableCollection<FavoriteModel>(favorites);
            }
            catch (Exception ex)
            {
                Favorites = new ObservableCollection<FavoriteModel>();
                await DisplayAlert("Erro ao buscar favoritos", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<FavoriteModel> Favorites
        {
            get { return _favorites; }
            set { SetProperty(ref _favorites, value); }
        }

        public UserInfoModel UserInfo => App.UserInfo;

        public Command LoadFavoritesCommand { get; }

        private async void ExecuteLoadFavoritesCommand()
        {
            await LoadAsync();
        }

        public Command<string> GoToNewsCommand { get; }

        private async void ExecuteGoToNewsCommand(string favoriteId)
        {
            if (string.IsNullOrWhiteSpace(favoriteId)) return;
            try
            {
                IsBusy = true;
                var favorite = Favorites.FirstOrDefault(x => x.Id == favoriteId);
                if (string.IsNullOrWhiteSpace(favorite?.Id)) return;

                await Task.Delay(100).ConfigureAwait(true);
                var news = favorite?.News ?? await _client.Get<NewsModel>(favorite?.NewsId);

                if (string.IsNullOrWhiteSpace(news?.Id))
                {
                    await DisplayAlert("Erro", "Noticia não encontrada", "OK");
                    return;
                }
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

    }

}
