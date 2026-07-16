namespace server.authentication.api;

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
        builder.Services.AddScoped<ICheckDnsService, CheckDnsService>();
        builder.Services.AddScoped<IValidEmailService, ValidEmailService>();
        builder.Services.AddScoped<IUserCheckEmailExistService, UserCheckEmailExistService>();
        builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
        builder.Services.AddScoped<ILoginUserService, LoginUserService>();
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
        builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
        builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();

        // JWT Authentication
        var jwtSection = builder.Configuration.GetSection("Jwt");
        var key = jwtSection["Key"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = !string.IsNullOrEmpty(jwtSection["Issuer"]),
                // We don't use a single static ValidAudience here because audience is per-user (username).
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["one-direction"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty)),
                ClockSkew = TimeSpan.Zero
            };

            // Validate that the token's audience equals the username claim inside the token
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var jwt = context.SecurityToken as JwtSecurityToken;
                    if (jwt == null)
                    {
                        context.Fail("Invalid token");
                        return Task.CompletedTask;
                    }

                    var tokenAudience = jwt.Audiences.FirstOrDefault();
                    var usernameClaim = context.Principal?.FindFirst(ClaimTypes.Name)?.Value
                        ?? context.Principal?.Identity?.Name;

                    if (string.IsNullOrEmpty(tokenAudience) || string.IsNullOrEmpty(usernameClaim) ||
                        !string.Equals(tokenAudience, usernameClaim, StringComparison.Ordinal))
                    {
                        context.Fail("Token audience does not match username.");
                    }

                    return Task.CompletedTask;
                }
            };
        });

        // Add services to the container.
        builder.Services.AddControllers();
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}