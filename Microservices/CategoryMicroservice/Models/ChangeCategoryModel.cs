using Interfaces.ViewModels.Category;

namespace CategoryMicroservice.Models
{
    public class ChangeCategoryModel : IChangeCategoryModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

    }
}
