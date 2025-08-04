using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookie_Session.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize]
        [HttpGet("profile")]
       public  IActionResult UserProfile()
        {
            string? UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int VisitProfile = HttpContext.Session.GetInt32("VisitProfileCount") ?? 0;

            ++VisitProfile;

            HttpContext.Session.SetInt32("VisitProfileCount", VisitProfile);

            var Profile = new
            {
                Id = UserId,
                VisitedProfileCount = VisitProfile
            };

            return Ok(Profile);
        }
    }
}
