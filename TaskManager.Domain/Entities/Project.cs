namespace TaskManager.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public Guid UserId { get; set; }

        protected Project() { }

        public Project(string name, string description, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            UserId = userId;
        }
    }

}
