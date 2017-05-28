using System.Collections.Generic;
using System.Threading.Tasks;
using NewsCentralizer.Model;

namespace NewsCentralizer.Services
{
    public interface IFelApiService
    {
        Task<List<NewsModel>> GetTopNewsAsync();
        Task<List<CategoryModel>> GetCategoriesAsync();
    }
}
