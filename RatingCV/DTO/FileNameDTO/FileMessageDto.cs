using System.Text.Json.Serialization;

namespace RatingCV.DTO.FileNameDTO;

public class FileMessageDto
{
    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("file_content")]
    public string FileContent { get; set; }
}