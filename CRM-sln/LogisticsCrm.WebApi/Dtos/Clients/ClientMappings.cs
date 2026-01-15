using LogisticsCrm.Domain.Entities;

namespace LogisticsCrm.WebApi.Dtos.Clients
{
    public static class ClientMappings
    {
        public static ClientResponseDto ToDto(this Client client)
        {
            return new ClientResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                ContactPerson = client.ContactPerson,
                Phone = client.Phone,
                Email = client.Email,
                IsActive = client.IsActive
            };
        }
    }
}
