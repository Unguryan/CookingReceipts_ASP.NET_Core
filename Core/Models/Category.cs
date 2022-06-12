using Core.Context.Dbo;
using Interfaces.Models;

namespace Core.Models
{
    public class Category : ICategory
    {
        public Category(CategoryDbo categoryDbo)
        {
            Id = categoryDbo.Id;
            Name = categoryDbo.Name;
        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

    }
}
