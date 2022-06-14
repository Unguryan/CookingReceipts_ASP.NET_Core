using Interfaces.ViewModels.Category;

namespace CategoryMicroservice.Models
{
    public class AddCategoryModel : IAddCategoryModel
    {

        public string Name { get; set; }

    }
}
