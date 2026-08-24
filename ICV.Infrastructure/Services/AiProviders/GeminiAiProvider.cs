using Google.GenAI;
using Google.GenAI.Types;
using ICV.Application.DTOs.AI;
using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.Interfaces.AI;
using ICV.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ICV.Application.DTOs.CvImport;

using SchemaType = Google.GenAI.Types.Type;

namespace ICV.Infrastructure.Services.AiProviders
{
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
                        Title = "Suggested Skill"
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
        You are an expert career advisor.

        Your task is NOT to find generic missing skills.

        Your task is to analyze the candidate's complete CV and recommend
        additional skills that would provide REAL VALUE to this specific
        candidate's career.

        TARGET PROFESSION:
        {professionName}

        COMPLETE CV:
        {cvContent}


        IMPORTANT:

        Analyze the candidate as an individual.

        Consider:

        - Education
        - Current profession
        - Work experience
        - Projects
        - Existing technical skills
        - Programming languages
        - Software/tools
        - Certificates
        - Languages
        - Current career level
        - Technologies already used
        - Technologies demonstrated through projects
        - Career direction that can reasonably be inferred


        CORE RULE:

        Recommend skills that are useful additions to THIS candidate's
        existing profile.

        Do NOT simply list skills that are commonly required for the profession.


        VERY IMPORTANT:

        A skill being common or popular in the profession does NOT mean
        that it should automatically be recommended.

        The recommendation must provide meaningful additional value.


        EXISTING SKILLS:

        Carefully identify skills the candidate already knows.

        A skill must NOT be recommended if the candidate clearly demonstrates
        knowledge or practical experience with it.

        Consider indirect evidence too.

        For example:

        If the candidate has several ASP.NET Core projects,
        do not recommend ASP.NET Core.

        If the candidate already demonstrates SQL through projects,
        do not recommend SQL.

        If the candidate already knows English,
        do not recommend English again.


        USEFUL ADDITIONAL SKILLS:

        Good recommendations can include things such as:

        - A new programming language
        - A useful software/tool
        - A complementary technology
        - A cloud technology
        - A useful engineering tool
        - A professional language
        - A certification-related skill
        - A domain-specific technology
        - A skill that expands employment opportunities
        - A skill that complements the candidate's existing profile


        EXAMPLE:

        If a Mechanical Engineer already knows:

        - English
        - SolidWorks
        - AutoCAD

        You may recommend:

        German

        because German can provide additional opportunities in
        German-speaking engineering companies and international
        manufacturing environments.

        Do NOT recommend AutoCAD or SolidWorks because the candidate
        already knows them.


        ANOTHER EXAMPLE:

        If a Computer Engineer already knows:

        - C#
        - ASP.NET Core
        - SQL Server
        - Git

        You may recommend:

        - Docker
        - Azure
        - Redis
        - React
        - CI/CD

        But only if the recommendation makes sense for the candidate's
        existing profile and career direction.


        DO NOT:

        - Recommend random trendy technologies.
        - Recommend skills only because they are frequently requested
          in job advertisements.
        - Recommend skills the candidate already clearly knows.
        - Recommend duplicate skills.
        - Recommend generic skills such as "communication" unless the CV
          provides a strong reason for it.
        - Recommend too many skills.


        NUMBER OF RECOMMENDATIONS:

        Return between 3 and 8 recommendations.

        Prefer quality over quantity.

        If only 4 recommendations are genuinely valuable,
        return 4 instead of inventing additional recommendations.


        CATEGORY:

        Use one concise category.

        Examples:

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
        Languages
        Engineering
        CAD
        Manufacturing
        Simulation
        Quality
        Optimization


        REASON:

        For every recommendation, explain specifically why this skill
        would benefit THIS candidate.

        The reason should be personalized.

        Do not use generic explanations.

        Example:

        BAD:
        "German is useful for engineers."

        GOOD:
        "Because the candidate already has English proficiency and a
        Mechanical Engineering background, German would expand access
        to engineering and manufacturing opportunities in Germany,
        Austria and Switzerland."


        TARGET LEVEL:

        Recommend the level the candidate should realistically aim for.

        1 = Beginner
        2 = Intermediate
        3 = Advanced

        Return ONLY the integer.

        Do not automatically recommend Advanced.

        The target level should depend on:

        - Candidate's current experience
        - Importance of the skill
        - Difficulty of the skill
        - Profession
        - Career direction


        OUTPUT:

        Return ONLY the JSON structure defined by the schema.
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

        public async Task<AiCvAnalysisResultDto> GenerateCvAnalysisAsync(
            CvForAiAnalysisDto cv,
            string professionName,
            CancellationToken cancellationToken = default)
        {
            if (cv == null)
            {
                throw new ArgumentNullException(nameof(cv));
            }

            if (string.IsNullOrWhiteSpace(professionName))
            {
                throw new ArgumentException(
                    "Profession name cannot be empty.",
                    nameof(professionName));
            }

            var analysisSchema = new Schema
            {
                Type = SchemaType.Object,

                Properties = new Dictionary<string, Schema>
                {
                    {
                        "score",
                        new Schema
                        {
                            Type = SchemaType.Number,
                            Title = "CV Score"
                        }
                    },

                    {
                        "summary",
                        new Schema
                        {
                            Type = SchemaType.String,
                            Title = "CV Summary"
                        }
                    },

                    {
                        "strengths",
                        new Schema
                        {
                            Type = SchemaType.Array,

                            Items = new Schema
                            {
                                Type = SchemaType.String
                            }
                        }
                    },

                    {
                        "weaknesses",
                        new Schema
                        {
                            Type = SchemaType.Array,

                            Items = new Schema
                            {
                                Type = SchemaType.String
                            }
                        }
                    },

                    {
                        "missingSkills",
                        new Schema
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
                                            Type = SchemaType.String
                                        }
                                    },

                                    {
                                        "category",
                                        new Schema
                                        {
                                            Type = SchemaType.String
                                        }
                                    },

                                    {
                                        "reason",
                                        new Schema
                                        {
                                            Type = SchemaType.String
                                        }
                                    }
                                },

                                Required = new List<string>
                                {
                                    "skill",
                                    "category",
                                    "reason"
                                },

                                PropertyOrdering = new List<string>
                                {
                                    "skill",
                                    "category",
                                    "reason"
                                }
                            }
                        }
                    },

                    {
                        "recommendations",
                        new Schema
                        {
                            Type = SchemaType.Array,

                            Items = new Schema
                            {
                                Type = SchemaType.String
                            }
                        }
                    }
                },

                Required = new List<string>
                {
                    "score",
                    "summary",
                    "strengths",
                    "weaknesses",
                    "missingSkills",
                    "recommendations"
                },

                PropertyOrdering = new List<string>
                {
                    "score",
                    "summary",
                    "strengths",
                    "weaknesses",
                    "missingSkills",
                    "recommendations"
                }
            };

            var cvJson = JsonSerializer.Serialize(
                cv,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            var prompt = $"""
                You are an expert career advisor, recruiter and CV analyzer.

                Analyze the candidate's ENTIRE CV for the target profession.

                TARGET PROFESSION:

                {professionName}

                IMPORTANT:

                Evaluate the candidate using ALL available CV information.

                Do NOT analyze only the Skills section.

                Consider together:

                - Profile
                - Summary
                - Education
                - Experience
                - Skills
                - Languages
                - Certificates
                - Projects
                - Dates
                - Job responsibilities
                - Project descriptions
                - Technologies used
                - Any other information contained in the CV

                Evaluate how these pieces of information work together.

                For example:

                - Education should be considered together with experience.
                - Skills should be compared with actual project and work experience.
                - Technologies mentioned in projects are stronger evidence than
                  technologies simply listed as skills.
                - Certificates should support the candidate's claimed knowledge.
                - Experience duration should be considered when evaluating skill level.
                - Projects should be considered when evaluating practical experience.

                Do NOT assume that a skill is strong just because it appears in
                the Skills section.

                TARGET PROFESSION ANALYSIS:

                Determine how suitable the candidate currently is for the target
                profession based on the complete CV.

                SCORE:

                Return a score between 0 and 100.

                The score should represent the candidate's overall suitability
                for the target profession.

                Consider:

                - Technical skills
                - Relevant education
                - Work experience
                - Project experience
                - Certificates
                - Languages
                - Practical evidence
                - Missing important skills
                - Overall career readiness

                STRENGTHS:

                Identify the candidate's strongest aspects.

                Focus on meaningful strengths supported by the CV.

                WEAKNESSES:

                Identify actual weaknesses or gaps in the CV.

                Do not invent weaknesses that cannot reasonably be inferred
                from the provided information.

                MISSING SKILLS:

                Identify important skills that the candidate should develop
                for the target profession.

                Rules:

                - Do not suggest skills the candidate clearly demonstrates.
                - Consider the candidate's existing experience.
                - Consider the candidate's projects.
                - Consider the target profession.
                - Avoid duplicate skills.
                - Prioritize skills that would meaningfully improve employability.
                - Return between 3 and 8 missing skills when possible.

                Categories should be concise names such as:

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
                Electronics
                Manufacturing
                CAD
                Simulation
                Quality
                Optimization

                RECOMMENDATIONS:

                Provide practical recommendations for improving the candidate's
                CV and professional readiness.

                Recommendations may include:

                - Skills to learn
                - Projects to build
                - Experience to gain
                - Certificates to obtain
                - CV improvements

                Do not recommend something the candidate already clearly has
                unless there is a meaningful reason to improve it.

                Return ONLY the requested JSON structure.

                COMPLETE CV:

                {cvJson}
                """;

            var response = await _client.Models.GenerateContentAsync(
                model: _options.Model,
                contents: prompt,

                config: new GenerateContentConfig
                {
                    ResponseMimeType = "application/json",
                    ResponseSchema = analysisSchema,
                    Temperature = 0.2
                },

                cancellationToken: cancellationToken);

            var responseText = response.Text;

            if (string.IsNullOrWhiteSpace(responseText))
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty CV analysis response.");
            }

            try
            {
                var result =
                    JsonSerializer.Deserialize<AiCvAnalysisResultDto>(
                        responseText,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (result == null)
                {
                    throw new InvalidOperationException(
                        "Gemini returned an empty CV analysis result.");
                }

                return result;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "Gemini returned an invalid CV analysis JSON response.",
                    ex);
            }
        }
        public async Task<ParsedCvDto> ParseCvAsync(
    string cvText,
    CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cvText))
            {
                throw new ArgumentException(
                    "CV text cannot be empty.",
                    nameof(cvText));
            }

            var parsedCvSchema = new Schema
            {
                Type = SchemaType.Object,

                Properties = new Dictionary<string, Schema>
        {
            {
                "summary",
                new Schema
                {
                    Type = SchemaType.String,
                    Nullable = true
                }
            },

            {
                "education",
                new Schema
                {
                    Type = SchemaType.Array,
                    Items = new Schema
                    {
                        Type = SchemaType.Object,

                        Properties = new Dictionary<string, Schema>
                        {
                            {
                                "title",
                                new Schema
                                {
                                    Type = SchemaType.String
                                }
                            },
                            {
                                "description",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            },
                            {
                                "startDate",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            },
                            {
                                "endDate",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            }
                        },

                        Required = new List<string>
                        {
                            "title",
                            "description",
                            "startDate",
                            "endDate"
                        }
                    }
                }
            },

            {
                "experience",
                new Schema
                {
                    Type = SchemaType.Array,
                    Items = new Schema
                    {
                        Type = SchemaType.Object,

                        Properties = new Dictionary<string, Schema>
                        {
                            {
                                "title",
                                new Schema
                                {
                                    Type = SchemaType.String
                                }
                            },
                            {
                                "description",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            },
                            {
                                "startDate",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            },
                            {
                                "endDate",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            }
                        },

                        Required = new List<string>
                        {
                            "title",
                            "description",
                            "startDate",
                            "endDate"
                        }
                    }
                }
            },

            {
                "skills",
                new Schema
                {
                    Type = SchemaType.Array,
                    Items = new Schema
                    {
                        Type = SchemaType.Object,

                        Properties = new Dictionary<string, Schema>
                        {
                            {
                                "name",
                                new Schema
                                {
                                    Type = SchemaType.String
                                }
                            }
                        },

                        Required = new List<string>
                        {
                            "name"
                        }
                    }
                }
            },

            {
                "languages",
                new Schema
                {
                    Type = SchemaType.Array,
                    Items = new Schema
                    {
                        Type = SchemaType.Object,

                        Properties = new Dictionary<string, Schema>
                        {
                            {
                                "name",
                                new Schema
                                {
                                    Type = SchemaType.String
                                }
                            },
                            {
                                "level",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            }
                        },

                        Required = new List<string>
                        {
                            "name",
                            "level"
                        }
                    }
                }
            },

            {
                "certificates",
                new Schema
                {
                    Type = SchemaType.Array,
                    Items = new Schema
                    {
                        Type = SchemaType.Object,

                        Properties = new Dictionary<string, Schema>
                        {
                            {
                                "title",
                                new Schema
                                {
                                    Type = SchemaType.String
                                }
                            },
                            {
                                "description",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            },
                            {
                                "startDate",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            }
                        },

                        Required = new List<string>
                        {
                            "title",
                            "description",
                            "startDate"
                        }
                    }
                }
            },

            {
                "projects",
                new Schema
                {
                    Type = SchemaType.Array,
                    Items = new Schema
                    {
                        Type = SchemaType.Object,

                        Properties = new Dictionary<string, Schema>
                        {
                            {
                                "title",
                                new Schema
                                {
                                    Type = SchemaType.String
                                }
                            },
                            {
                                "description",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            },
                            {
                                "startDate",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            },
                            {
                                "endDate",
                                new Schema
                                {
                                    Type = SchemaType.String,
                                    Nullable = true
                                }
                            }
                        },

                        Required = new List<string>
                        {
                            "title",
                            "description",
                            "startDate",
                            "endDate"
                        }
                    }
                }
            }
        },

                Required = new List<string>
        {
            "summary",
            "education",
            "experience",
            "skills",
            "languages",
            "certificates",
            "projects"
        },

                PropertyOrdering = new List<string>
        {
            "summary",
            "education",
            "experience",
            "skills",
            "languages",
            "certificates",
            "projects"
        }
            };

            var prompt = $"""
        You are an expert CV parser and recruitment specialist.

        Your task is to extract structured information from the CV text below.

        IMPORTANT RULES:

        - Extract ONLY information that is actually present in the CV.
        - Never invent information.
        - Never guess missing dates.
        - Never invent companies, schools, projects, certificates or skills.
        - Preserve the meaning of the original CV.
        - If information is missing, use null.
        - Do not add information based on what is common for the profession.
        - Do not generate career recommendations.
        - This task is ONLY CV information extraction.

        SUMMARY:

        Extract the candidate's profile/summary if one exists.

        EDUCATION:

        Extract each education record.

        Examples:

        - University
        - Faculty
        - Department
        - Degree
        - High school

        TITLE should contain the institution/degree/department information
        in a useful concise form.

        EXPERIENCE:

        Extract each work experience or internship.

        TITLE should contain the job title and/or company information.

        DESCRIPTION should contain the responsibilities and relevant details.

        SKILLS:

        Extract explicitly mentioned technical and professional skills.

        Examples:

        - C#
        - ASP.NET Core
        - SQL Server
        - AutoCAD
        - SolidWorks

        Do not invent skills.

        LANGUAGES:

        Extract languages mentioned in the CV.

        If a proficiency level is explicitly stated, include it.

        Examples:

        - English - B2
        - German - Intermediate

        CERTIFICATES:

        Extract certificates explicitly mentioned in the CV.

        PROJECTS:

        Extract projects explicitly mentioned in the CV.

        Include useful project descriptions and technologies when available.

        DATES:

        Dates should be returned as ISO-compatible date strings when possible.

        Examples:

        2024-09-01
        2025-06-30

        If only a year is available, use:

        2024-01-01

        If no date is available, return null.

        OUTPUT:

        Return ONLY the JSON structure defined by the provided schema.

        CV TEXT:

        {cvText}
        """;

            var response = await _client.Models.GenerateContentAsync(
                model: _options.Model,
                contents: prompt,

                config: new GenerateContentConfig
                {
                    ResponseMimeType = "application/json",
                    ResponseSchema = parsedCvSchema,
                    Temperature = 0.1
                },

                cancellationToken: cancellationToken);

            var responseText = response.Text;

            if (string.IsNullOrWhiteSpace(responseText))
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty CV parsing response.");
            }

            try
            {
                var result =
                    JsonSerializer.Deserialize<ParsedCvDto>(
                        responseText,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (result == null)
                {
                    throw new InvalidOperationException(
                        "Gemini returned an empty parsed CV result.");
                }

                return result;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "Gemini returned an invalid CV parsing JSON response.",
                    ex);
            }
        }
    }
}