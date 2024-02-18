namespace BackendAPI.DTOs
{
    public class AddPersonDto
    {
        public int LocationId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public int NumberOfPeople { get; set; }
        public string UserId { get; set; }
        public string CreateBy { get; set; }

    }
}
