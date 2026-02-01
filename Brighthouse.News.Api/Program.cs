using Brighthouse.News.Api.Features.ArticleDisplay;
using Brighthouse.News.Api.Features.ArticleManage;
using Brighthouse.News.Api.Infrastructure.Contexts;
using Brighthouse.News.Api.Infrastructure.Migrations;
using Brighthouse.News.Api.Infrastructure.Repositories;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
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

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();

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

builder.Services.AddProblemDetails();

// Register the dbcontexts
builder.Services.AddDbContext<NewsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("NewsSqlLiteConnection")));

builder.Services.AddDbContext<SecurityDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SecuritySqlLiteConnection")));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<SecurityDbContext>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IArticleDisplayService, ArticleDisplayService>();
builder.Services.AddScoped<IArticleManageService, ArticleManageService>();

var app = builder.Build();

// Apply pending migrations.
app.MigrateNewsDatabase();
app.MigrateSecurityDatabase();

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
app.MapIdentityApi<IdentityUser>();
app.RegisterArticleDisplayEndpoints();
app.RegisterArticleManageEndpoints();

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();
