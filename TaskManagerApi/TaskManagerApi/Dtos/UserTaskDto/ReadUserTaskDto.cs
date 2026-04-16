namespace TaskManagerApi.Dtos.UserTaskDto
{
    public class ReadUserTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public AdditionalInfoDto? AdditionalInfo { get; set; }
    }
}
