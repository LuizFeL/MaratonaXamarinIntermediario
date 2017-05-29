using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using NewsCentralizer.Model;

namespace NewsCentralizer.Services
{
    public abstract class AzureDbClient<T> where T : IKeyObject, new()
    {
        protected readonly IMobileServiceClient Client;
        const string DbPath = "data.db";
        private const string ServiceUri = "http://maratonaxamarinfel.azurewebsites.net/";
        protected readonly IMobileServiceSyncTable<T> Table;
        protected readonly string Name;

        protected AzureDbClient()
        {
            Client = new MobileServiceClient(ServiceUri);
            var store = new MobileServiceSQLiteStore(DbPath);

            Name = "all_" + typeof(T).Name;

            store.DefineTable<T>();

            Client.SyncContext.InitializeAsync(store);
            Table = Client.GetSyncTable<T>();
        }

        public async Task<IEnumerable<T>> GetUsers()
        {
            var empty = new T[0];
            try
            {
                if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                    await SyncUsersAsync();

                return await Table.ToEnumerableAsync();
            }
            catch (Exception)
            {
                return empty;
            }
        }


        public async Task<T> GetUser(string key)
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

            if (await GetUser(data.Id) == null)
            {
                await table.InsertAsync(data);
                return;
            }

            await table.UpdateAsync(data);
        }

        public async Task SyncUsersAsync()
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


        public async Task CleanUserData()
        {
            await Table.PurgeAsync(Name, Table.CreateQuery(), new System.Threading.CancellationToken());
        }



        public async void Delete(T data)
        {
            var table = Table;
            if (await GetUser(data.Id) == null) return;
            await table.DeleteAsync(data);
        }
    }
}
