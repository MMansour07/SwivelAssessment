using Swivel.Core.Interfaces;
using Swivel.Data.Identity;
using Swivel.Data.Repositories;
using System.Threading.Tasks;

namespace Swivel.Data.Unity
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        private IJobRepository _jobRepository;
        private IMediaRepository _mediaRepository;
        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IJobRepository jobRepository
        {
            get
            {
                if (_jobRepository == null)
                    _jobRepository = new JobRepository(_context);

                return _jobRepository;
            }
        }

        public IMediaRepository mediaRepository
        {
            get
            {
                if (_mediaRepository == null)
                    _mediaRepository = new MediaRepository(_context);

                return _mediaRepository;
            }
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}