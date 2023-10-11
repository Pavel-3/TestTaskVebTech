using Microsoft.EntityFrameworkCore;
using TestTaskVebTech.Data;
using TestTaskVebTech.Data.Entities;
using TestTaskVebTech.Bussiness.Abstractions;
using TestTaskVebTech.Bussiness.Services;
namespace TestTaskVebTech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(Program));
            // Add services to the container.
            builder.Services.AddDbContext<UserListContext>(option =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("DefaultConnection");
                option.UseSqlServer(connString);
            });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddControllers();
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