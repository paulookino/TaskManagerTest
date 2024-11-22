using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public EStatus Status { get; set; }
        public EPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
        public List<TaskComment> Comments { get; set; } = new();
        public List<TaskHistory> History { get; private set; } = new();

        public Task(string title, string description, DateTime dueDate, EPriority priority, Guid projectId)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            DueDate = dueDate;
            Status = EStatus.Pending;
            Priority = priority;
            ProjectId = projectId;
        }
        protected Task() { }
        public void UpdateDetails(string title, string description, DateTime dueDate)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("O título é obrigatório.");
            if (dueDate < DateTime.Now) throw new ArgumentException("A data de vencimento não pode ser no passado.");

            History.Add(new TaskHistory(this.Id, "Detalhes atualizados", DateTime.UtcNow));

            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        public void UpdateStatus(EStatus newStatus)
        {
            if (newStatus == Status) return;

            History.Add(new TaskHistory(this.Id, $"Status alterado de {Status} para {newStatus}", DateTime.UtcNow));

            Status = newStatus;
        }
    }

}
