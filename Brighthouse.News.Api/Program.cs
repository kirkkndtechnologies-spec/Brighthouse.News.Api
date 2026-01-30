using Brighthouse.News.Api.Features.ArticleDisplay;
using Brighthouse.News.Api.Features.ArticleManage;
using Brighthouse.News.Api.Infrastructure.Contexts;
using Brighthouse.News.Api.Infrastructure.Migrations;
using Brighthouse.News.Api.Infrastructure.Repositories;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// Configure the validators
builder.Services.AddValidatorsFromAssemblyContaining<ArticleAddValidator>();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "V1",
        Title = "Brighthouse news article services",
        Description = "The services to be consumed by client applications for the news"
    });

});

// Register Mapster.
builder.Services.AddMapster();
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// Register the logging framework.
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Register the dbcontext
builder.Services.AddDbContext<NewsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddScoped<NewsRepository>();
builder.Services.AddScoped<IArticleDisplayService, ArticleDisplayService>();
builder.Services.AddScoped<IArticleManageService, ArticleManageService>();

var app = builder.Build();

// Apply pending migrations.
app.MigrateDatabase();

// Register the swagger ui
app.UseSwagger(configuration => configuration.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("" + $"/swagger/v1/swagger.json", "Brighthouse News Services");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Register the minimal api endpoints
app.RegisterArticleDisplayEndpoints();
app.RegisterArticleManageEndpoints();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseRouting();
app.UseHttpsRedirection();

app.Run();
