using Core.Context.Dbo;
using Interfaces.Models;

namespace Core.Models
{
    public class Receipt : IReceipt
    {
        public Receipt(ReceiptDbo receiptDbo)
        {
            Id = receiptDbo.Id;
            //TODO: Fix error here 
            Owner = new User(receiptDbo.Owner, false);
            Name = receiptDbo.Name;
            Description = receiptDbo.Description;
            Ingredients = receiptDbo.Ingredients;
            Categories = new List<ICategory>();
            if(receiptDbo.Categories != null)
            {
                foreach (var item in receiptDbo.Categories)
                {
                    Categories.Add(new Category(item));
                }
            }
        }

        public Receipt(int id, IUser owner, string name, string description, string ingredients)
        {
            Id = id;
            Owner = owner;
            Name = name;
            Description = description;
            Ingredients = ingredients;
            Categories = new List<ICategory>();
        }

        public Receipt(int id, IUser owner, string name, List<ICategory> categories, string description, string ingredients)
        {
            Id = id;
            Owner = owner;
            Name = name;
            Categories = categories;
            Description = description;
            Ingredients = ingredients;
        }

        public int Id { get; }

        public IUser Owner { get; }

        public string Name { get; }

        public List<ICategory> Categories { get; }

        public string Description { get; }

        public string Ingredients { get; }

    }
}
