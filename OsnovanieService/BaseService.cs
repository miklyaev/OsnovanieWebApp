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
        protected readonly Greeter.GreeterClient _client;
        protected readonly IConfiguration _configuration;
        protected readonly HttpClientHandler _httpHandler;

        public BaseService(IConfiguration configuration)
        {
            _configuration = configuration;

            // из-за апгрейда VS стала появляться ошибка, что локальный сертификат не относится к доверенным
            //AuthenticationException: The remote certificate is invalid because of errors in the certificate chain ....
            //этот код обходит эту ошибку, но применять его на проде нельзя
            _httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel = GrpcChannel.ForAddress(configuration["gRPCUrl"], new GrpcChannelOptions { HttpHandler = _httpHandler });
            _client = new Greeter.GreeterClient(channel);

        }
    }
}
