namespace TaskManager.Domain.Entities
{
    public class TaskHistory
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string ChangeDetails { get; set; }
        public DateTime ChangeDate { get; set; }
        public Guid ChangedByUserId { get; set; }

        public TaskHistory(Guid taskId, string changeDetails, DateTime changeDate)
        {
            this.TaskId = taskId;
            this.ChangeDetails = changeDetails;
            this.ChangeDate = changeDate;
        }
    }

}
