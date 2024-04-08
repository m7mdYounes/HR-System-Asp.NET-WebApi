
using finalProject.Models;
using finalProject.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace finalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string corss = "cors";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<APIContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("apicon")));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corss,
                builder =>
                {
                    builder.AllowAnyOrigin();
                   //.AllowAnyMethod()
                   //.AllowAnyHeader();
                    //builder.WithOrigins("http://localhost:4200");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
            builder.Services.AddScoped<IHolidaysRepository, HolidaysRepository>();
            builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
            builder.Services.AddScoped<ILogsRepo, LogsRepo>();
            builder.Services.AddScoped<INewEmployeeRepository, NewEmployeeRepository>();
            builder.Services.AddScoped<ISalaryReportRepo, SalaryReportRepo>();
            builder.Services.AddScoped<IRoleRepo, RoleRepo>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
               options =>
               {
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequiredLength = 8;
               }
               ).AddEntityFrameworkStores<APIContext>();



            builder.Services.AddAuthentication(option =>
                option.DefaultAuthenticateScheme = "myschema")
                    .AddJwtBearer("myschema", option =>
                    {
                        string key = "welcome to my account hr adminstrator jwtjwtjwtjwt";
                        var secertKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                        option.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = secertKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                        };
                    }
                        );



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(corss);

            app.MapControllers();

            app.Run();
        }
    }
}
