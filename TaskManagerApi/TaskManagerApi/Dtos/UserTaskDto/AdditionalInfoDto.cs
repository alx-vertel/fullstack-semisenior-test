using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagerApi.Dtos.UserTaskDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserTaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public class AdditionalInfoDto
    {
        [EnumDataType(typeof(UserTaskPriority), ErrorMessage = "Priority is invalid. Allowed values are: Low, Medium, High.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserTaskPriority? Priority { get; set; } = UserTaskPriority.Medium;
        public DateTime? EstimatedEndDate { get; set; }
        public List<string> Tags { get; set; }
    }
}
