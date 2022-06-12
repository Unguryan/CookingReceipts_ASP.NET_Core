using Core.Context.Dbo;
using Interfaces.Models;

namespace Core.Models
{
    public class User : IUser
    {
        public User(UserDbo userDbo)
        {
            Id = userDbo.Id;
            Name = userDbo.Name;
            Receipts = new List<IReceipt>();
            if(userDbo.Receipts != null)
            {
                foreach (var item in userDbo.Receipts)
                {
                    Receipts.Add(new Receipt(item));
                }
            }
        }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
            Receipts = new List<IReceipt>();
        }

        public User(int id, string name, List<IReceipt> receipts)
        {
            Id = id;
            Name = name;
            Receipts = receipts;
        }

        public int Id { get; }

        public string Name { get; }

        public List<IReceipt> Receipts { get; }

    }
}
