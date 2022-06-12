namespace Interfaces.Models
{
    public interface IUser
    {

        int Id { get; }

        string Name { get; }

        List<IReceipt> Receipts { get; }

    }
}
