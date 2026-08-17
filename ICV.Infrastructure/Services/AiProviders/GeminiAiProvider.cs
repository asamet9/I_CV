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
                        "skill",
                        "category",
                        "reason",
                        "recommendedTargetLevel"
                    },

                    PropertyOrdering = new List<string>
                    {
                        "skill",
                        "category",
                        "reason",
                        "recommendedTargetLevel"
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
    }
}