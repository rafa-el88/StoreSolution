using Newtonsoft.Json;

namespace StoreSolution.Server.Configuration
{
    public class AppSettings
    {
        //[JsonProperty("smtpConfig")]
        public SmtpConfig? SmtpConfig { get; set; }
    }

    public class SmtpConfig
    {
        //[JsonProperty("host")]
        public required string Host { get; set; }

        //[JsonProperty("port")]
        public int Port { get; set; }

        //[JsonProperty("useSSL")]
        public bool UseSSL { get; set; }

        //[JsonProperty("emailAddress")]
        public required string EmailAddress { get; set; }

        //[JsonProperty("name")]
        public string? Name { get; set; }

        //[JsonProperty("username")]
        public string? Username { get; set; }

        //[JsonProperty("password")]
        public string? Password { get; set; }
    }
}
