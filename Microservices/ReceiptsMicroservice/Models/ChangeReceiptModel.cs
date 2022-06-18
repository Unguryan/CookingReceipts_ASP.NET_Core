using Interfaces.ViewModels.Receipt;

namespace ReceiptsMicroservice.Models
{
    public class ChangeReceiptModel : IChangeReceiptModel
    {

        public int ReceiptId { get; set; }

        public int OwnerId { get; set; }

        public string Name { get; set; }

        public List<int> Categories { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

    }
}
