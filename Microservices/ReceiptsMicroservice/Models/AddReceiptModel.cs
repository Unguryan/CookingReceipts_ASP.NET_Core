namespace ReceiptsMicroservice.Models
{
    public class AddReceiptModel
    {
        public int OwnerId { get; set; }

        public string Name { get; set; }

        public List<int> Categories { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }
    }
}
