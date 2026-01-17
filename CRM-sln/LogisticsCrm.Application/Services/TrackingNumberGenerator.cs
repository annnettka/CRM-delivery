namespace LogisticsCrm.Application.Services
{
    public class TrackingNumberGenerator : ITrackingNumberGenerator
    {
        public string Generate()
        {
            // простий унікальний формат: TRK-YYYYMMDD-XXXXXXXX
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var suffix = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            return $"TRK-{date}-{suffix}";
        }
    }
}
