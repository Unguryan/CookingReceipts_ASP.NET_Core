using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Context.Dbo
{
    public class UserDbo
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ReceiptDbo> Receipts { get; set; } 

    }
}
