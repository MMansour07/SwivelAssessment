using Swivel.Core.Dtos.General;
using Swivel.Core.Dtos.Job;
using System.Threading.Tasks;

namespace Swivel.Service.Interfaces
{
    public interface IJobService
    {
        Task<ResponseModel<TableModel<JobDto>>> GetJobsByUserIdAsync(RequestModel<string> obj);
        Task<ResponseModel<NewJobDto>> CreateJobAsync(NewJobDto obj);
        Task<ResponseModel<EditJobDto>> FindJobAsync(int Id);
        Task<ResponseModel<EditJobPostDto>> EditJobAsync(EditJobPostDto obj);
        Task<ResponseModel<int>> DeleteAllJobs();
        Task<ResponseModel<int>> DeleteJob(int id);
    }
}
