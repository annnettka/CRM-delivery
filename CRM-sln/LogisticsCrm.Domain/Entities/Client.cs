using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCrm.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = null!;
        public string? ContactPerson { get; private set; }
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        public bool IsActive { get; private set; }

        private Client() { } // для EF Core

        public Client(string name, string? contactPerson, string? phone, string? email)
        {
            Id = Guid.NewGuid();

            Name = name ?? throw new ArgumentNullException(nameof(name));
            ContactPerson = contactPerson;
            Phone = phone;
            Email = email;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}

