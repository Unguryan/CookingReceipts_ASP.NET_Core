namespace Interfaces.ViewModels.Receipt
{
    public interface IChangeReceiptModel
    {

        List<int> Categories { get; set; }

        string Description { get; set; }

        string Ingredients { get; set; }

        string Name { get; set; }

        int OwnerId { get; set; }

        int ReceiptId { get; set; }

    }
}