using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SecurityMonitoringSystem.Models;
using System;

namespace SecurityMonitoringSystem.Services
{
    public class AccessControlService
    {
        public async Task<List<UserAccount>> GetAllUsersAsync()
        {
            var users = new List<UserAccount>();
            using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT Username, Role, Status, LastLogin FROM Users", conn);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new UserAccount
                        {
                            Username = reader.GetString(0),
                            Role = reader.GetString(1),
                            Status = reader.GetString(2),
                            LastLogin = reader.IsDBNull(3) ? "Never" : reader.GetDateTime(3).ToString("yyyy-MM-dd")
                        });
                    }
                }
            }
            return users;
        }

        public async Task<string> AuthenticateAsync(string username, string rawPassword)
        {
            string hashed = DatabaseInitializer.ComputeSha256Hash(rawPassword);
            using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("SELECT Role FROM Users WHERE Username=@u AND PasswordHash=@p", conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", hashed);
                var result = await cmd.ExecuteScalarAsync();

                if (result != null)
                {
                    string role = result.ToString();
                    SessionProvider.CurrentUsername = username;
                    SessionProvider.CurrentRole = role;

                    // Update LastLogin
                    var upd = new SqlCommand("UPDATE Users SET LastLogin=@d WHERE Username=@u", conn);
                    upd.Parameters.AddWithValue("@d", DateTime.Now);
                    upd.Parameters.AddWithValue("@u", username);
                    await upd.ExecuteNonQueryAsync();

                    await ActivityLogService.LogActionAsync(username, "Access Control", "User logged in successfully");
                    return role;
                }
                
                await ActivityLogService.LogActionAsync(username, "Access Control", "Failed login attempt");
                return null;
            }
        }

        public async Task<bool> AddUserAsync(string username, string rawPassword, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(rawPassword)) return false;
            
            try
            {
                string hashed = DatabaseInitializer.ComputeSha256Hash(rawPassword);
                using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
                {
                    await conn.OpenAsync();
                    
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username=@u", conn);
                    checkCmd.Parameters.AddWithValue("@u", username);
                    int count = (int)await checkCmd.ExecuteScalarAsync();
                    if (count > 0) return false; // Already exists

                    var cmd = new SqlCommand("INSERT INTO Users (Username, PasswordHash, Role, Status) VALUES (@u, @p, @r, 'Active')", conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", hashed);
                    cmd.Parameters.AddWithValue("@r", role);
                    await cmd.ExecuteNonQueryAsync();

                    await ActivityLogService.LogActionAsync("Admin", "Access Control", $"Added new user: {username}");
                    return true;
                }
            }
            catch { return false; }
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            if (username.Equals("Admin", StringComparison.OrdinalIgnoreCase)) return false; // Protect root admin
            
            try
            {
                using (var conn = new SqlConnection(DatabaseInitializer.ConnectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("DELETE FROM Users WHERE Username=@u", conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    int rows = await cmd.ExecuteNonQueryAsync();
                    
                    if (rows > 0)
                    {
                        await ActivityLogService.LogActionAsync("Admin", "Access Control", $"Deleted user: {username}");
                        return true;
                    }
                    return false;
                }
            }
            catch { return false; }
        }
    }
}
