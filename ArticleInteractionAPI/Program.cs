using System.Reflection;
using ArticleInteractionAPI.Infrastructure;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;

namespace ArticleInteractionAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ArticleInteractionConnection")));

            builder.Services.AddControllers();

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["Redis:ConnectionString"];
            });
            builder.Services.AddSingleton<IRateLimiter, RedisRateLimiter>();
            builder.Services.AddMemoryCache();
            builder.Services.AddInMemoryRateLimiting();

            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
