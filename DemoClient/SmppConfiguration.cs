using JamaaTech.Smpp.Net.Lib;

namespace DemoClient
{
    public class SmppConfiguration : ISmppConfiguration
    {
        public int Id { get; set; }
        public int AutoReconnectDelay { get; set; }
        public string DefaultServiceType { get; set; }
        public DataCoding Encoding { get; set; }
        public string Host { get; set; }
        public bool IgnoreLength { get; set; }
        public int KeepAliveInterval { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int ReconnectInteval { get; set; }
        public string SourceAddress { get; set; }
        public string SystemId { get; set; }
        public string SystemType { get; set; }
        public int TimeOut { get; set; }
        public bool StartAutomatically { get; set; }
        public string DestinationAddressRegex { get; set; }
    }
}
