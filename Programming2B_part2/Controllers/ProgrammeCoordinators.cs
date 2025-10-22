using Microsoft.AspNetCore.Mvc;
using Programming2B_part2.Data;
using Programming2B_part2.Models;

namespace Programming2B_part2.Controllers
{
    public class ProgrammeCoordinators : Controller
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
                    "verified" => ClaimSection.GetClaimsByStatus(ClaimStatus.Verified),
                    "declined" => ClaimSection.GetClaimsByStatus(ClaimStatus.Declined),
                    _ => claims
                };

                ViewBag.Filter = filter;
                ViewBag.PendingCount = ClaimSection.GetPendingCount();
                ViewBag.VerifiedCount = ClaimSection.GetVerifyCount();
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
            try
            {
                var claims = ClaimSection.GetClaimById(id);
                if (claims == null)
                {
                    TempData["Error"] = "claims not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(claims);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading claims.";
                return RedirectToAction(nameof(Index));
            }
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(int id)
        {
            try
            {
                var success = ClaimSection.UpdateStatus(id, ClaimStatus.Verified);

                if (success)
                {
                    TempData["Success"] = "Claim verified successfully!";

                    return RedirectToAction(nameof(Index), new { filter = "verified" });
                }
                else
                {
                    TempData["Error"] = "Claim not verified.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Error verifying claim.";
                return RedirectToAction(nameof(Index));
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id, string? comments)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(comments))
                {
                    TempData["Error"] = "Please provide a reason for declining.";
                    return RedirectToAction(nameof(Review), new { id });
                }


                var success = ClaimSection.UpdateStatus(id, ClaimStatus.Declined);

                if (success)
                {
                    TempData["Success"] = "claim declined.";
                }
                else
                {
                    TempData["Error"] = "claim not found.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error declining claim.";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}