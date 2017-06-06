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

        public async Task<string> GetApiAsync(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode) return null;
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return await new StreamReader(responseStream).ReadToEndAsync().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<T> TryDeserializeList<T>(string sJson) where T : BaseModel, new()
        {
            try
            {
                return string.IsNullOrWhiteSpace(sJson) ? null : JsonConvert.DeserializeObject<List<T>>(sJson);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<NewsModel>> GetTopNewsAsync()
        {
            const string url = Constants.AppUrl + "api/TopNews";
            var sApiResult = await GetApiAsync(url);
            Settings.LastTopNews = string.IsNullOrWhiteSpace(sApiResult) ? Settings.LastTopNews : sApiResult;
            var news = TryDeserializeList<NewsModel>(Settings.LastTopNews) ?? new List<NewsModel>();
            return news;
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync()
        {
            const string url = Constants.AppUrl + "api/Category";
            var sApiResult = await GetApiAsync(url);
            if (!string.IsNullOrWhiteSpace(sApiResult)) Settings.Categories = sApiResult;
            var categories = TryDeserializeList<CategoryModel>(Settings.Categories);
            App.Categories = categories ?? App.Categories ?? new List<CategoryModel> { new CategoryModel { Id = "-1", Name = "Geral" } };
            return App.Categories;
        }
    }
}
