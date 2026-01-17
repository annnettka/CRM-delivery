namespace LogisticsCrm.Domain.Entities
{
    public class Courier
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; } = null!;
        public string Phone { get; private set; } = null!;
        public bool IsActive { get; private set; }

        private Courier() { } 

        public Courier(string fullName, string phone)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Phone = phone;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
