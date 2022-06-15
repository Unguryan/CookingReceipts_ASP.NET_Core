using Core.Context;
using Core.Context.Dbo;
using Core.Models;
using Core.RabbitMQ.Models;
using Interfaces.RabbitMQ;

namespace UserMicroservice.Services
{
    public class UserServiceHandler
    {

        private readonly IRabbitBus _bus;

        private readonly IServiceProvider _provider;

        public UserServiceHandler(IRabbitBus bus, IServiceProvider provider)
        {
            _bus = bus;
            _provider = provider;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _bus.ReceiveAsync<UserAddedRabbitModel>("Users", async(u) => await OnUserAdded(u));
        }

        private async Task OnUserAdded(UserAddedRabbitModel u)
        {
            using (var scope = _provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CookingContext>();
                    await context.Users.AddAsync(new UserDbo() { Id = u.Id, 
                                                                 Name = u.Name, 
                                                                 Receipts = new List<ReceiptDbo>() });
                    await context.SaveChangesAsync();
                }
                catch
                {
                }
            }
        }
    }
}
