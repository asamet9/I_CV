using ICV.Application.Interfaces.Repositories;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Application.Services;

using ICV.Infrastructure.Persistence.Context;
using ICV.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICvRepository, CvRepository>();
builder.Services.AddScoped<IProfessionRepository, ProfessionRepository>();
builder.Services.AddScoped<ICvSectionRepository, CvSectionRepository>();
builder.Services.AddScoped<ICvSectionItemRepository, CvSectionItemRepository>();
builder.Services.AddScoped<ISkillSuggestionRepository, SkillSuggestionRepository>();
builder.Services.AddScoped<ICourseRecommendationRepository, CourseRecommendationRepository>();
builder.Services.AddScoped<IQuestionTemplateRepository, QuestionTemplateRepository>();
builder.Services.AddScoped<IUserSkillProgressRepository, UserSkillProgressRepository>();

// Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();