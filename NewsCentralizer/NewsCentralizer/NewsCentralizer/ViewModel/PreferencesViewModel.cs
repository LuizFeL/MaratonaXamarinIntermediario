using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using Xamarin.Forms;
using NewsCentralizer.Services;

namespace NewsCentralizer.ViewModel
{

    public class PreferencesViewModel : BaseViewModel
    {
        private readonly AzureClient _client;
        private ObservableCollection<PreferenceModel> _preferences;
        private ObservableCollection<CategoryModel> _categories;
        private string _selectedTags;
        private CategoryModel _selectedCategory;

        public PreferencesViewModel(AzureClient client)
        {
            _client = client;

            LoadCommand = new Command(ExecuteLoadCommand);
            DeleteCommand = new Command<string>(ExecuteDeleteCommand);
            FavoriteCommand = new Command(ExecuteFavoriteCommand);
            SaveCommand = new Command(ExecuteSaveCommand);

            Task.Run(() => LoadAsync());
        }

        public Command SaveCommand { get; }

        private async void ExecuteSaveCommand()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedTags) && SelectedCategory == null)
                {
                    await DisplayAlert("Atenção", "Informe uma Categoria e/ou TAGs", "OK");
                    return;
                }

                var confirm = await DisplayAlert("Atenção", "Deseja adicionar a preferência?", "Sim", "Não");
                if (!confirm) return;
                
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);

                var preference = new PreferenceModel
                {
                    CategoryId = SelectedCategory?.Id,
                    Id = Guid.NewGuid().ToString(),
                    Tag = SelectedTags,
                    UserId = Settings.UserId
                };
                await _client.Save(preference);
                SelectedCategory = null;
                SelectedTags = null;
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

        public Command<string> DeleteCommand { get; }

        private async void ExecuteDeleteCommand(string preferenceId)
        {
            var confirm = await DisplayAlert("Atenção", "Deseja realmente excluir a preferência?", "Sim", "Não");
            if (!confirm || string.IsNullOrWhiteSpace(preferenceId)) return;
            try
            {
                IsBusy = true;
                await Task.Delay(100).ConfigureAwait(true);
                var preference = await _client.Get<PreferenceModel>(preferenceId);
                if (string.IsNullOrWhiteSpace(preference?.Id))
                {
                    await DisplayAlert("Erro", "Preferência não encontrada", "OK");
                    return;
                }
                await _client.Delete(preference);
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
                await Task.Delay(100).ConfigureAwait(true);

                Categories = new ObservableCollection<CategoryModel>((await _client.GetCategoriesAsync()).OrderBy(x => x.Name));
                var preferences = (await _client.GetList<PreferenceModel>(x => x.UserId == Settings.UserId)).ToArray();

                var ix = 0;
                foreach (var preference in preferences)
                {
                    preference.Index = ix++;
                    preference.Category = _categories.FirstOrDefault(x => x.Id == preference.CategoryId);
                }
                Preferences = new ObservableCollection<PreferenceModel>(preferences);
            }
            catch (Exception ex)
            {
                Preferences = new ObservableCollection<PreferenceModel>();
                await DisplayAlert("Erro ao buscar preferências", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public CategoryModel SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }

        public string SelectedTags
        {
            get { return _selectedTags; }
            set { SetProperty(ref _selectedTags, value); }
        }

        public ObservableCollection<PreferenceModel> Preferences
        {
            get { return _preferences; }
            set { SetProperty(ref _preferences, value); }
        }

        public ObservableCollection<CategoryModel> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        public UserInfoModel UserInfo => App.UserInfo;

        public Command LoadCommand { get; }

        private async void ExecuteLoadCommand()
        {
            await LoadAsync();
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
