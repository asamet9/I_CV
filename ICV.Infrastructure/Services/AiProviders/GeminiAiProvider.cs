using Google.GenAI;
using Google.GenAI.Types;
using ICV.Application.DTOs.AI;
using ICV.Application.Interfaces.AI;
using ICV.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

using SchemaType = Google.GenAI.Types.Type;

namespace ICV.Infrastructure.Services.AiProviders
{
    /// <summary>
    /// Google Gemini API ile iletişim kuran AI provider sınıfıdır.
    /// </summary>
    public class GeminiAiProvider : IAiProvider
    {
        private readonly GeminiOptions _options;
        private readonly Client _client;

        public GeminiAiProvider(IOptions<GeminiOptions> options)
        {
            _options = options.Value;

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new ArgumentException(
                    "Gemini API key cannot be empty.",
                    nameof(options));
            }

            _client = new Client(
                apiKey: _options.ApiKey);
        }

        public async Task<IEnumerable<AiSkillSuggestionDto>> GenerateSkillSuggestionsAsync(
            string cvContent,
            string professionName,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cvContent))
            {
                throw new ArgumentException(
                    "CV content cannot be empty.",
                    nameof(cvContent));
            }

            if (string.IsNullOrWhiteSpace(professionName))
            {
                throw new ArgumentException(
                    "Profession name cannot be empty.",
                    nameof(professionName));
            }

            var skillSuggestionSchema = new Schema
            {
                Type = SchemaType.Array,

                Items = new Schema
                {
                    Type = SchemaType.Object,

                    Properties = new Dictionary<string, Schema>
                    {
                        {
                            "skill",
                            new Schema
                            {
                                Type = SchemaType.String,
                                Title = "Skill"
                            }
                        },

                        {
                            "category",
                            new Schema
                            {
                                Type = SchemaType.String,
                                Title = "Category"
                            }
                        },

                        {
                            "reason",
                            new Schema
                            {
                                Type = SchemaType.String,
                                Title = "Reason"
                            }
                        },

                        {
                            "recommendedTargetLevel",
                            new Schema
                            {
                                Type = SchemaType.Integer,
                                Title = "Recommended Target Level"
                            }
                        }
                    },

                    Required = new List<string>
                    {
                       "category",
    "title",
    "provider",
    "url",
    "reason",
    "level",
    "isFree",
    "durationHours"
                    },

                    PropertyOrdering = new List<string>
{
    "category",
    "title",
    "provider",
    "url",
    "reason",
    "level",
    "isFree",
    "durationHours"
}
                }
            };

            var prompt = $"""
                You are an expert career advisor and CV analyzer.

                Analyze the following CV for the profession: {professionName}

                Identify the most valuable technical or professional skills
                that the candidate should develop.

                Rules:

                - Suggest only relevant skills.
                - Do not suggest skills the candidate already clearly demonstrates.
                - Focus on skills that improve employability.
                - Avoid duplicate skills.
                - Keep the number of suggestions between 3 and 8.

                Category should be a short category name such as:
                Backend, Frontend, DevOps, Database, Programming,
                Cloud, Testing or Security.

                Reason should briefly explain why the skill is valuable
                for this candidate.

                For every suggested skill, determine the recommended target
                level for this specific candidate.

                Skill levels are strictly represented by integers:

                1 = Beginner
                2 = Intermediate
                3 = Advanced

                For recommendedTargetLevel:
                - You MUST return exactly one integer: 1, 2, or 3.
                - Never return the text "Beginner", "Intermediate", or "Advanced".
                - Never return any other number.

                The recommended target level should represent the level
                the candidate should reasonably aim for based on:
                - their current CV
                - their existing technical experience
                - the profession
                - the importance of the skill
                - realistic career development

                Do not automatically recommend Advanced.
                Recommend Advanced only when it is realistically appropriate
                for the candidate and profession.

                Return ONLY the requested JSON structure.
                                COURSE CATEGORY:

                - Every course MUST have exactly one category.
                - The category must describe the main technical area of the course.
                - Use ONLY one of the following categories:

                Backend
                Frontend
                DevOps
                Database
                Programming
                Cloud
                Testing
                Security
                Mobile
                AI
                Data Science
                Tools

                - Do not invent new category names.
                - Do not return null or empty category.
                - The category must be relevant to the requested skill.

                CV:
                {cvContent}
                """;

            var response = await _client.Models.GenerateContentAsync(
                model: _options.Model,
                contents: prompt,

                config: new GenerateContentConfig
                {
                    ResponseMimeType = "application/json",
                    ResponseSchema = skillSuggestionSchema,
                    Temperature = 0.2
                },

                cancellationToken: cancellationToken);

            var responseText = response.Text;

            if (string.IsNullOrWhiteSpace(responseText))
            {
                return Enumerable.Empty<AiSkillSuggestionDto>();
            }

            try
            {
                var suggestions =
                    JsonSerializer.Deserialize<List<AiSkillSuggestionDto>>(
                        responseText,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                return suggestions ?? Enumerable.Empty<AiSkillSuggestionDto>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "Gemini returned an invalid JSON response.",
                    ex);
            }
        }

        public async Task<IEnumerable<AiCourseRecommendationDto>>
     GenerateCourseRecommendationsAsync(
         AiCourseSearchRequestDto request,
         CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.SkillName))
            {
                throw new ArgumentException(
                    "Skill name cannot be empty.",
                    nameof(request));
            }

            var courseRecommendationSchema = new Schema
            {
                Type = SchemaType.Array,

                Items = new Schema
                {
                    Type = SchemaType.Object,

                    Properties = new Dictionary<string, Schema>
            {

                        {
                         "category",
    new Schema
    {
        Type = SchemaType.String,
        Title = "Course Category"
    }
},
                {
                    "title",
                    new Schema
                    {
                        Type = SchemaType.String,
                        Title = "Course Title"
                    }
                },

                {
                    "provider",
                    new Schema
                    {
                        Type = SchemaType.String,
                        Title = "Course Provider"
                    }
                },

                {
                    "url",
                    new Schema
                    {
                        Type = SchemaType.String,
                        Title = "Course URL"
                    }
                },

                {
                    "reason",
                    new Schema
                    {
                        Type = SchemaType.String,
                        Title = "Recommendation Reason"
                    }
                },

                {
                    "level",
                    new Schema
                    {
                        Type = SchemaType.Integer,
                        Title = "Course Level"
                    }
                },

                {
                    "isFree",
                    new Schema
                    {
                        Type = SchemaType.Boolean,
                        Title = "Is Free"
                    }
                },

                {
                    "durationHours",
                    new Schema
                    {
                        Type = SchemaType.Integer,
                        Title = "Estimated Duration Hours"
                    }
                }
            },

                    Required = new List<string>
            {
                "title",
                "provider",
                "url",
                "reason",
                "level",
                "isFree",
                "durationHours"
            },

                    PropertyOrdering = new List<string>
            {
                "title",
                "provider",
                "url",
                "reason",
                "level",
                "isFree",
                "durationHours"
            }
                }
            };

            var prompt = $"""
        You are an expert career development advisor.

        Recommend real online courses for the user's skill development goal.

        USER DEVELOPMENT GOAL:

        Skill: {request.SkillName}

        Current Level: {request.CurrentLevel}

        Target Level: {request.TargetLevel}

        Preferred Duration: {request.PreferredDuration}

        Wants Paid Course: {request.WantsPaidCourse}

        Wants Certificate: {request.WantsCertificate}

        Purpose: {request.Purpose ?? "Not specified"}


        IMPORTANT RULES:

        - Recommend only real courses.
        - Never invent a course.
        - Never invent a provider.
        - Never invent a URL.
        - URL must point directly to the real course page.
        - Do not return search result URLs.
        - Do not return category pages.
        - Do not return homepage URLs.
        - Return only HTTPS URLs.
        - Do not return URLs that are only the platform homepage.
        - Prefer trusted platforms such as Udemy, Coursera, edX,
          Microsoft Learn, AWS Skill Builder, Google Cloud Skills Boost,
          freeCodeCamp, or similar trusted platforms.


        COURSE QUANTITY:

        - Recommend exactly 5 different courses whenever possible.
        - Every course must be relevant to the requested skill.
        - Do not recommend the same course twice.
        - Do not return duplicate URLs.
        - If fewer than 5 suitable real courses can be confidently identified,
          return only the courses that can be verified as real.
        - Never invent a course just to reach 5 recommendations.


        USER PREFERENCES:

        - Consider the user's current skill level.
        - Consider the user's target skill level.
        - Consider the preferred learning duration.
        - Respect whether the user wants paid or free courses.
        - If the user does not want paid courses, recommend only free courses.
        - If the user wants paid courses, paid courses may be recommended.
        - If the user wants a certificate, prioritize courses that provide
          a certificate.
        - Consider the user's purpose when selecting courses.


        COURSE LEVEL:

        1 = Beginner
        2 = Intermediate
        3 = Advanced

        You MUST return only 1, 2, or 3 for the level field.


        COURSE DURATION:

        - Return the estimated total learning duration in hours.
        - DurationHours must be a positive integer.
        - Estimate the duration based on the actual course whenever possible.
        - Do not use weeks.
        - Do not return 0.


        RECOMMENDATION REASON:

        - Explain briefly why the course is suitable for this user's goal.
        - Consider the user's current level, target level and purpose.


        FINAL REQUIREMENTS:

        - Return JSON only.
        - Follow the provided JSON schema exactly.
        """;

            var response = await _client.Models.GenerateContentAsync(
                model: _options.Model,
                contents: prompt,

                config: new GenerateContentConfig
                {
                    ResponseMimeType = "application/json",
                    ResponseSchema = courseRecommendationSchema,
                    Temperature = 0.2
                },

                cancellationToken: cancellationToken);

            var json = response.Text;

            if (string.IsNullOrWhiteSpace(json))
            {
                return Enumerable.Empty<AiCourseRecommendationDto>();
            }

            try
            {
                var result =
                    JsonSerializer.Deserialize<List<AiCourseRecommendationDto>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                return result ?? Enumerable.Empty<AiCourseRecommendationDto>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "Gemini returned an invalid JSON response.",
                    ex);
            }
        }



    }
}