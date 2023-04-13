using System.Net.Security;

namespace QuickStart.Infra.Rabbitmq.ConfigOptions
{
    /// <summary>
    /// Ssl options.
    /// </summary>
    public class ServerSsloptions
    {
        /// <summary>
        /// If enabled ssl connection.
        /// </summary>
        public bool Enable { get; set; } = false;
        /// <summary>
        /// Server name for CA.
        /// </summary>
        public string ServerName { get; set; } = string.Empty;
        /// <summary>
        /// CA path.
        /// </summary>
        public string CertificatePath { get; set; } = string.Empty;
        /// <summary>
        /// CertificatePassphrase.
        /// </summary>
        public string CertificatePassphrase { get; set; } = string.Empty;
        /// <summary>
        /// SslPolicyErrors.
        /// </summary>
        public SslPolicyErrors? SslPolicyErrors { get; set; }
    }
}
