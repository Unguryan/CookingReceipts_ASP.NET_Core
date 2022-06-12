using System.ComponentModel.DataAnnotations.Schema;

namespace IDS.DB.Models
{
    public class RoleIDS
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
