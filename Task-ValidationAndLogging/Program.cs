
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Task_ValidationAndLogging.Data;
using Task_ValidationAndLogging.Dtos;
using Task_ValidationAndLogging.Error;

namespace Task_ValidationAndLogging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllers();
            var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(DefaultConnection);
            });
            builder.Services.AddExceptionHandler<GlobalExpetionHandler>();

            builder.Services.AddScoped<IValidator<CreateProductDtos>, CreateProductDtoValidation >();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Host.UseSerilog((context, configration) =>
            {
                configration.ReadFrom.Configuration(context.Configuration);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseExceptionHandler(options => { });

            app.MapControllers();

            app.Run();
        }
    }
}
