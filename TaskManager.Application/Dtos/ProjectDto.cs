namespace TaskManager.Application.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskCount { get; set; }
    }

}
