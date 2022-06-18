using Core.Context;
using Core.RabbitMQ.Models;
using Interfaces.RabbitMQ;
using Microsoft.EntityFrameworkCore;

namespace ReceiptsMicroservice.Services
{
    public class ReceiptsServiceHandler
    {

        private readonly IRabbitBus _bus;

        private readonly IServiceProvider _provider;

        public ReceiptsServiceHandler(IRabbitBus bus, IServiceProvider provider)
        {
            _bus = bus;
            _provider = provider;
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _bus.ReceiveAsync<RemovedCategoryRabbitModel>("Categories", async(c) => await OnRemovedCategory(c));
        }

        private async Task OnRemovedCategory(RemovedCategoryRabbitModel cat)
        {
            using (var scope = _provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CookingContext>();

                    //TODO: DO NOT remove receipt, and just notify user, that he have to change category

                    //foreach (var item in context.Receipts
                    //    .Include(r => r.Categories).AsEnumerable()
                    //    .Where(r => r.Categories.Any(c => c.Name.Equals(cat.Name))))
                    //{
                    //    context.Receipts.Remove(item);
                    //    //TODO: Notify user, that receipt was removed
                    //}

                    //await context.SaveChangesAsync();
                }
                catch
                {
                }
            }
        }
    }
}
