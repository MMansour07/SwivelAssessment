using Swivel.Core.Interfaces;
using Swivel.Core.Model;
using Swivel.Data.Identity;

namespace Swivel.Data.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(DataContext dbContext) : base(dbContext)
        {

        }
    }
}
