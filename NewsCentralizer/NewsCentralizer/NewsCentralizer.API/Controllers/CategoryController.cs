using System.Collections.Generic;
using System.Web.Http;
using NewsCentralizer.API.Models;
using Swashbuckle.Swagger.Annotations;

namespace NewsCentralizer.API.Controllers
{
    public class CategoryController : ApiController
    {
        // GET: api/Category
        [SwaggerOperation("GetCategories")]
        public IEnumerable<CategoryModel> Get()
        {
            return new List<CategoryModel>
            {
                new CategoryModel {Id = "0", Name = "Politica"},
                new CategoryModel {Id = "1", Name = "Esportes"},
                new CategoryModel {Id = "2", Name = "Destaques"},
                new CategoryModel {Id = "3", Name = "Clima"},
                new CategoryModel {Id = "4", Name = "Sociedade"},
                new CategoryModel {Id = "5", Name = "Saúde"},
                new CategoryModel {Id = "6", Name = "Arte"},
                new CategoryModel {Id = "7", Name = "Cultura"},
                new CategoryModel {Id = "8", Name = "Entreterimento"},
                new CategoryModel {Id = "9", Name = "Ciência e Tecnologia"},
                new CategoryModel {Id = "10", Name = "Desastres e Acidentes"},
                new CategoryModel {Id = "11", Name = "Ecologia e Meio ambiente"},
                new CategoryModel {Id = "12", Name = "Economia e Negócios"},
                new CategoryModel {Id = "13", Name = "Judicial"},
                new CategoryModel {Id = "14", Name = "Obituário"},
                new CategoryModel {Id = "15", Name = "Educação"},
                new CategoryModel {Id = "16", Name = "Trabalho"},
                new CategoryModel {Id = "17", Name = "Comportamento"},
                new CategoryModel {Id = "18", Name = "Religião"},
                new CategoryModel {Id = "19", Name = "História"},
                new CategoryModel {Id = "20", Name = "Variados"}
            };
        }
    }
}
