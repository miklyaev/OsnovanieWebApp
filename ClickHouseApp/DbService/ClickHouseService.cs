
using ClickHouseApp.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octonica.ClickHouseClient;
using Polly;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClickHouseApp.DbService
{
    public interface IClickHouseService
    {
        public Task AddUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int id);

    }

    public class ClickHouseService : IClickHouseService
    {
        public readonly IConfiguration _configuration;
        private readonly ILogger<ClickHouseService> _logger;
        private readonly ClickHouseConnection _connection;
        private readonly RestClient _client;
        private readonly IAsyncPolicy<IRestResponse> _insertPolicy;

        private static readonly Regex ClickhouseExceptionRegex = new(@"DB::[A-Za-z]*Exception", RegexOptions.Compiled);

        public ClickHouseService(IConfiguration config, ILogger<ClickHouseService> log, IOptions<ClickhouseOptions> clickhouseOptions)
        {
            _configuration = config;
            _logger = log;
            var connectionString = clickhouseOptions.Value.ConnectionString;
            log.LogInformation("Using Clickhouse {ClickhouseConnectionString}", connectionString);

            //var sb = new ClickHouseConnectionStringBuilder(connectionString);
            //_connection = new ClickHouseConnection(sb);
            //_connection.Open();

            _client = new RestClient(connectionString)
            {
                Timeout = Timeout.Infinite
            };

            const int maxTryCount = 10;
            _insertPolicy = Policy
                .HandleResult<IRestResponse>(r => r.StatusCode != HttpStatusCode.OK)
                .WaitAndRetryAsync(maxTryCount, iteration => TimeSpan.FromSeconds(iteration / 2),
                (resultDelegate, _) =>
                {
                    var result = resultDelegate.Result;
                    _logger.LogWarning("Failed ClickHouse insert attempt. StatusCode: {StatusCode}. Response: {Response}", resultDelegate.Result.StatusCode, result.Content);
                });

        }

        public void UpdateUser(User user)
        {
            
        }
        public async Task AddUser(User user)
        {
            var sql = $"INSERT INTO t_first (id, name, weight) values ('{user.UserId}', '{user.UserName}', '{user.Weight}')";
            var responseFinal = await _insertPolicy.ExecuteAsync(async () =>
            {
                var response = await ExecuteInternalAsync(sql, isIgnoreFail: true).ConfigureAwait(false);
                return response;
            });
            if (responseFinal.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Отправка пакета не удалась");
            }
        }


        private async Task<IRestResponse> ExecuteInternalAsync(string sql, bool isIgnoreFail = false, params (string setting, string value)[] settings)
        {
            var request = new RestRequest("", Method.POST);
            foreach (var (setting, value) in settings)
            {
                request.AddQueryParameter(setting, value);
            }

            request.AddParameter("application/text; charset=utf-8", sql, ParameterType.RequestBody);

            var response = await _client.ExecuteTaskAsync(request).ConfigureAwait(false);

            if (!isIgnoreFail && response.StatusCode != HttpStatusCode.OK)
            {
                throw response.ErrorException ?? new Exception(response.Content);
            }

            if (ClickhouseExceptionRegex.IsMatch(response.Content))
            {
                throw new Exception(response.Content);
            }

            return response;
        }

        public void DeleteUser(int id)
        {
            //ApplicationContext db = new ApplicationContext();
            //TUser? user = db.Users.Where(x => x.UserId == id).FirstOrDefault();

            //if (user == null)
            //    return;

            //db.Users.Remove(user);
            //db.SaveChanges();
        }

    }
}
