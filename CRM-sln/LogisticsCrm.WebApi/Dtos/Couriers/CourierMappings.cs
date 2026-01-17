using LogisticsCrm.Domain.Entities;

namespace LogisticsCrm.WebApi.Dtos.Couriers
{
    public static class CourierMappings
    {
        public static CourierResponseDto ToDto(this Courier courier)
        {
            return new CourierResponseDto
            {
                Id = courier.Id,
                FullName = courier.FullName,
                Phone = courier.Phone,
                IsActive = courier.IsActive
            };
        }
    }
}
