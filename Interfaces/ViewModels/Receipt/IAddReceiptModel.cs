namespace Interfaces.ViewModels.Receipt
{
    public interface IAddReceiptModel
    {

        List<int> Categories { get; set; }

        string Description { get; set; }

        string Ingredients { get; set; }

        string Name { get; set; }

        int OwnerId { get; set; }

    }
}