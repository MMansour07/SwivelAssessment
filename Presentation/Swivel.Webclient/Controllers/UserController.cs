using Swivel.Service.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Swivel.Core.Dtos.User;
using Microsoft.Owin.Security;
using System.Web;
using Swivel.Core.Dtos.General;
using Swivel.Core.Helper;

namespace Swivel.Webclient.Controllers
{
    [Authorize(Roles = ERole.SuperAdmin + "," + ERole.Admin)]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Userlst(RequestModel<string> req)
        {
            var response = await _authService.GetUsers(req);
            if (response.Success)
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            else
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            // handle failure
            // navigate to acknowledge page
        }

        [OverrideAuthorization]
        [Authorize(Roles = ERole.SuperAdmin + "," + ERole.Admin + "," + ERole.Employer)]
        [HttpGet]
        public async Task<ActionResult> Details()
        {
            var response = await _authService.GetUserInfoAsync(User.Identity.GetUserId());
            if (response.Success)
                return View(response.Data);
            else
                return RedirectToAction("Error", "Handler");
            // handle failure
            // navigate to acknowledge page
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var response = await _authService.FindUserAsync(User.Identity.GetUserId());
            if (response.Success)
                return View(response.Data);
            else
                return RedirectToAction("Error", "Handler");
            // handle failure
            // navigate to acknowledge page
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditDto user)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.UpdateUserAsync(user);
                if (response.Success)
                    return RedirectToAction("Details");
                else
                    return RedirectToAction("Error", "Handler");
                // handle failure
                // navigate to acknowledge page

            }
            return View(user);
        }

        [HttpGet]
        public async Task<ActionResult> Delete()
        {
            var response = await _authService.FindUserAsync(User.Identity.GetUserId());
            if (response.Success)
                return View(response.Data);
            else
                return RedirectToAction("Error", "Handler");
            // handle failure
            // navigate to acknowledge page
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed()
        {
            // not soft delete
            var response = await _authService.DeleteUserAsync(User.Identity.GetUserId());
            if (response.Success)
            {
                if(response.Data.Succeeded)
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Login", "Account");
                }
                return View();
            }
            else
                return RedirectToAction("Error", "Handler");
            // handle failure
            // navigate to acknowledge page
        }

        [OverrideAuthorization]
        [Authorize(Roles = ERole.Employer + "," + ERole.Admin)]
        [HttpGet]
        public async Task<ActionResult> Land()
        {
            var response = await _authService.GetUserInfoAsync(User.Identity.GetUserId());
            if (response.Success)
                return View(response.Data);
            else
                return RedirectToAction("Error", "Handler");
            // handle failure
            // navigate to acknowledge page
        }

        //must be secured against users and only for superadmin for now depenent on show/hide
        [HttpGet]
        public async Task<ActionResult> AddUserToRole(string userId)
        {
            if(!string.IsNullOrEmpty(userId))
            {
                var response = await _authService.AddUserToRoleAsync(userId);
                if (response.Success)
                    return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet); // to be handled
        }

        //should be and send data in body
        [HttpGet]
        public async Task<ActionResult> RemoveUserFromRole(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var response = await _authService.RemoveUserFromRoleAsync(userId);
                if (response.Success)
                    return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet); // to be handled
        }
    }
}