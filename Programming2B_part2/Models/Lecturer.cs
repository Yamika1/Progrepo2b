namespace Programming2B_part2.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }

        public string? LecturerName { get; set; }

        public string? Email { get; set; }

        public int ContactNumber { get; set; }
        public List<UploadedDocument> Documents { get; set; }

    }
}
