using Domain.Enums;
using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("devices")]
    public class Device
    {
        // For EF
#pragma warning disable CA1041 // Provide ObsoleteAttribute message
        [Obsolete]
#pragma warning restore CA1041 // Provide ObsoleteAttribute message
        public Device() { }

        public Device(string name, string brand)
        {
            Name = name;
            Brand = brand;
            State = DeviceState.Available;
        }

        [Key]
        public Guid Id { get; private set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; private set; } = string.Empty;
        [Required]
        [MaxLength(250)]
        public string Brand { get; private set; } = string.Empty;
        [Required]
        public DeviceState State { get; private set; }
        [Required]
        public DateTimeOffset CreatedAt { get; private set; }

        public void Rename(string newName)
        {
            EnsureNotInUse("Name cannot be updated when device is in use.");
            Name = newName;
        }
        public void ChangeBrand(string newBrand)
        {
            EnsureNotInUse("Brand cannot be updated when device is in use.");
            Brand = newBrand;
        }
        public void ChangeState(DeviceState newState)
        {
            State = newState;
        }

        public void EnsureCanBeDeleted()
        {
            EnsureNotInUse("In-use devices cannot be deleted");
        }

        private void EnsureNotInUse(string message)
        {
            if (State == DeviceState.InUse)
                throw new DeviceInUseException(message);
        }
    }
}
