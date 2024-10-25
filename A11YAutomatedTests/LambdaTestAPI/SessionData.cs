using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace A11YAutomatedTests;


/// <summary>
/// 
/// </summary>
[DataContract]
public class SessionData
{
    [DataMember(Name = "test_id", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "test_id")]
    public string TestId { get; set; }

    [DataMember(Name = "build_id", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "build_id")]
    public int? BuildId { get; set; }

    [DataMember(Name = "name", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [DataMember(Name = "user_id", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "user_id")]
    public int? UserId { get; set; }

    [DataMember(Name = "username", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; }

    [DataMember(Name = "duration", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "duration")]
    public int? Duration { get; set; }

    [DataMember(Name = "platform", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "platform")]
    public string Platform { get; set; }

    [DataMember(Name = "browser", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "browser")]
    public string Browser { get; set; }

    [DataMember(Name = "browser_version", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "browser_version")]
    public string BrowserVersion { get; set; }

    [DataMember(Name = "device", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "device")]
    public string Device { get; set; }

    [DataMember(Name = "status_ind", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "status_ind")]
    public string StatusInd { get; set; }

    [DataMember(Name = "session_id", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "session_id")]
    public string SessionId { get; set; }

    [DataMember(Name = "build_name", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "build_name")]
    public string BuildName { get; set; }

    [DataMember(Name = "create_timestamp", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "create_timestamp")]
    public string CreateTimestamp { get; set; }

    [DataMember(Name = "start_timestamp", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "start_timestamp")]
    public string StartTimestamp { get; set; }

    [DataMember(Name = "end_timestamp", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "end_timestamp")]
    public string EndTimestamp { get; set; }

    [DataMember(Name = "remark", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "remark")]
    public string Remark { get; set; }

    [DataMember(Name = "console_logs_url", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "console_logs_url")]
    public string ConsoleLogsUrl { get; set; }

    [DataMember(Name = "network_logs_url", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "network_logs_url")]
    public string NetworkLogsUrl { get; set; }

    [DataMember(Name = "command_logs_url", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "command_logs_url")]
    public string CommandLogsUrl { get; set; }

    [DataMember(Name = "selenium_logs_url", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "selenium_logs_url")]
    public string SeleniumLogsUrl { get; set; }

    [DataMember(Name = "screenshot_url", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "screenshot_url")]
    public string ScreenshotUrl { get; set; }

    [DataMember(Name = "video_url", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "video_url")]
    public string VideoUrl { get; set; }

    [DataMember(Name = "public_url", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "public_url")]
    public string PublicUrl { get; set; }

    [DataMember(Name = "test_type", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "test_type")]
    public string TestType { get; set; }

    [DataMember(Name = "test_execution_status", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "test_execution_status")]
    public string TestExecutionStatus { get; set; }

    [DataMember(Name = "customData", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "customData")]
    public Object CustomData { get; set; }


    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
