using ICV.Application.Interfaces.AI;
using ICV.Application.Interfaces.Repositories;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Application.Services;

using ICV.Infrastructure.Configuration;
using ICV.Infrastructure.Persistence.Context;
using ICV.Infrastructure.Persistence.Repositories;
using ICV.Infrastructure.Services;
using ICV.Infrastructure.Services.AiProviders;
using ICV.Infrastructure.Services.FileParsing;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// =====================================================
// Controllers
// =====================================================

builder.Services.AddControllers();


// =====================================================
// Application Services
// =====================================================

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICvService, CvService>();
builder.Services.AddScoped<ICvSectionService, CvSectionService>();
builder.Services.AddScoped<ICvSectionItemService, CvSectionItemService>();
builder.Services.AddScoped<IProfessionService, ProfessionService>();
builder.Services.AddScoped<IQuestionTemplateService, QuestionTemplateService>();
builder.Services.AddScoped<ISkillSuggestionService, SkillSuggestionService>();
builder.Services.AddScoped<ICourseRecommendationService, CourseRecommendationService>();
builder.Services.AddScoped<IUserSkillProgressService, UserSkillProgressService>();
builder.Services.AddScoped<IUserCvAnswerService, UserCvAnswerService>();
builder.Services.AddScoped<ICvBuilderService, CvBuilderService>();
builder.Services.AddScoped<ISkillDevelopmentGoalService, SkillDevelopmentGoalService>();
builder.Services.AddScoped<ICvAnalysisService, CvAnalysisService>();


// =====================================================
// CV Import Services
// =====================================================

builder.Services.AddScoped<ICvImportService, CvImportService>();
builder.Services.AddScoped<IPdfTextExtractor, PdfTextExtractor>();
builder.Services.AddScoped<IDocxTextExtractor, DocxTextExtractor>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<ICvFileService, CvFileService>();


// =====================================================
// Swagger
// =====================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "JWT token giriniz. Örnek: Bearer {token}"
        });

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});


// =====================================================
// Database
// =====================================================

Console.WriteLine(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// =====================================================
// JWT Authentication
// =====================================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings =
            builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings["Key"]!))
            };
    });


// =====================================================
// Repositories
// =====================================================

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICvRepository, CvRepository>();
builder.Services.AddScoped<IProfessionRepository, ProfessionRepository>();
builder.Services.AddScoped<ICvSectionRepository, CvSectionRepository>();
builder.Services.AddScoped<ICvSectionItemRepository, CvSectionItemRepository>();
builder.Services.AddScoped<ISkillSuggestionRepository, SkillSuggestionRepository>();
builder.Services.AddScoped<ICourseRecommendationRepository, CourseRecommendationRepository>();
builder.Services.AddScoped<IQuestionTemplateRepository, QuestionTemplateRepository>();
builder.Services.AddScoped<IUserSkillProgressRepository, UserSkillProgressRepository>();
builder.Services.AddScoped<ICvAnalysisRepository, CvAnalysisRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICvFileRepository, CvFileRepository>();

builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>));


// =====================================================
// Unit Of Work
// =====================================================

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// =====================================================
// Gemini AI
// =====================================================

builder.Services.Configure<GeminiOptions>(
    builder.Configuration.GetSection("Gemini"));

builder.Services.AddScoped<IAiProvider, GeminiAiProvider>();


// =====================================================
// Build
// =====================================================

var app = builder.Build();


// =====================================================
// Swagger
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// =====================================================
// Middleware
// =====================================================

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();