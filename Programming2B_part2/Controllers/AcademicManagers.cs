using Microsoft.AspNetCore.Mvc;
using Programming2B_part2.Data;
using Programming2B_part2.Models;
using Programming2B_part2.Services;

namespace Programming2B_part2.Controllers
{
    public class AcademicManagersController : Controller
    {
   public IActionResult Index(string filter = "pending")
{
    try
    {
        
        var claims = ClaimSection.GetAllClaims();

        filter = filter.ToLower();
        claims = filter switch
        {
            "pending" => ClaimSection.GetClaimsByStatus(ClaimStatus.Pending),
            "approved" => ClaimSection.GetClaimsByStatus(ClaimStatus.Approved),
            "declined" => ClaimSection.GetClaimsByStatus(ClaimStatus.Declined),
            _ => claims
        };

        ViewBag.Filter = filter;
        ViewBag.PendingCount = ClaimSection.GetPendingCount();
        ViewBag.ApprovedCount = ClaimSection.GetApprovedCount();
        ViewBag.DeclinedCount = ClaimSection.GetDeclinedCount();

        return View(claims);
    }
    catch
    {
        ViewBag.Error = "Unable to load claims.";
        return View(new List<Claims>());
    }
}

        public IActionResult Review(int id)
        {
            var claim = ClaimSection.GetClaimById(id);
            if (claim == null)
            {
                TempData["Error"] = "Claim not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var success = ClaimSection.UpdateStatus(id, ClaimStatus.Approved);
            TempData[success ? "Success" : "Error"] = success ? "Claim approved." : "Claim not found.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Decline(int id, string comments)
        {
            if (string.IsNullOrWhiteSpace(comments))
            {
                TempData["Error"] = "Please provide a reason for declining.";
                return RedirectToAction(nameof(Review), new { id });
            }

            var success = ClaimSection.UpdateStatus(id, ClaimStatus.Declined);
            TempData[success ? "Success" : "Error"] = success ? "Claim declined." : "Claim not found.";
            return RedirectToAction(nameof(Index));
        }

        

    
    }

}
    
