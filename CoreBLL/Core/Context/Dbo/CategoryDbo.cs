using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Context.Dbo
{
    public class CategoryDbo
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
