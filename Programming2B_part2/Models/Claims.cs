namespace Programming2B_part2.Models
{
    public class Claims
    {
        public int ClaimId { get; set; }
        public string? ClaimName { get; set; }
        public string? ClaimType { get; set; }

        public int HoursWorked { get; set; }
        public string? HourlyRate { get; set; }

        public string? ClaimMonth { get; set; }
        public ClaimStatus Status { get; set; }

        public DateTime SubmittedDate { get; set; }

        public DateTime? ReviewedDate { get; set; }
        public List<UploadedDocument> Documents { get; set; }  = new List<UploadedDocument>();



    }


}
