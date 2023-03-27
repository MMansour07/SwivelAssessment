using AutoMapper;
using Swivel.Core.Dtos.General;
using Swivel.Core.Dtos.Job;
using Swivel.Core.Interfaces;
using Swivel.Core.Model;
using Swivel.Service.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using System.Configuration;
using System;
using CloudinaryDotNet.Actions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;

namespace Swivel.Service.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _autoMapper;
        public JobService(IMapper autoMapper, IUnitOfWork unitOfWork)
        {
            _autoMapper = autoMapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel<TableModel<JobDto>>> GetJobsByUserIdAsync(RequestModel<string> obj)
        {
            try
            {
                var jobs = _unitOfWork.jobRepository.Filter(i => i.UserId == obj.Data, obj.sort, obj.query, i => i.Title.ToLower().Contains(obj.query.generalSearch));
                return new ResponseModel<TableModel<JobDto>>()
                {
                    Success = true,
                    Data = _autoMapper.Map<PagedList<Job>, TableModel<JobDto>>(await PagedList<Job>.Create(jobs, obj.pagination.page, obj.pagination.perpage, jobs.Count()))
                };
            }
            catch (Exception ex)
            {
                //logs + roll back
                return new ResponseModel<TableModel<JobDto>>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<NewJobDto>> CreateJobAsync(NewJobDto obj)
        {
            try
            {
                Cloudinary _cloudinary = new Cloudinary(new Account()
                {
                    Cloud = ConfigurationManager.AppSettings["CloudName"],
                    ApiKey = ConfigurationManager.AppSettings["ApiKey"],
                    ApiSecret = ConfigurationManager.AppSettings["ApiSecret"],
                });

                IList<VideoUploadResult> videoUploadResult = new List<VideoUploadResult>();

                if (obj.Files?.Count > 0)
                {
                    for (int i = 0; i < obj.Files.Count; i++)
                    {
                        using (var stream = obj.Files[i].InputStream)
                        {
                            var response = await _cloudinary.UploadAsync(new VideoUploadParams() { File = new FileDescription(obj.Files[i].FileName, stream) });
                            if (response.StatusCode == HttpStatusCode.OK)
                                videoUploadResult.Add(response);
                            else
                                break;
                                // roll back to achieve unit of work
                        }
                    }
                }

                var result =  _unitOfWork.jobRepository.Add(obj.ToJob(obj, _autoMapper.Map<NewJobDto, Job>(obj), _autoMapper.Map<IList<VideoUploadResult>, IList<Media>>(videoUploadResult)));

                await _unitOfWork.SaveChanges();

                return new ResponseModel<NewJobDto>() { Data = _autoMapper.Map<Job, NewJobDto>(result), Success = true};
            }
            catch (Exception ex)
            {
                //logs + roll back
                return new ResponseModel<NewJobDto>() {Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<EditJobDto>> FindJobAsync(int Id)
        {
            try
            {
                return new ResponseModel<EditJobDto>() {Data = _autoMapper.Map<Job, EditJobDto>(await _unitOfWork.jobRepository.FindBy(i => i.Id == Id).FirstOrDefaultAsync()), Success = true };
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<EditJobDto>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<EditJobPostDto>> EditJobAsync(EditJobPostDto obj)
        {
            try
            {
                Cloudinary _cloudinary = new Cloudinary(new Account()
                {
                    Cloud = ConfigurationManager.AppSettings["CloudName"],
                    ApiKey = ConfigurationManager.AppSettings["ApiKey"],
                    ApiSecret = ConfigurationManager.AppSettings["ApiSecret"],
                });

                List<VideoUploadResult> videoUploadResult = new List<VideoUploadResult>();
                DelResResult delresponse = new DelResResult();

                if(obj.PublicIds != null)
                {
                     delresponse = await _cloudinary.DeleteResourcesAsync(ResourceType.Video, obj.PublicIds.ToArray());
                }

                if (delresponse.StatusCode == HttpStatusCode.OK || obj.PublicIds == null)
                {

                    if (obj.NewFiles != null)
                    {
                        for (int i = 0; i < obj.NewFiles.Count; i++)
                        {
                            using (var stream = obj.NewFiles[i].InputStream)
                            {
                                var response = await _cloudinary.UploadAsync(new VideoUploadParams() { File = new FileDescription(obj.NewFiles[i].FileName, stream) });
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                    videoUploadResult.Add(response);
                                else
                                    break;
                                // roll back to achieve unit of work
                            }
                        }
                    }
                    if (obj.EliminatedIds != null)
                    await _unitOfWork.mediaRepository.Delete(i => obj.EliminatedIds.Contains(i.Id));

                    if (videoUploadResult.Count > 0)
                        _unitOfWork.mediaRepository.AddRange(obj.ToMedialst(obj, _autoMapper.Map<List<VideoUploadResult>, List<Media>>(videoUploadResult)));

                    _unitOfWork.mediaRepository.UpdateMedia(obj.Id, obj.Titles, obj.PublicIds);

                    _unitOfWork.jobRepository.UpdateAsync(_autoMapper.Map<EditJobPostDto, Job>(obj));

                    await _unitOfWork.SaveChanges();
                }
                return new ResponseModel<EditJobPostDto>() { Data = obj, Success = true };
            }
            catch (Exception ex)
            {
                //logs + roll back
                return new ResponseModel<EditJobPostDto>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }


        public async Task<ResponseModel<int>> DeleteAllJobs()
        {
            try
            {
                // ask if we should delete video from the cloud or not
                await _unitOfWork.jobRepository.Delete(i => i.Id > 0);
                
                return new ResponseModel<int>() { Data = await _unitOfWork.SaveChanges(), Success = true };
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<int>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

        public async Task<ResponseModel<int>> DeleteJob(int id)
        {
            try
            {
                // ask if we should delete video from the cloud or not

                await _unitOfWork.jobRepository.Delete(i => i.Id == id);

                return new ResponseModel<int>() { Data = await _unitOfWork.SaveChanges(), Success = true };
            }
            catch (Exception ex)
            {
                //logs
                return new ResponseModel<int>() { Ex = ex, Message = ex.Message, Success = false };
            }
        }

    }
}
