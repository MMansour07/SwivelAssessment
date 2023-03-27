
using Swivel.Core.Model;
using System.Threading.Tasks;

namespace Swivel.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IJobRepository jobRepository { get; }
        IMediaRepository mediaRepository { get; }
        Task<int> SaveChanges();
    }
}
