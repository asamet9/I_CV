using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ICV.Application.Interfaces.Repositories;
using ICV.Domain.Entities;
using ICV.Infrastructure.Persistence.Context;

namespace ICV.Infrastructure.Persistence.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}