using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Context.Dbo
{
    public class ReceiptDbo
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public UserDbo Owner { get; set; }

        public string Name { get; set; }

        public List<CategoryDbo> Categories { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

    }
}
