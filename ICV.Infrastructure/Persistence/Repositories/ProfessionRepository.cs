using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;
using ICV.Infrastructure.Persistence.Repositories;

public class ProfessionRepository : GenericRepository<Profession>, IProfessionRepository
{
    public ProfessionRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}