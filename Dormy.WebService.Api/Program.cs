using Dormy.WebService.Api.Infrastructure.Postgres;
using Dormy.WebService.Api.Infrastructure.Postgres.IRepositories;
using Dormy.WebService.Api.Infrastructure.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("LocalConnection"));
            });

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Add infrastructure
            builder.Services.AddTransient<IAdminRepository, AdminRepository>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}