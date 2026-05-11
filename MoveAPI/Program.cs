using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoveAPI.Models;
using MoveAPI.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ConnectionString =builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(ConnectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IGenresServices, GenresServicse>();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(option => 
{
    option.SwaggerDoc(name: "v1",info:new OpenApiInfo
    {
        Version = "V1",
        Title = "Move API",
        Description = "My First API",
        TermsOfService = new Uri("https://www.Google.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Mostafa Ayman",
            Email = "mostafaayman6644@example.com",
            Url = new Uri("https://www.Google.com")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    option.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme 
    {
        Name ="Authorization",
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer",
        BearerFormat="JWT",
        In =ParameterLocation.Header,
        Description= "Enter you JWT token in the format: Bearer {your token}"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                In= ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}   

app.UseHttpsRedirection();
app.UseCors(b=>b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
