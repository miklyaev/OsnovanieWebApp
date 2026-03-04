using ClickHouseApp.DbService.Exceptions;
using ClickHouseApp.Dto;
using Microsoft.Extensions.Options;
using Octonica.ClickHouseClient;
using Polly;
using RestSharp;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
namespace ClickHouseApp.DbService
{
    public interface IClickHouseService
    {
        public Task AddUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int id);
        public Task AddSignal(Signal signal);
        public Task<bool> AddSignals(List<Signal> signals);

    }

    public class ClickHouseService : IClickHouseService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClickHouseService> _logger;
        private readonly ClickHouseConnection _connection;
        private readonly RestClient _client;
        private readonly IAsyncPolicy<string?> _insertPolicy;
        private readonly string _password;
        private readonly string _database;

        private static readonly Regex ClickhouseExceptionRegex = new(@"DB::[A-Za-z]*Exception", RegexOptions.Compiled);        public ClickHouseService(IConfiguration config, ILogger<ClickHouseService> log, IOptions<ClickhouseOptions> clickhouseOptions)
        {
            _configuration = config;
            _logger = log;
            var connectionString = clickhouseOptions.Value.ConnectionString;
            _password = clickhouseOptions.Value.Password;
            _database = clickhouseOptions.Value.Database;
            log.LogInformation("Using Clickhouse {ClickhouseConnectionString}", connectionString);

            var options = new RestClientOptions(connectionString)
            {
                Timeout = TimeSpan.FromMilliseconds(-1) // Эквивалент бесконечного ожидания
            };
            _client = new RestClient(options);

            const int maxTryCount = 10;
            _insertPolicy = Policy
                .HandleResult<string?>(content => content != null && ClickhouseExceptionRegex.IsMatch(content))
                .WaitAndRetryAsync(maxTryCount, iteration => TimeSpan.FromSeconds(iteration / 2),
                (resultDelegate, _) =>
                {
                    _logger.LogWarning("Failed ClickHouse insert attempt. Response: {Response}", resultDelegate.Result);
                });
        }

        public void UpdateUser(User user)
        {

        }

        public async Task AddUser(User user)
        {
            var weightFormatted = Convert.ToString(user.Weight, CultureInfo.InvariantCulture);
            var sql = $"INSERT INTO t_first (id, name, weight) values ('{user.UserId}', '{user.UserName}', '{weightFormatted}')";
            var content = await _insertPolicy.ExecuteAsync(async () =>
            {
                return await ExecuteInternalAsync(sql, isIgnoreFail: true, ("password", _password), ("database", _database)).ConfigureAwait(false);
            });
            
            if (content != null && ClickhouseExceptionRegex.IsMatch(content))
            {
                throw new Exception($"Ошибка ClickHouse: {content}");
            }
        }

        public async Task<bool> AddSignals(List<Signal> signals)
        {
            StringBuilder builder = new StringBuilder();
            var sql = $"INSERT INTO t_signal (id, TagName, TagType, TagValue) values ";

            foreach (var signal in signals)
            {
                // Форматируем значение TagValue в зависимости от реального типа
                var tagValueSql = FormatTagValueForSql(signal.TagValue);
                var tagNameEscaped = EscapeSqlString(signal.TagName);
                builder.Append($"('{signal.SignalId}', '{tagNameEscaped}', '{signal.TagType}', {tagValueSql}),");
            }

            sql += builder.ToString();
            var outStr = sql.TrimEnd(',') + ";";

            var content = await _insertPolicy.ExecuteAsync(async () =>
            {
                return await ExecuteInternalAsync(outStr, isIgnoreFail: true, ("password", _password), ("database", _database)).ConfigureAwait(false);
            });

            if (content != null && ClickhouseExceptionRegex.IsMatch(content))
            {
                throw new Exception($"Отправка пакета не удалась: {content}");
            }

            return true;
        }

        public async Task AddSignal(Signal signal)
        {
            var tagValueSql = FormatTagValueForSql(signal.TagValue);
            var tagNameEscaped = EscapeSqlString(signal.TagName);
            var sql = $"INSERT INTO t_signal (id, TagName, TagType, TagValue) values ('{signal.SignalId}', '{tagNameEscaped}', '{signal.TagType}', {tagValueSql})";

            var content = await _insertPolicy.ExecuteAsync(async () =>
            {
                return await ExecuteInternalAsync(sql, isIgnoreFail: true, ("password", _password), ("database", _database)).ConfigureAwait(false);
            });

            if (content != null && ClickhouseExceptionRegex.IsMatch(content))
            {
                throw new Exception($"Отправка пакета не удалась: {content}");
            }
        }

        private static string EscapeSqlString(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Replace("'", "''");
        }

        private static string FormatTagValueForSql(object? value)
        {
            if (value == null) return "NULL";

            switch (value)
            {
                case string s:
                    return $"'{EscapeSqlString(s)}'";
                case bool b:
                    // ClickHouse accepts 1/0 for boolean values
                    return b ? "1" : "0";
                case byte or sbyte or short or ushort or int or uint or long or ulong:
                    return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "0";
                case float or double or decimal:
                    return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "0";
                case Guid g:
                    return $"'{g}'";
                default:
                    var str = Convert.ToString(value, CultureInfo.InvariantCulture);
                    if (str == null) return "NULL";
                    return $"'{EscapeSqlString(str)}'";
            }
        }

        private async Task<string?> ExecuteInternalAsync(string sql, bool isIgnoreFail = false, params (string setting, string value)[] settings)
        {
            var request = new RestRequest("", Method.Post);
            foreach (var (setting, value) in settings)
            {
                request.AddQueryParameter(setting, value);
            }

            request.AddStringBody(sql, DataFormat.None);

            var response = await _client.ExecuteAsync(request).ConfigureAwait(false);

            if (!isIgnoreFail && response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("ClickHouse error: {StatusCode}. Content: {Content}", response.StatusCode, response.Content);
            }

            return response.Content;
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
