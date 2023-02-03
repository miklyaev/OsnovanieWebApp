using Confluent.Kafka;

namespace KafkaLibNetCore
{
    class StaticConfig
    {
        public static SecurityProtocol GetSecurityProtocol(string protokol)
        {
            switch (protokol)
            {
                case "PLAIN_TEXT":
                    return SecurityProtocol.Plaintext;
                case "SASL_PLAIN_TEXT":
                    return SecurityProtocol.SaslPlaintext;
                case "SASL_SSL":
                    return SecurityProtocol.SaslSsl;
                case "SSL":
                    return SecurityProtocol.Ssl;
                default:
                    return SecurityProtocol.Plaintext;

            }
        }

        public static SaslMechanism GetSaslMechanism(string value)
        {
            switch (value)
            {
                case "GSS_API":
                    return SaslMechanism.Gssapi;
                case "OAUTH_BEARER":
                    return SaslMechanism.OAuthBearer;
                case "PLAIN":
                    return SaslMechanism.Plain;
                case "SHA256":
                    return SaslMechanism.ScramSha256;
                case "SHA512":
                    return SaslMechanism.ScramSha512;

                default:
                    return SaslMechanism.Gssapi;

            }
        }

        public static AutoOffsetReset GetAutoOffsetReset(string value)
        {
            switch (value)
            {
                case "EARLIEST":
                    return AutoOffsetReset.Earliest;
                case "ERROR":
                    return AutoOffsetReset.Error;
                case "LATEST":
                    return AutoOffsetReset.Latest;
                default:
                    return AutoOffsetReset.Latest;

            }
        }
    }
}
