using Microsoft.AspNet.Identity;
using Swivel.Core.Dtos.General;
using Swivel.Core.Dtos.Job;
using Swivel.Core.Helper;
using Swivel.Service.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Swivel.Webclient.Controllers
{
    [Authorize(Roles = ERole.Admin)]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [OverrideAuthorization]
        [Authorize(Roles = ERole.Admin + "," + ERole.Employer)]
        [HttpPost]
        public async Task<ActionResult> Joblst(RequestModel<string> req)
        {
            req.Data = User.Identity.GetUserId();
            var response = await _jobService.GetJobsByUserIdAsync(req);
            if (response.Success)
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            else
                return Json(response, JsonRequestBehavior.AllowGet);
            // handle failure
            // navigate to acknowledge page
        }

        [OverrideAuthorization]
        [Authorize(Roles = ERole.Admin + "," + ERole.Employer)]
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [OverrideAuthorization]
        [Authorize(Roles = ERole.Admin + "," + ERole.Employer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateJob(NewJobDto req)
        {

            if (ModelState.IsValid)
            {
                if (req.Files != null)
                {
                    req.UserId = User.Identity.GetUserId();
                    var result = await _jobService.CreateJobAsync(req);
                    if (result.Success)
                        return Json(result, JsonRequestBehavior.AllowGet);
                    else
                        // handle failure
                        // navigate to acknowledge page
                        return Json(result, JsonRequestBehavior.AllowGet);
                }
                ModelState.AddModelError("", "Please upload at least 1 file!");
            }
            return View(req);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int Id)
        {
            var response = await _jobService.FindJobAsync(Id);
            if (response.Success)
                return View(response.Data);
            else
                return RedirectToAction("Error", "Handler");
            // handle failure
            // navigate to acknowledge page
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditJob(EditJobPostDto req)
        {

            if (ModelState.IsValid)
            {
                if (req.NewFiles == null && req.EliminatedIds?.Count == 3)
                {
                    ModelState.AddModelError("", "Please upload at least 1 file!");
                }
                else
                {
                    req.UserId = User.Identity.GetUserId();
                    var result = await _jobService.EditJobAsync(req);
                    if (result.Success)
                        return Json(result.Success, JsonRequestBehavior.AllowGet);
                    else
                        // handle failure
                        // navigate to acknowledge page
                        return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return View(req);

        }


        [HttpGet]
        public async Task<ActionResult> DeleteJob(int Id)
        {
            // not soft delete
            var response = await _jobService.DeleteJob(Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAll()
        {
            // not soft delete
            var response = await _jobService.DeleteAllJobs();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
