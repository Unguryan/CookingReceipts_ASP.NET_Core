using System.ComponentModel.DataAnnotations.Schema;

namespace IDS.DB.Models
{
    public class UserTokenIDS
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpairedAt { get; set; }

    }
}
