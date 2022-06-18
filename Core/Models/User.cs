using Core.Context.Dbo;
using Interfaces.Models;

namespace Core.Models
{
    public class User : IUser
    {
        public User(UserDbo userDbo, bool initReceipts = false)
        {
            Id = userDbo.Id;
            Name = userDbo.Name;
            Receipts = new List<IReceipt>();
            if(userDbo.Receipts != null && initReceipts)
            {
                foreach (var item in userDbo.Receipts)
                {
                    Receipts.Add(new Receipt(item.Id,
                                             this,
                                             item.Name,
                                             item.Description,
                                             item.Ingredients));
                }
            }

            LikedReceipts = new List<IReceipt>();
            if (userDbo.LikedReceipts != null && initReceipts)
            {
                foreach (var item in userDbo.LikedReceipts)
                {
                    LikedReceipts.Add(new Receipt(item.Id,
                                             this,
                                             item.Name,
                                             item.Description,
                                             item.Ingredients));
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
