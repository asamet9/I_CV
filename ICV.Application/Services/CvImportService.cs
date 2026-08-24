using ICV.Application.DTOs.CvImport;
using ICV.Application.Interfaces.AI;
using ICV.Application.Interfaces.Services;
using ICV.Application.Interfaces.UnitOfWork;
using ICV.Domain.Entities;
using ICV.Domain.Enums;

namespace ICV.Application.Services
{
    public class CvImportService : ICvImportService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IPdfTextExtractor _pdfTextExtractor;
        private readonly IDocxTextExtractor _docxTextExtractor;
        private readonly IAiProvider _aiProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CvImportService(
            IFileStorageService fileStorageService,
            IPdfTextExtractor pdfTextExtractor,
            IDocxTextExtractor docxTextExtractor,
            IAiProvider aiProvider,
            IUnitOfWork unitOfWork)
        {
            _fileStorageService = fileStorageService;
            _pdfTextExtractor = pdfTextExtractor;
            _docxTextExtractor = docxTextExtractor;
            _aiProvider = aiProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<ImportedCvDto> ImportAsync(
            ImportCvRequestDto request,
            int userId,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.FileStream == null ||
                request.FileStream == Stream.Null)
            {
                throw new ArgumentException(
                    "CV file cannot be empty.",
                    nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.FileName))
            {
                throw new ArgumentException(
                    "File name cannot be empty.",
                    nameof(request));
            }

            if (request.ProfessionId <= 0)
            {
                throw new ArgumentException(
                    "Profession must be selected.",
                    nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException(
                    "CV title cannot be empty.",
                    nameof(request));
            }

            // -------------------------------------------------
            // 1. Profession kontrolü
            // -------------------------------------------------

            var profession = await _unitOfWork.Professions
                .GetByIdAsync(request.ProfessionId);

            if (profession == null)
            {
                throw new InvalidOperationException(
                    "Selected profession was not found.");
            }

            // -------------------------------------------------
            // 2. Dosya türünü belirle
            // -------------------------------------------------

            var extension = Path.GetExtension(request.FileName)
                .ToLowerInvariant();

            string cvText;

            switch (extension)
            {
                case ".pdf":

                    cvText = await _pdfTextExtractor.ExtractTextAsync(
                        request.FileStream,
                        cancellationToken);

                    break;

                case ".docx":

                    cvText = await _docxTextExtractor.ExtractTextAsync(
                        request.FileStream,
                        cancellationToken);

                    break;

                default:

                    throw new InvalidOperationException(
                        "Only PDF and DOCX files are supported.");
            }

            if (string.IsNullOrWhiteSpace(cvText))
            {
                throw new InvalidOperationException(
                    "No readable text could be extracted from the CV.");
            }

            // -------------------------------------------------
            // 3. Gemini ile CV parsing
            // -------------------------------------------------

            var parsedCv = await _aiProvider.ParseCvAsync(
                cvText,
                cancellationToken);

            if (parsedCv == null)
            {
                throw new InvalidOperationException(
                    "CV could not be parsed by AI.");
            }

            // -------------------------------------------------
            // 4. CV oluştur
            // -------------------------------------------------

            var cv = new Cv
            {
                UserId = userId,
                ProfessionId = request.ProfessionId,
                Title = request.Title,
                Summary = parsedCv.Summary,
                Source = CvSource.Uploaded
            };

            await _unitOfWork.Cvs.AddAsync(cv);

            // CV Id'sinin oluşması için önce kaydet.
            await _unitOfWork.SaveChangesAsync();

            // -------------------------------------------------
            // 5. Education
            // -------------------------------------------------

            if (parsedCv.Education.Any())
            {
                var section = new CvSection
                {
                    CvId = cv.Id,
                    Type = CvSectionType.Education,
                    OrderIndex = 1
                };

                await _unitOfWork.CvSections.AddAsync(section);

                await _unitOfWork.SaveChangesAsync();

                foreach (var education in parsedCv.Education)
                {
                    var item = new CvSectionItem
                    {
                        CvSectionId = section.Id,
                        Title = education.Title,
                        Description = education.Description,
                        StartDate = education.StartDate,
                        EndDate = education.EndDate
                    };

                    await _unitOfWork.CvSectionItems.AddAsync(item);
                }
            }

            // -------------------------------------------------
            // 6. Experience
            // -------------------------------------------------

            if (parsedCv.Experience.Any())
            {
                var section = new CvSection
                {
                    CvId = cv.Id,
                    Type = CvSectionType.Experience,
                    OrderIndex = 2
                };

                await _unitOfWork.CvSections.AddAsync(section);

                await _unitOfWork.SaveChangesAsync();

                foreach (var experience in parsedCv.Experience)
                {
                    var item = new CvSectionItem
                    {
                        CvSectionId = section.Id,
                        Title = experience.Title,
                        Description = experience.Description,
                        StartDate = experience.StartDate,
                        EndDate = experience.EndDate
                    };

                    await _unitOfWork.CvSectionItems.AddAsync(item);
                }
            }

            // -------------------------------------------------
            // 7. Skills
            // -------------------------------------------------

            if (parsedCv.Skills.Any())
            {
                var section = new CvSection
                {
                    CvId = cv.Id,
                    Type = CvSectionType.Skill,
                    OrderIndex = 3
                };

                await _unitOfWork.CvSections.AddAsync(section);

                await _unitOfWork.SaveChangesAsync();

                foreach (var skill in parsedCv.Skills)
                {
                    var item = new CvSectionItem
                    {
                        CvSectionId = section.Id,
                        Title = skill.Name
                    };

                    await _unitOfWork.CvSectionItems.AddAsync(item);
                }
            }

            // -------------------------------------------------
            // 8. Languages
            // -------------------------------------------------

            if (parsedCv.Languages.Any())
            {
                var section = new CvSection
                {
                    CvId = cv.Id,
                    Type = CvSectionType.Language,
                    OrderIndex = 4
                };

                await _unitOfWork.CvSections.AddAsync(section);

                await _unitOfWork.SaveChangesAsync();

                foreach (var language in parsedCv.Languages)
                {
                    var item = new CvSectionItem
                    {
                        CvSectionId = section.Id,
                        Title = language.Name,
                        Description = language.Level
                    };

                    await _unitOfWork.CvSectionItems.AddAsync(item);
                }
            }

            // -------------------------------------------------
            // 9. Certificates
            // -------------------------------------------------

            if (parsedCv.Certificates.Any())
            {
                var section = new CvSection
                {
                    CvId = cv.Id,
                    Type = CvSectionType.Certificate,
                    OrderIndex = 5
                };

                await _unitOfWork.CvSections.AddAsync(section);

                await _unitOfWork.SaveChangesAsync();

                foreach (var certificate in parsedCv.Certificates)
                {
                    var item = new CvSectionItem
                    {
                        CvSectionId = section.Id,
                        Title = certificate.Title,
                        Description = certificate.Description,
                        StartDate = certificate.StartDate
                    };

                    await _unitOfWork.CvSectionItems.AddAsync(item);
                }
            }

            // -------------------------------------------------
            // 10. Projects
            // -------------------------------------------------

            if (parsedCv.Projects.Any())
            {
                var section = new CvSection
                {
                    CvId = cv.Id,
                    Type = CvSectionType.Project,
                    OrderIndex = 6
                };

                await _unitOfWork.CvSections.AddAsync(section);

                await _unitOfWork.SaveChangesAsync();

                foreach (var project in parsedCv.Projects)
                {
                    var item = new CvSectionItem
                    {
                        CvSectionId = section.Id,
                        Title = project.Title,
                        Description = project.Description,
                        StartDate = project.StartDate,
                        EndDate = project.EndDate
                    };

                    await _unitOfWork.CvSectionItems.AddAsync(item);
                }
            }

            // -------------------------------------------------
            // 11. Fiziksel dosyayı Storage'a kaydet
            // -------------------------------------------------

            request.FileStream.Position = 0;

            var (storedFileName, storagePath) =
                await _fileStorageService.SaveAsync(
                    request.FileStream,
                    request.FileName);

            // -------------------------------------------------
            // 12. CvFile kaydı
            // -------------------------------------------------

            var cvFile = new CvFile
            {
                CvId = cv.Id,
                OriginalFileName = request.FileName,
                StoredFileName = storedFileName,
                StoragePath = storagePath,
                ContentType = request.ContentType,
                FileSize = request.FileStream.Length
            };

            await _unitOfWork.CvFiles.AddAsync(cvFile);

            // -------------------------------------------------
            // 13. Her şeyi DB'ye kaydet
            // -------------------------------------------------

            await _unitOfWork.SaveChangesAsync();

            // -------------------------------------------------
            // 14. Sonuç
            // -------------------------------------------------

            return new ImportedCvDto
            {
                CvId = cv.Id,
                Title = cv.Title,
                ProfessionId = cv.ProfessionId,
                Message = "CV successfully imported and parsed."
            };
        }
    }
}