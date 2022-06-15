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

                    foreach (var item in context.Receipts
                        .Include(r => r.Categories)
                        .Where(r => r.Categories.Any(c => c.Id == cat.Id)))
                    {
                        context.Receipts.Remove(item);
                        //TODO: Notify user, that receipt was removed
                    }

                    await context.SaveChangesAsync();
                }
                catch
                {
                }
            }
        }
    }
}
