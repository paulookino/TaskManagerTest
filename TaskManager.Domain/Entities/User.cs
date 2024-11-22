using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ERole Role { get; set; }
    }

}
