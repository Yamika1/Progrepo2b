using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Programming2B_part2.Data;
using Programming2B_part2.Models;
using Programming2B_part2.Services;

namespace Programming2B_part2.Controllers
{
    public class LecturerController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly FileEncryptionService _encryptionService;

        public LecturerController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _encryptionService = new FileEncryptionService();
        }

       
        public IActionResult Index()
        {
            try
            {
                var claims = ClaimSection.GetAllClaims();
                return View(claims);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Unable to load books";
                return View(new List<Claims>());
            }

        }

        public IActionResult AddClaim()
        {
            return View();
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClaim(List<IFormFile> documents, Claims claims)
        {
            try
            {
                if (string.IsNullOrEmpty(claims.ClaimName))
                {
                    ViewBag.Error = "claim name is required";
                    return View( claims);
                }

                if (string.IsNullOrEmpty(claims.ClaimType))
                {
                    ViewBag.Error = "claim type is required";
                    return View(claims);
                }


                if (documents != null && documents.Count > 0)
                {
                    foreach (var file in documents)
                    {
                        if (file.Length > 0)
                        {
                            var allowedExtensions = new[] { ".pdf", ".docx", ".txt", ".xlsx" };
                            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                            if (!allowedExtensions.Contains(extension))
                            {
                                ViewBag.Error = $"File extension {extension} not allowed";
                                return View(claims);
                            }

                            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                            Directory.CreateDirectory(uploadsFolder);

                            var uniqueFileName = Guid.NewGuid().ToString() + ".encrypted";
                            var encryptedFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = file.OpenReadStream())
                            {
                                await _encryptionService.EncryptFileAsync(fileStream, encryptedFilePath);
                            }

                            claims.Documents.Add(new UploadedDocument
                            {
                                FileName = file.FileName,
                                FilePath = "/uploads/" + uniqueFileName,
                                FileSize = file.Length,
                                IsEncrypted = true
                            });

                        }
                    }
                }

                ClaimSection.AddClaim(claims);
                TempData["Success"] = "claim submitted successfully";
                return RedirectToAction(nameof(Index));


            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error submitting claim: " + ex.Message;
                return View(claims);
            }

        }



        public IActionResult Details(int id)
        {
            try
            {
                var claim = ClaimSection.GetClaimById(id);
                if (claim == null)
                {
                    TempData["Error"] = "Book not found.";
                    return View();
                }
                return View(claim);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error loading book";
                return RedirectToAction(nameof(Index));
            }
        }

        // upload documents

        public async Task<IActionResult> DownloadDocument(int claimId, int docId)
        {
            try
            {
                var claim = ClaimSection.GetClaimById(claimId);
                if (claim == null)
                    return NotFound("Claim not found.");

                var doc = claim.Documents.FirstOrDefault(d => d.Id == docId);
                if (doc == null)
                    return NotFound("Document not found.");

                var filePath = Path.Combine(_environment.WebRootPath, doc.FilePath.TrimStart('/'));
                if (!System.IO.File.Exists(filePath))
                    return NotFound("File not found.");

                using var decryptedStream = await _encryptionService.DecryptFileStream(filePath);

                var contentType = Path.GetExtension(doc.FileName).ToLower() switch
                {
                    ".pdf" => "application/pdf",
                    ".txt" => "text/plain",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    _ => "application/octet-stream"
                };

                
                return File(decryptedStream.ToArray(), contentType, doc.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading file: {ex.Message}");
            }
        }
    }
}
