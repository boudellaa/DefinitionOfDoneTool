using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskLibrary.Models
{
    public class TaskInfo
    {
        [JsonPropertyName("TaskID")]
        public int TaskID { get; set; }

        [JsonPropertyName("TaskTitle")]
        public string TaskTitle { get; set; }

        [JsonPropertyName("TaskType")]
        public string TaskType { get; set; }

        [JsonPropertyName("Developer")]
        public string Developer { get; set; }

        [JsonPropertyName("Guard")]
        public string Guard { get; set; }

    }
}

