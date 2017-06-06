using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using NewsCentralizer.Authentication;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NewsCentralizer.Services
{
    public class AzureClient
    {
        protected readonly MobileServiceClient Client;
        protected MobileServiceSQLiteStore Store;
        const string DbPath = "data.db";

        public AzureClient()
        {
            Client = new MobileServiceClient(Constants.AppUrl);
            Store = new MobileServiceSQLiteStore(DbPath);
            Store.DefineTable<CategoryModel>();
            Store.DefineTable<FavoriteModel>();
            Store.DefineTable<NewsModel>();
            Store.DefineTable<PreferenceModel>();
            Client.SyncContext.InitializeAsync(Store);
            TryLogin();
        }

        public string GetName<T>() where T : IKeyObject, new()
        {
            return "all_" + typeof(T).Name;
        }

        public bool TryLogin()
        {
            if (string.IsNullOrWhiteSpace(Settings.AuthToken) || string.IsNullOrWhiteSpace(Settings.UserId)) return false;
            Client.CurrentUser = new MobileServiceUser(Settings.UserId) { MobileServiceAuthenticationToken = Settings.AuthToken };
            return true;
        }

        public async Task<bool> LoginAsync(SocialLoginModel model)
        {
            if (TryLogin()) return true;
            if (model == null) return false;
            var user = await DependencyService.Get<IAuthentication>().LoginAsync(Client, model.Provider);

            Settings.AuthToken = user?.MobileServiceAuthenticationToken;
            Settings.UserId = user?.UserId;
            Settings.LoginProvider = ((int)model.Provider).ToString();

            return Settings.IsLoggedIn;
        }

        public async Task LogoutAsync()
        {
            await Client.LogoutAsync();
            Settings.AuthToken = null;
            Settings.UserId = null;
            App.UserInfo = new UserInfoModel { Id = "0", Image = "", Name = "Fazer Login" };
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : BaseModel, new()
        {
            var empty = new T[0];
            try
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                    await SyncAsync<T>();

                var table = Client.GetSyncTable<T>();
                return await table.ToEnumerableAsync();
            }
            catch (Exception)
            {
                return empty;
            }
        }

        public async Task<T> Get<T>(string key) where T : BaseModel, new()
        {
            return await Get<T>(x => x.Id == key);
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> where) where T : BaseModel, new()
        {
            try
            {
                var table = Client.GetSyncTable<T>();
                var queryResult = await table.Where(where).ToListAsync();
                return queryResult.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<T>> GetList<T>(Expression<Func<T, bool>> where) where T : BaseModel, new()
        {
            try
            {
                var table = Client.GetSyncTable<T>();
                var queryResult = await table.Where(where).ToListAsync();
                return queryResult.ToList();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public async Task Save<T>(T data) where T : BaseModel, new()
        {
            var table = Client.GetSyncTable<T>();

            var existingData = await Get<T>(data.Id);
            var exists = !string.IsNullOrWhiteSpace(existingData?.Id);

            if (!exists)
            {
                await table.InsertAsync(data);
                return;
            }

            await table.UpdateAsync(data);
        }

        public async Task SyncAsync<T>() where T : BaseModel, new()
        {
            try
            {
                var table = Client.GetSyncTable<T>();
                await Client.SyncContext.PushAsync();
                await table.PullAsync(GetName<T>(), table.CreateQuery());
            }
            catch (Exception)
            {
                //DoNothing
            }
        }

        public async Task CleanData<T>() where T : BaseModel, new()
        {
            var table = Client.GetSyncTable<T>();
            await table.PurgeAsync(GetName<T>(), table.CreateQuery(), new System.Threading.CancellationToken());
        }

        public async Task Delete<T>(T data) where T : BaseModel, new()
        {
            var table = Client.GetSyncTable<T>();
            if (await Get<T>(data.Id) == null) return;
            await table.DeleteAsync(data);
        }

        public async Task<List<NewsModel>> GetTopNewsAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

#if DEBUG
                var url = "http://localhost:50037/api/TopNews";
#else
            var url = Constants.AppUrl + "api/TopNews";
#endif
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        return JsonConvert.DeserializeObject<List<NewsModel>>(
                            await new StreamReader(responseStream)
                                .ReadToEndAsync().ConfigureAwait(false));
                    }
                }

                return new List<NewsModel>();
            }
            catch (Exception ex)
            {
                var displayAlert = Application.Current?.MainPage?.DisplayAlert("Erro ao buscar top news", ex.Message, "OK");
                if (displayAlert != null) await displayAlert;
                return new List<NewsModel>();
            }
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            try
            {
                if (App.Categories != null) return App.Categories;
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

#if DEBUG
                var url = "http://localhost:50037/api/Category";
#else
                var url = Constants.AppUrl + "api/TopNews";
#endif
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {

                        App.Categories = JsonConvert.DeserializeObject<List<CategoryModel>>(
                            await new StreamReader(responseStream)
                                .ReadToEndAsync().ConfigureAwait(false));
                        return App.Categories;
                    }
                }

                App.Categories = new List<CategoryModel>();
                return App.Categories;
            }
            catch (Exception ex)
            {
                App.Categories = new List<CategoryModel>();
                var displayAlert = Application.Current?.MainPage?.DisplayAlert("Erro ao buscar categorias", ex.Message, "OK");
                if (displayAlert != null) await displayAlert;
                return App.Categories;
            }
        }
    }
}
