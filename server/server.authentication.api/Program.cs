using server.authentication.data.DatabaseConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using server.authentication.data.IRepository;
using server.authentication.data.Repository;
using server.authentication.application.IService;
using server.authentication.application.Service;

namespace server.authentication.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection Strings
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Database Contexts
            builder.Services.AddDbContext<UserDataContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("server.authentication.api")));

            // Dependency Injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDnsService, DnsService>();
            builder.Services.AddScoped<IValidEmailService, ValidEmailService>();
            builder.Services.AddScoped<IUserCheckEmailExistService, UserCheckEmailExistService>();
            builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();

            // Add services to the container.

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
