using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TangyAPI.Data;
using TangyAPI.Models;
using TangyAPI.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidation();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(config =>
{
    // Category Mappings
    config.CreateMap<Category, CategoryDto>();
    config.CreateMap<CreateCategoryDto, Category>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.AddedDate, opt => opt.Ignore());
    config.CreateMap<UpdateCategoryDto, Category>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.AddedDate, opt => opt.Ignore());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

// Get all categories
app.MapGet("/api/categories", async (ApplicationDbContext db, CancellationToken ct) =>
{
    var categories = await db.Categories.ToListAsync(ct);

    return Results.Ok(categories);
}).WithName("GetAllCategories");

// Get category by id
app.MapGet("/api/categories/{id:int}", async (int id, ApplicationDbContext db, ILogger<Program> logger, CancellationToken ct) =>
{
    if (logger.IsEnabled(LogLevel.Information))
    {
        logger.LogInformation("Fetching category with id: {Id}", id);
    }

    if (id <= 0)
    {
        logger.LogWarning("Invalid category id: {Id}", id);
        return Results.BadRequest(new { Message = "Invalid category id." });
    }

    var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

    return category is not null ? Results.Ok(category) : Results.NotFound();
}).WithName("GetCategoryById");

// Create a category
app.MapPost("/api/categories", async (CreateCategoryDto createDto, ApplicationDbContext db, IMapper mapper, CancellationToken ct) =>
{
    var category = mapper.Map<Category>(createDto);
    category.AddedDate = DateTime.Now;

    await db.Categories.AddAsync(category, ct);
    await db.SaveChangesAsync(ct);

    var categoryDto = mapper.Map<CategoryDto>(category);

    return Results.Created($"/api/categories/{category.Id}", categoryDto);
}).WithName("CreateCategory")
.Accepts<CreateCategoryDto>("application/json")
.Produces<CategoryDto>(StatusCodes.Status201Created)
.ProducesProblem(StatusCodes.Status400BadRequest);

app.Run();