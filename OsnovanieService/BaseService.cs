using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsnovanieService
{
    public class BaseService
    {
        public readonly Greeter.GreeterClient _client;
        public readonly IConfiguration _configuration;
        public readonly HttpClientHandler _httpHandler;
        public BaseService(IConfiguration config)
        {
            _configuration = config;

            // из-за апгрейда VS стала появляться ошибка, что локальный сертификат не относится к доверенным
            //AuthenticationException: The remote certificate is invalid because of errors in the certificate chain ....
            //этот код обходит эту ошибку, но применять его на проде нельзя
            _httpHandler = new HttpClientHandler();
            _httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(config["gRPCUrl"], new GrpcChannelOptions { HttpHandler = _httpHandler });
            _client = new Greeter.GreeterClient(channel);

        }
    }
}
