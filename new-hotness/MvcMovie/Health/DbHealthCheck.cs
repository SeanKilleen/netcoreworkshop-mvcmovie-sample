using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MvcMovie.Health
{

    public class DbHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public DbHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var connectionString = _configuration.GetConnectionString("MovieDBContext");

            var healthCheckResultHealthy = !string.IsNullOrEmpty(connectionString);

            return
                Task.FromResult(healthCheckResultHealthy
                                    ? HealthCheckResult.Healthy("A healthy result.")
                                    : HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}
