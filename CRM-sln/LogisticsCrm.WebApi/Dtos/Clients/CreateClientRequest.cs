namespace LogisticsCrm.WebApi.Dtos.Clients
{
    public class CreateClientRequest
    {
        public string Name { get; set; } = null!;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
