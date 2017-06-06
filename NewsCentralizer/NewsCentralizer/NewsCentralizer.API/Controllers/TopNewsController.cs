using System.Collections.Generic;
using System.Web.Http;
using NewsCentralizer.API.Models;
using Swashbuckle.Swagger.Annotations;

namespace NewsCentralizer.API.Controllers
{
    public class TopNewsController : ApiController
    {
        // GET: api/TopNews
        [SwaggerOperation("GetTopNews")]
        public IEnumerable<NewsModel> Get()
        {
            //TODO: Get news from other services
            var topNewsCompleteList = new List<NewsModel>
            {
                new NewsModel
                {
                    Id = "0",
                    Index = 0,
                    Title = "Câmara recebe 14º pedido de impeachment de Temer em 15 dias",
                    Url = "http://istoe.com.br/em-duas-semanas-camara-recebe-14-pedidos-de-impeachment-de-temer/",
                    ImageUrl =
                        "https://p2.trrsf.com/image/fget/cf/372/372/20/0/140/140/images.terra.com/2017/05/30/florestanacionaljamanximgreenpeace.jpg"
                },
                new NewsModel
                {
                    Id = "1",
                    Index = 1,
                    Title = "Com crise, ruralistas aceleram votação de projetos polêmicos",
                    Url =
                        "https://www.terra.com.br/noticias/brasil/politica/com-crise-no-governo-ruralistas-aceleram-votacao-de-projetos-polemicos,200ff8e3475dabaaa1976ecd97685144p3m1r13y.html",
                    ImageUrl =
                        "https://p2.trrsf.com/image/fget/cf/372/372/20/0/140/140/images.terra.com/2017/05/30/florestanacionaljamanximgreenpeace.jpg"
                },
                new NewsModel
                {
                    Id = "2",
                    Index = 2,
                    Title = "Rei dos desarmes e bom passador, Jucilei conquista São Paulo",
                    Url =
                        "https://www.terra.com.br/esportes/lance/rei-dos-desarmes-no-time-e-bom-passador-jucilei-conquista-sao-paulo,56af3b37b36dd1c1649f354a98d888268ftdmmtf.html",
                    ImageUrl =
                        "https://p2.trrsf.com/image/fget/cf/140/140/images.terra.com/2017/05/30/jucileisaopaulopalmeirasbrasileirao27052017rubenschirispfc.JPG"
                },
                new NewsModel
                {
                    Id = "3",
                    Index = 3,
                    Title = "Veja os 10 atores que mais arrecadaram em bilheteria no cinema",
                    Url =
                        "http://click.uol.com.br/?rf=homec-chamada-topo-modulo-tt-carros1&pos=mod-1_col-4;topo&u=https://cinema.uol.com.br/noticias/redacao/2017/05/30/bilhoes-e-mais-bilhoes-confira-os-10-astros-mais-valiosos-de-hollywood.htm",
                    ImageUrl =
                        "https://hp.imguol.com.br/c/home/d3/2017/02/05/cartaz-de-capitao-america-2-o-soldado-invernal-mostra-samuel-l-jackson-no-papel-de-nick-fury-1486294455279_200x140.jpg"
                },
                new NewsModel
                {
                    Id = "4",
                    Index = 4,
                    Title = "Primeira vez com Anitta. Vem ver",
                    Url =
                        "http://click.uol.com.br/?rf=homec-chamada-destaque-entretenimento-modulo1-item1&pos=mod-7_col-3;entretenimento&u=https://musica.uol.com.br/noticias/redacao/2017/05/29/quando-foi-o-primeiro-porre-da-anitta.htm",
                    ImageUrl =
                        "https://hp.imguol.com.br/c/home/1f/2017/05/29/anitta-participa-do-quadro-primeira-vez-1496086942921_300x200.jpg"
                },
                new NewsModel
                {
                    Id = "5",
                    Index = 5,
                    Title = "Homem desenterra irmão, carrega o caixão na bicicleta e é detido em MG",
                    Url =
                        "http://click.bol.com.br/?rf=homeb-painel-item1&pos=mod-1;painel&u=https://noticias.bol.uol.com.br/ultimas-noticias/brasil/2017/05/29/homem-desenterra-irmao-carrega-o-caixao-na-bicicleta-e-e-detido-em-minas-gerais.htm",
                    ImageUrl =
                        "https://conteudo.imguol.com.br/c/bol/fotos/calango/73/2017/05/30/homem-desenterra-irmao-carrega-o-caixao-na-bicicleta-e-e-detido-em-mg-1496138436333_v2_693x352.jpg"
                },
                new NewsModel
                {
                    Id = "a1231231321",
                    Index = 6,
                    Title = "Aécio Neves não achava que cassação ia até o fim",
                    ImageUrl =
                        "https://hp.imguol.com.br/c/home/d8/2017/05/22/9nov2016---senador-aecio-neves-psdb-mg-1495435036641_80x80.jpg",
                    Url =
                        "http://click.uol.com.br/?rf=homec-submanchete-topo-modulo3&pos=mod-1_col-2;topo&u=https://noticias.uol.com.br/politica/ultimas-noticias/2017/06/06/em-grampo-aecio-disse-que-nao-achava-que-acao-no-tse-iria-ate-o-fim-a-dilma-caiu-e-a-acao-continuou.htm"
                },
                new NewsModel
                {
                    Id = "b1231231322",
                    Index = 7,
                    Title = "Vivo vai transmitir os dois jogos da seleção brasileira na Austrália",
                    ImageUrl =
                        "https://hp.imguol.com.br/c/home/f7/2017/03/14/daniel-alves-coutinho-neymar-e-gabriel-jesus-comemoram-gol-da-selecao-brasileira-contra-a-argentina-1489533737016_200x140.jpg",
                    Url =
                        "http://click.uol.com.br/?rf=homec-chamada-esporte-modulo20&pos=mod-5_col-4;esporte&u=http://www1.folha.uol.com.br/esporte/2017/06/1890488-vivo-vai-transmitir-amistosos-da-selecao-brasileira-na-australia.shtml"
                },
                new NewsModel
                {
                    Id = "c1231231323",
                    Title = "Crise do Golfo pode afetar organização",
                    Index = 8,
                    ImageUrl =
                        "https://hp.imguol.com.br/c/home/74/2017/06/05/copa-do-mundo-no-qatar-1496704215690_200x140.jpg",
                    Url =
                        "http://click.uol.com.br/?rf=homec-chamada-esporte-modulo16&pos=mod-5_col-4;esporte&u=https://esporte.uol.com.br/ultimas-noticias/afp/2017/06/05/crise-do-golfo-pode-afetar-organizacao-da-copa-do-mundo-do-catar.htm"
                },
                new NewsModel
                {
                    Id = "d1231231324",
                    Index = 9,
                    Title = "5 dicas para fazer seu celular velho ficar com cara de novo",
                    ImageUrl =
                        "https://hp.imguol.com.br/c/home/53/2017/06/06/quatro-dicas-sobre-o-que-fazer-com-seu-celular-velho-1496723975627_142x100.jpg",
                    Url =
                        "http://click.uol.com.br/?rf=homec-chamada-topo-modulo18-direita&pos=mod-1_col-3;topo&u=http://tecnologia.uol.com.br/noticias/redacao/2017/06/06/5-dicas-para-voce-fazer-seu-celular-velho-parecer-novo.htm"
                }
            };

            return topNewsCompleteList.PickRandom(4);
        }
    }
}
