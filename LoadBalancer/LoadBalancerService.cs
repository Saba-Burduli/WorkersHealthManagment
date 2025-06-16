namespace LoadBalancer;

public class LoadBalancerService
{
    private readonly ConsistentHashRing _hashRing;
    private readonly ILogger<LoadBalancerService> _logger;
    private readonly Dictionary<string, ServerHealth> _serverHealth;

    public LoadBalancerService(ILogger<LoadBalancerService> logger)
    {
        _hashRing = new ConsistentHashRing();
        _logger = logger;
        _serverHealth = new Dictionary<string, ServerHealth>();
        InitializeServers();
    }

    private void InitializeServers()
    {
        var servers = new[] { "API-Server-1", "API-Server-2", "API-Server-3" };
            
        foreach (var server in servers)
        {
            _hashRing.AddServer(server);
            _serverHealth[server] = new ServerHealth
            {
                ServerName = server,
                IsHealthy = true,
                LastHealthCheck = DateTime.UtcNow,
                RequestCount = 0,
                ResponseTime = TimeSpan.Zero
            };
        }
}

        public string RouteRequest(string requestKey)
        {
            try
            {
                var selectedServer = _hashRing.GetServer(requestKey);
                
                // Check if server is healthy
                if (_serverHealth.ContainsKey(selectedServer) && !_serverHealth[selectedServer].IsHealthy)
                {
                    // Find alternative server
                    var healthyServers = _serverHealth.Where(s => s.Value.IsHealthy).Select(s => s.Key).ToList();
                    if (healthyServers.Any())
                    {
                        selectedServer = healthyServers.First();
                        _logger.LogWarning("Routed to alternative server {Server} due to health issues", selectedServer);
                    }
                }

                // Update request count
                if (_serverHealth.ContainsKey(selectedServer))
                {
                    _serverHealth[selectedServer].RequestCount++;
                }

                _logger.LogInformation("Request routed to server {Server} for key {RequestKey}", selectedServer, requestKey);
                return selectedServer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error routing request for key {RequestKey}", requestKey);
                throw;
            }
        }

        public void UpdateServerHealth(string serverName, bool isHealthy, TimeSpan responseTime)
        {
            if (_serverHealth.ContainsKey(serverName))
            {
                _serverHealth[serverName].IsHealthy = isHealthy;
                _serverHealth[serverName].LastHealthCheck = DateTime.UtcNow;
                _serverHealth[serverName].ResponseTime = responseTime;

                _logger.LogInformation("Updated health for server {Server}: Healthy={IsHealthy}, ResponseTime={ResponseTime}ms", 
                    serverName, isHealthy, responseTime.TotalMilliseconds);
            }
        }

        public void AddServer(string serverName)
        {
            _hashRing.AddServer(serverName);
            _serverHealth[serverName] = new ServerHealth
            {
                ServerName = serverName,
                IsHealthy = true,
                LastHealthCheck = DateTime.UtcNow,
                RequestCount = 0,
                ResponseTime = TimeSpan.Zero
            };

            _logger.LogInformation("Added server {Server} to load balancer", serverName);
        }

        public void RemoveServer(string serverName)
        {
            _hashRing.RemoveServer(serverName);
            _serverHealth.Remove(serverName);
            _logger.LogInformation("Removed server {Server} from load balancer", serverName);
        }

        public Dictionary<string, ServerHealth> GetServerHealthStatus()
        {
            return new Dictionary<string, ServerHealth>(_serverHealth);
        }

        public LoadBalancerStats GetStats()
        {
            return new LoadBalancerStats
            {
                TotalServers = _hashRing.GetServerCount(),
                HealthyServers = _serverHealth.Count(s => s.Value.IsHealthy),
                TotalRequests = _serverHealth.Sum(s => s.Value.RequestCount),
                ServerStats = _serverHealth.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new ServerStats
                    {
                        RequestCount = kvp.Value.RequestCount,
                        AverageResponseTime = kvp.Value.ResponseTime,
                        IsHealthy = kvp.Value.IsHealthy,
                        LastHealthCheck = kvp.Value.LastHealthCheck
                    })
            };
        }
    }

    public class ServerHealth
    {
        public string ServerName { get; set; } = string.Empty;
        public bool IsHealthy { get; set; }
        public DateTime LastHealthCheck { get; set; }
        public int RequestCount { get; set; }
        public TimeSpan ResponseTime { get; set; }
    }

    public class LoadBalancerStats
    {
        public int TotalServers { get; set; }
        public int HealthyServers { get; set; }
        public int TotalRequests { get; set; }
        public Dictionary<string, ServerStats> ServerStats { get; set; } = new();
    }

    public class ServerStats
    {
        public int RequestCount { get; set; }
        public TimeSpan AverageResponseTime { get; set; }
        public bool IsHealthy { get; set; }
        public DateTime LastHealthCheck { get; set; }
    }

