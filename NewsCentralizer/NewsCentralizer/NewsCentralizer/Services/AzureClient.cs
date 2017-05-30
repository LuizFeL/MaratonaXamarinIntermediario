using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using NewsCentralizer.Authentication;
using NewsCentralizer.Helpers;
using NewsCentralizer.Model;
using Xamarin.Forms;

namespace NewsCentralizer.Services
{
    public class AzureClient<T> where T : IKeyObject, new()
    {
        protected readonly MobileServiceClient Client;
        const string DbPath = "data.db";
        private const string ServiceUri = "https://maratonaxamarinfel.azurewebsites.net/";
        protected readonly IMobileServiceSyncTable<T> Table;
        protected readonly string Name;

        public AzureClient()
        {
            Client = new MobileServiceClient(ServiceUri);
            var store = new MobileServiceSQLiteStore(DbPath);

            Name = "all_" + typeof(T).Name;

            store.DefineTable<T>();

            Client.SyncContext.InitializeAsync(store);
            Table = Client.GetSyncTable<T>();

            TryLogin();
        }

        private bool TryLogin()
        {
            if (string.IsNullOrWhiteSpace(Settings.AuthToken) || string.IsNullOrWhiteSpace(Settings.UserId)) return false;
            Client.CurrentUser = new MobileServiceUser(Settings.UserId)
            {
                MobileServiceAuthenticationToken = Settings.AuthToken
            };
            return true;
        }

        public async Task<bool> LoginAsync(SocialLoginModel model)
        {
            if (TryLogin()) return true;
            if (model == null) return false;
            var user = await DependencyService.Get<IAuthentication>().LoginAsync(Client, model.Provider);

            Settings.AuthToken = user?.MobileServiceAuthenticationToken;
            Settings.UserId = user?.UserId;

            return Settings.IsLoggedIn;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var empty = new T[0];
            try
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                    await SyncAsync();

                return await Table.ToEnumerableAsync();
            }
            catch (Exception)
            {
                return empty;
            }
        }

        public async Task<T> Get(string key)
        {
            try
            {
                var queryResult = await Table.Where(x => x.Id == key).ToListAsync();
                return queryResult.FirstOrDefault();
            }
            catch (Exception)
            {
                return new T();
            }
        }

        public async void Save(T data)
        {
            var table = Table;

            if (await Get(data.Id) == null)
            {
                await table.InsertAsync(data);
                return;
            }

            await table.UpdateAsync(data);
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await Client.SyncContext.PushAsync();
                await Table.PullAsync(Name, Table.CreateQuery());
            }
            catch (MobileServicePushFailedException pushEx)
            {
                if (pushEx.PushResult != null) syncErrors = pushEx.PushResult.Errors;
            }
            catch (Exception)
            {
                //DoNothing
            }
        }

        public async Task CleanData()
        {
            await Table.PurgeAsync(Name, Table.CreateQuery(), new System.Threading.CancellationToken());
        }

        public async void Delete(T data)
        {
            var table = Table;
            if (await Get(data.Id) == null) return;
            await table.DeleteAsync(data);
        }
    }
}
