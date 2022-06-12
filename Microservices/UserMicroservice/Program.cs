using Core.Context;
using Core.RabbitMQ;
using Interfaces.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using UserMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(s => RabbitFactory.CreateBus("localhost"));
builder.Services.AddSingleton<UserServiceHandler>();
//builder.Services.AddSingleton(s => new UserServiceHandler(s.GetService<IRabbitBus>(), s));

builder.Services.AddControllers();



var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

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
            var context = services.GetRequiredService<CookingContext>();
            context.Database.EnsureCreated();
            context.SaveChanges();

            var temp = services.GetRequiredService<UserServiceHandler>();
        }
        catch
        {
        }
    }
}