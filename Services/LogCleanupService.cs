
using Microsoft.Data.SqlClient;

namespace ECommerce.Services
{
    public class LogCleanupService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LogCleanupService> _logger;
        public LogCleanupService(IConfiguration configuration,ILogger<LogCleanupService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Log cleanup service is starting");
            while (!stoppingToken.IsCancellationRequested)
            {
                await DeleteOldLogs();
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
        private async Task DeleteOldLogs()
        {
            try
            {
                using(var Connection=new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string query = "DELETE FROM Logs WHERE TimeStamp < DATEADD(day,-30,GETDATE())";
                    SqlCommand command = new SqlCommand(query, Connection);
                    await Connection.OpenAsync();
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    _logger.LogInformation($"{affectedRows} log entries are deleted ");
                }
                _logger.LogInformation("one month old logs are deleted successfully");
            }catch(Exception ex)
            {
                _logger.LogInformation(ex, "error while cleaning up the logs");
            }
        }
    }
}
