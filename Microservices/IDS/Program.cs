using Core.RabbitMQ;
using IDS.DB;
using IDS.Interfaces;
using IDS.Tokens;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
            .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });

builder.Services.AddDbContext<IDS_Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IDS_Connection")));

builder.Services.AddSingleton(s => RabbitFactory.CreateBus("localhost"));

builder.Services.AddScoped<ITokenManager, TokenManager>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<DBSeed>();

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

Task.Run(() => Seed(app));

app.Run();



async Task Seed(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<IDS_Context>();
            context.Database.EnsureCreated();
            context.SaveChanges();

            var seed = services.GetRequiredService<DBSeed>();

            await seed.Seed();
        }
        catch
        {
        }
    }
}
