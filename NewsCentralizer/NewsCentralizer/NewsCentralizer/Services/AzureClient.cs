using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
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
        private const string ServiceUri = "https://maratonaxamarinfel.azurewebsites.net/";

        public AzureClient()
        {
            Client = new MobileServiceClient(ServiceUri);
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
            Client.CurrentUser = new MobileServiceUser(Settings.UserId)
            {
                MobileServiceAuthenticationToken = Settings.AuthToken
            };
            Task.Run(() => SetUserAvatar(Settings.LoginProvider));
            return true;
        }

        public async Task<bool> LoginAsync(SocialLoginModel model)
        {
            if (TryLogin()) return true;
            if (model == null) return false;
            var user = await DependencyService.Get<IAuthentication>().LoginAsync(Client, model.Provider);

            Settings.AuthToken = user?.MobileServiceAuthenticationToken;
            Settings.UserId = user?.UserId;
            Settings.LoginProvider = model.Provider;
            await SetUserAvatar(model.Provider);

            return Settings.IsLoggedIn;
        }

        private async Task SetUserAvatar(MobileServiceAuthenticationProvider provider)
        {
            try
            {
                //TODO: Get user info
                App.UserInfo = new UserInfoModel { Id = "0", ImageUri = "usericon.png", Name = "Logedin with " + provider };
            }
            catch (Exception ex)
            {
                App.UserInfo = new UserInfoModel { Id = "0", ImageUri = "", Name = "Fazer Login" };
            }
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : IKeyObject, new()
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

        public async Task<T> Get<T>(string key) where T : IKeyObject, new()
        {
            try
            {
                var table = Client.GetSyncTable<T>();
                var queryResult = await table.Where(x => x.Id == key).ToListAsync();
                return queryResult.FirstOrDefault();
            }
            catch (Exception)
            {
                return new T();
            }
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> where) where T : IKeyObject, new()
        {
            try
            {
                var table = Client.GetSyncTable<T>();
                var queryResult = await table.Where(where).ToListAsync();
                return queryResult.FirstOrDefault();
            }
            catch (Exception)
            {
                return new T();
            }
        }

        public async void Save<T>(T data) where T : IKeyObject, new()
        {
            var table = Client.GetSyncTable<T>();

            if (await Get<T>(data.Id) == null)
            {
                await table.InsertAsync(data);
                return;
            }

            await table.UpdateAsync(data);
        }

        public async Task SyncAsync<T>() where T : IKeyObject, new()
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

        public async Task CleanData<T>() where T : IKeyObject, new()
        {
            var table = Client.GetSyncTable<T>();
            await table.PurgeAsync(GetName<T>(), table.CreateQuery(), new System.Threading.CancellationToken());
        }

        public async void Delete<T>(T data) where T : IKeyObject, new()
        {
            var table = Client.GetSyncTable<T>();
            if (await Get<T>(data.Id) == null) return;
            await table.DeleteAsync(data);
        }

        public async Task<List<NewsModel>> GetTopNewsAsync()
        {
            /*var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync($"{BaseUrl}News").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    return JsonConvert.DeserializeObject<List<NewsModel>>(
                        await new StreamReader(responseStream)
                            .ReadToEndAsync().ConfigureAwait(false));
                }
            }

            return null;*/

            //TODO: Build and get API for top news

            await Task.Delay(1500);
            var topNews = new List<NewsModel>
            {
                new NewsModel {Id = "0",Index = 0, Title = "Câmara recebe 14º pedido de impeachment de Temer em 15 dias",Url ="http://istoe.com.br/em-duas-semanas-camara-recebe-14-pedidos-de-impeachment-de-temer/", ImageUrl ="https://p2.trrsf.com/image/fget/cf/372/372/20/0/140/140/images.terra.com/2017/05/30/florestanacionaljamanximgreenpeace.jpg"},
                new NewsModel {Id = "1",Index = 1, Title = "Com crise, ruralistas aceleram votação de projetos polêmicos",Url ="https://www.terra.com.br/noticias/brasil/politica/com-crise-no-governo-ruralistas-aceleram-votacao-de-projetos-polemicos,200ff8e3475dabaaa1976ecd97685144p3m1r13y.html", ImageUrl ="https://p2.trrsf.com/image/fget/cf/372/372/20/0/140/140/images.terra.com/2017/05/30/florestanacionaljamanximgreenpeace.jpg"},
                new NewsModel {Id = "2",Index = 2, Title = "Rei dos desarmes e bom passador, Jucilei conquista São Paulo",Url ="https://www.terra.com.br/esportes/lance/rei-dos-desarmes-no-time-e-bom-passador-jucilei-conquista-sao-paulo,56af3b37b36dd1c1649f354a98d888268ftdmmtf.html", ImageUrl ="https://p2.trrsf.com/image/fget/cf/140/140/images.terra.com/2017/05/30/jucileisaopaulopalmeirasbrasileirao27052017rubenschirispfc.JPG"},
                new NewsModel {Id = "3",Index = 3, Title = "Veja os 10 atores que mais arrecadaram em bilheteria no cinema",Url ="http://click.uol.com.br/?rf=homec-chamada-topo-modulo-tt-carros1&pos=mod-1_col-4;topo&u=https://cinema.uol.com.br/noticias/redacao/2017/05/30/bilhoes-e-mais-bilhoes-confira-os-10-astros-mais-valiosos-de-hollywood.htm", ImageUrl ="https://hp.imguol.com.br/c/home/d3/2017/02/05/cartaz-de-capitao-america-2-o-soldado-invernal-mostra-samuel-l-jackson-no-papel-de-nick-fury-1486294455279_200x140.jpg"},
                new NewsModel {Id = "4",Index = 4, Title = "Primeira vez com Anitta. Vem ver",Url ="http://click.uol.com.br/?rf=homec-chamada-destaque-entretenimento-modulo1-item1&pos=mod-7_col-3;entretenimento&u=https://musica.uol.com.br/noticias/redacao/2017/05/29/quando-foi-o-primeiro-porre-da-anitta.htm", ImageUrl ="https://hp.imguol.com.br/c/home/1f/2017/05/29/anitta-participa-do-quadro-primeira-vez-1496086942921_300x200.jpg"},
                new NewsModel {Id = "5",Index = 5, Title = "Homem desenterra irmão, carrega o caixão na bicicleta e é detido em MG",Url ="http://click.bol.com.br/?rf=homeb-painel-item1&pos=mod-1;painel&u=https://noticias.bol.uol.com.br/ultimas-noticias/brasil/2017/05/29/homem-desenterra-irmao-carrega-o-caixao-na-bicicleta-e-e-detido-em-minas-gerais.htm", ImageUrl ="https://conteudo.imguol.com.br/c/bol/fotos/calango/73/2017/05/30/homem-desenterra-irmao-carrega-o-caixao-na-bicicleta-e-e-detido-em-mg-1496138436333_v2_693x352.jpg"}
            };

            return topNews;
        }

    }
}
