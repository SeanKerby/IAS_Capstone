using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Services
{
    public class ThreatDetectionService
    {
        public static event Action<string, string> OnThreatDetected;

        public static async Task AnalyzeNetworkAnomalyAsync()
        {
            try
            {
                using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("SELECT ID, Name, Status FROM NetworkDevices", conn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string status = reader.GetString(2);
                            if (status == "Critical")
                            {
                                string deviceId = reader.GetString(0);
                                string deviceName = reader.GetString(1);
                                await LogThreatAsync($"Critical anomaly detected on {deviceName} ({deviceId})", "High");
                            }
                            else if (status == "Warning")
                            {
                                string deviceId = reader.GetString(0);
                                string deviceName = reader.GetString(1);
                                await LogThreatAsync($"Warning flag raised on {deviceName} ({deviceId})", "Medium");
                            }
                        }
                    }
                }
            }
            catch { /* Silently fail for background task */ }
        }

        private static async Task LogThreatAsync(string description, string riskLevel)
        {
            try
            {
                using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
                {
                    await conn.OpenAsync();
                    
                    // Check if a similar threat was logged in the last minute to prevent spam
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM ThreatAlerts WHERE Description = @desc AND Timestamp > @min", conn);
                    checkCmd.Parameters.AddWithValue("@desc", description);
                    checkCmd.Parameters.AddWithValue("@min", DateTime.Now.AddMinutes(-1));
                    int count = (int)await checkCmd.ExecuteScalarAsync();

                    if (count == 0)
                    {
                        var threatId = "TR-" + new Random().Next(1000, 9999).ToString();
                        var cmd = new SqlCommand("INSERT INTO ThreatAlerts (ThreatID, Description, RiskLevel, Timestamp) VALUES (@id, @desc, @lvl, @time)", conn);
                        cmd.Parameters.AddWithValue("@id", threatId);
                        cmd.Parameters.AddWithValue("@desc", description);
                        cmd.Parameters.AddWithValue("@lvl", riskLevel);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        await cmd.ExecuteNonQueryAsync();

                        await ActivityLogService.LogActionAsync("System", "Threat Detection", description);
                        OnThreatDetected?.Invoke(description, riskLevel);
                    }
                }
            }
            catch { }
        }

        public async Task<List<ThreatAlert>> GetAlertsAsync()
        {
            var alerts = new List<ThreatAlert>();
            using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT ThreatID, Description, RiskLevel, Timestamp FROM ThreatAlerts ORDER BY Timestamp DESC", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        alerts.Add(new ThreatAlert
                        {
                            ThreatID = reader.GetString(0),
                            Description = reader.GetString(1),
                            RiskLevel = reader.GetString(2),
                            Timestamp = reader.GetDateTime(3).ToString("yyyy-MM-dd HH:mm:ss")
                        });
                    }
                }
            }
            return alerts;
        }
    }
}
