namespace Domain.Entities
{
    public class Phone
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Additional { get; set; }
        public Office Office { get; set; } = null!;
    }
}
