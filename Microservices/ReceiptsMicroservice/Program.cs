using Core.Context;
using Core.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using ReceiptsMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(s => RabbitFactory.CreateBus("localhost"));
builder.Services.AddSingleton<ReceiptsServiceHandler>();

builder.Services.AddControllers();


builder.Services.AddCors();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(builder => builder.WithOrigins("https://localhost:44497")
                                .AllowAnyMethod()
                                .AllowAnyHeader());

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

Task.Run(() => Check(app));

app.Run();



async Task Check(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var temp = services.GetRequiredService<ReceiptsServiceHandler>();
        }
        catch
        {
        }
    }
}