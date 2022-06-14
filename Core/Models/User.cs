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

            LikedReceipts = new List<IReceipt>();
            if (userDbo.LikedReceipts != null)
            {
                foreach (var item in userDbo.LikedReceipts)
                {
                    LikedReceipts.Add(new Receipt(item));
                }
            }
        }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
            Receipts = new List<IReceipt>();
            LikedReceipts = new List<IReceipt>();
        }

        public User(int id, string name, List<IReceipt> receipts, List<IReceipt> likedReceipts)
        {
            Id = id;
            Name = name;
            Receipts = receipts;
            LikedReceipts = likedReceipts;
        }

        public int Id { get; }

        public string Name { get; }

        public List<IReceipt> Receipts { get; }

        public List<IReceipt> LikedReceipts { get; }

    }
}
