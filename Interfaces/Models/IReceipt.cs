namespace Interfaces.Models
{
    public interface IReceipt
    {

        int Id { get; }

        IUser Owner { get; }

        string Name { get; }

        List<ICategory> Categories { get; }

        string Description { get; }

        string Ingredients { get; }

    }
}
