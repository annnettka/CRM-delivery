namespace LogisticsCrm.WebApi.Dtos.Couriers
{
    public class CourierResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
