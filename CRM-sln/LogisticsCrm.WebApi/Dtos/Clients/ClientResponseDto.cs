namespace LogisticsCrm.WebApi.Dtos.Clients
{
    public class ClientResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}
