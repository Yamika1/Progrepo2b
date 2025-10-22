using Programming2B_part2.Models;

namespace Programming2B_part2.Data
{
    public class ClaimSection
    {
    
            private static List<Claims> _claims = new List<Claims>
        {
            new Claims
            {
                ClaimId = 1,
                ClaimName = "Claim 1",
                ClaimType = "Hourly Claim",
                ClaimMonth = "January",
                HoursWorked = 10,
                HourlyRate = "R250/Hr",
                Status = ClaimStatus.Pending,
              SubmittedDate = DateTime.Now.AddDays(-5),
                        Documents = new List<UploadedDocument>()
            },
              new Claims
            {
             ClaimId = 2,
                 ClaimName = "Claim 2",
                ClaimType = "Monthly Work claim",
                  ClaimMonth = "April",
                     HoursWorked = 4,
                HourlyRate = "R50/Hr",
                Status = ClaimStatus.Pending,
              SubmittedDate = DateTime.Now.AddDays(-3),
                        Documents = new List<UploadedDocument>()
            },
                new Claims
            {
               ClaimId = 3,
                 ClaimName = "Claim 3",
                ClaimType = "Consultation contract",
                  ClaimMonth = "March",
                     HoursWorked = 9,
                HourlyRate = "R300/Hr",
                Status = ClaimStatus.Pending,
              SubmittedDate = DateTime.Now.AddDays(-4),
                        Documents = new List<UploadedDocument>()
            },
                  new Claims
            {
                 ClaimId = 4,
                 ClaimName = "Claim 4",
                ClaimType = "Subcontractor Claim",
                  ClaimMonth = "August",
                     HoursWorked = 7,
                HourlyRate = "R200/Hr",
                Status = ClaimStatus.Pending,
              SubmittedDate = DateTime.Now.AddDays(-7),
                        Documents = new List<UploadedDocument>()
            },
  new Claims
            {
                 ClaimId = 5,
                 ClaimName = "Claim 5",
                ClaimType = "Overtime Claim",
                  ClaimMonth = "September",
                     HoursWorked = 17,
                HourlyRate = "R450/Hr",
                Status = ClaimStatus.Verified,
              SubmittedDate = DateTime.Now.AddDays(-8),
                        Documents = new List<UploadedDocument>()
            },
   new Claims
            {
                 ClaimId = 6,
                 ClaimName = "Claim 6",
                ClaimType = "commission Claim",
                  ClaimMonth = "December",
                     HoursWorked = 10,
                HourlyRate = "R400/Hr",
                Status = ClaimStatus.Verified,
              SubmittedDate = DateTime.Now.AddDays(-5),
                        Documents = new List<UploadedDocument>()
            },
    new Claims
            {
                 ClaimId = 7,
                 ClaimName = "Claim 7",
                ClaimType = "maintenance Claim",
                  ClaimMonth = "October",
                     HoursWorked = 19,
                HourlyRate = "R60/Hr",
                Status = ClaimStatus.Approved,
              SubmittedDate = DateTime.Now.AddDays(-4),
                        Documents = new List<UploadedDocument>()
            },
     new Claims
            {
                 ClaimId = 8,
                 ClaimName = "Claim 8",
                ClaimType = "expense Claim",
                  ClaimMonth = "February",
                     HoursWorked = 5,
                HourlyRate = "R350/Hr",
                Status = ClaimStatus.Approved,
              SubmittedDate = DateTime.Now.AddDays(-2),
                        Documents = new List<UploadedDocument>()
            },
    new Claims
            {
                 ClaimId = 9,
                 ClaimName = "Claim 9",
                ClaimType = "Fixed monthly Claim",
                  ClaimMonth = "July",
                     HoursWorked = 2,
                HourlyRate = "R210/Hr",
                Status = ClaimStatus.Declined,
              SubmittedDate = DateTime.Now.AddDays(-7),
                        Documents = new List<UploadedDocument>()
            }
        };
            private static int _Id = 20;
            public static List<Claims> GetAllClaims() => _claims.ToList();

            public static Claims? GetClaimById(int id) => _claims.FirstOrDefault(c => c.ClaimId == id);

            public static List<Claims> GetClaimsByStatus(ClaimStatus status)
                => _claims.Where(c => c.Status == status).ToList();

            public static void AddClaim(Claims claims)
            {
                claims.ClaimId = _Id;
                _Id++;
                claims.SubmittedDate = DateTime.Now;
                claims.Status = ClaimStatus.Pending;
                _claims.Add(claims);
            }
            public static bool UpdateStatus(int id, ClaimStatus newStatus)
            {
                var claim = _claims.FirstOrDefault(c => c.ClaimId == id);
                if (claim == null) return false;



                claim.Status = newStatus;
                claim.ReviewedDate = DateTime.Now;

                return true;
            }
            public static bool SubmitClaim(string claimName, string claimType, string claimMonth, List<UploadedDocument>? documents = null)
            {
                if (string.IsNullOrWhiteSpace(claimName) || string.IsNullOrWhiteSpace(claimType) || string.IsNullOrWhiteSpace(claimMonth))
                {

                    return false;
                }

                var newClaim = new Claims
                {
                    ClaimId = _Id++,
                    ClaimName = claimName,
                    ClaimType = claimType,
                    ClaimMonth = claimMonth,
                    Status = ClaimStatus.Pending,
                    SubmittedDate = DateTime.Now,
                    Documents = documents ?? new List<UploadedDocument>()
                };

                _claims.Add(newClaim);
                return true;
            }
            public static int GetPendingCount() => _claims.Count(b => b.Status == ClaimStatus.Pending);
            public static int GetApprovedCount() => _claims.Count(b => b.Status == ClaimStatus.Approved);
            public static int GetDeclinedCount() => _claims.Count(b => b.Status == ClaimStatus.Declined);
            public static int GetVerifyCount() => _claims.Count(b => b.Status == ClaimStatus.Verified);
        }
    }
