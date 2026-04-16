using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SecurityMonitoringSystem.Models;
using System.Threading.Tasks;

namespace SecurityMonitoringSystem.Services
{
    public class ActivityLogService
    {
        public static async Task LogActionAsync(string username, string module, string action)
        {
            using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("INSERT INTO SystemLogs (Timestamp, Username, Action, Module) VALUES (@time, @user, @act, @mod)", conn);
                cmd.Parameters.AddWithValue("@time", DateTime.Now);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@act", action);
                cmd.Parameters.AddWithValue("@mod", module);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<SystemLog>> GetLogsAsync(string searchFilter = "", string moduleFilter = "All Modules")
        {
            var logs = new List<SystemLog>();
            using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT Timestamp, Username, Action, Module FROM SystemLogs WHERE 1=1 ";
                
                if (moduleFilter != "All Modules")
                {
                    query += " AND Module = @mod ";
                }
                if (!string.IsNullOrWhiteSpace(searchFilter))
                {
                    query += " AND (Action LIKE @search OR Username LIKE @search) ";
                }
                
                query += " ORDER BY Timestamp DESC";

                var cmd = new SqlCommand(query, conn);
                
                if (moduleFilter != "All Modules")
                    cmd.Parameters.AddWithValue("@mod", moduleFilter);

                if (!string.IsNullOrWhiteSpace(searchFilter))
                    cmd.Parameters.AddWithValue("@search", "%" + searchFilter + "%");

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var timestamp = reader.GetDateTime(0);
                        logs.Add(new SystemLog
                        {
                            Timestamp = timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                            User = reader.GetString(1),
                            Action = reader.GetString(2),
                            Module = reader.GetString(3)
                        });
                    }
                }
            }
            return logs;
        }
    }
}
