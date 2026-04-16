using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using SecurityMonitoringSystem.Models;

namespace SecurityMonitoringSystem.Services
{
    public class NetworkSimulationService
    {
        private static bool _isRunning = false;

        public static void StartSimulation()
        {
            if (_isRunning) return;
            _isRunning = true;
            
            Task.Run(async () =>
            {
                Random rand = new Random();
                while (_isRunning)
                {
                    try
                    {
                        await Task.Delay(5000); // Simulate every 5 seconds
                        await SimulateTrafficSpikeAsync(rand);
                        await ThreatDetectionService.AnalyzeNetworkAnomalyAsync();
                    }
                    catch { }
                }
            });
        }

        private static async Task SimulateTrafficSpikeAsync(Random rand)
        {
            using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
            {
                await conn.OpenAsync();
                
                // Fetch random device
                var devices = new List<string>();
                var cmd = new SqlCommand("SELECT ID FROM NetworkDevices", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        devices.Add(reader.GetString(0));
                    }
                }

                if (devices.Count > 0)
                {
                    string targetId = devices[rand.Next(devices.Count)];
                    
                    string newStatus = "Normal";
                    int roll = rand.Next(100);
                    if (roll > 80 && roll <= 95) newStatus = "Warning";
                    else if (roll > 95) newStatus = "Critical";

                    string newSpeed = $"{rand.Next(1, 200)} Gbps";
                    if (newStatus == "Critical" && rand.Next(100) > 50)
                        newSpeed = "Offline";

                    var updateCmd = new SqlCommand("UPDATE NetworkDevices SET Status = @stat, UplinkSpeed = @speed WHERE ID = @id", conn);
                    updateCmd.Parameters.AddWithValue("@stat", newStatus);
                    updateCmd.Parameters.AddWithValue("@speed", newSpeed);
                    updateCmd.Parameters.AddWithValue("@id", targetId);
                    await updateCmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<NetworkDevice>> GetAllDevicesAsync()
        {
            var results = new List<NetworkDevice>();
            using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT ID, Name, IPAddress, Type, Status, UplinkSpeed FROM NetworkDevices", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new NetworkDevice
                        {
                            ID = reader.GetString(0),
                            Name = reader.GetString(1),
                            IPAddress = reader.GetString(2),
                            Type = reader.GetString(3),
                            Status = reader.GetString(4),
                            UplinkSpeed = reader.GetString(5)
                        });
                    }
                }
            }
            return results;
        }
    }
}
