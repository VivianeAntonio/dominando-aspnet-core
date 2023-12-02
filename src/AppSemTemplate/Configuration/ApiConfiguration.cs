namespace AppSemTemplate.Configuration
{
    public class ApiConfiguration
    {
        public const string ConfigName = "ApiConfiguration";

        public string? Domain { get; set; }
        public string? UserKey { get; set; }
        public string? UserSecret { get; set; }
    }
}
