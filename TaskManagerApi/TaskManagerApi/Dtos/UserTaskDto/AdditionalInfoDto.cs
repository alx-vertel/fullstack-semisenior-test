namespace TaskManagerApi.Dtos.UserTaskDto
{
    public class AdditionalInfoDto
    {
        public string Priority { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public List<string> Tags { get; set; }
    }
}
