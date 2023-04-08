using System.Net.Security;

namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    public class ServerSsloptions
    {
        public bool Enable { get; set; } = false;
        public string ServerName { get; set; } = string.Empty;
        public string CertificatePath { get; set; } = string.Empty;
        public string CertificatePassphrase { get; set; } = string.Empty;
        public SslPolicyErrors? SslPolicyErrors { get; set; }
    }
}
