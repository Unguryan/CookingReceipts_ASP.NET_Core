using Interfaces.ViewModels.Receipt;

namespace ReceiptsMicroservice.Models
{
    public class AddOrRemoveLikedReceiptModel : IAddOrRemoveLikedReceiptModel
    {

        public int UserId { get; set; }

        public int ReceiptId { get; set; }

    }
}
