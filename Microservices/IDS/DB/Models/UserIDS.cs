using Interfaces.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDS.DB.Models
{
    public class UserIDS : IUserIDS
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Roles { get; set; }

    }
}
