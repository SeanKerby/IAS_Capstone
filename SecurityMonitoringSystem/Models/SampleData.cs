using System;
using System.Collections.Generic;

namespace SecurityMonitoringSystem.Models
{
    public class NetworkDevice
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string Type { get; set; }
        public string Status { get; set; } // Normal, Warning, Critical
        public string UplinkSpeed { get; set; }
    }

    public class SystemLog
    {
        public string Timestamp { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
        public string Module { get; set; }
    }

    public class ThreatAlert
    {
        public string ThreatID { get; set; }
        public string Description { get; set; }
        public string RiskLevel { get; set; } // Low, Medium, High
        public string Timestamp { get; set; }
    }

    public class UserAccount
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string LastLogin { get; set; }
        public string Status { get; set; }
    }

    public static class SampleData
    {
        public static List<NetworkDevice> GetDevices()
        {
            return new List<NetworkDevice>
            {
                new NetworkDevice { ID = "ND-101", Name = "Core Router A", IPAddress = "192.168.10.1", Type = "Router", Status = "Normal", UplinkSpeed = "100 Gbps" },
                new NetworkDevice { ID = "ND-102", Name = "Sat-Link Terminal", IPAddress = "10.0.5.21", Type = "Satellite", Status = "Warning", UplinkSpeed = "2 Gbps" },
                new NetworkDevice { ID = "ND-103", Name = "Edge Node 51", IPAddress = "192.168.12.50", Type = "6G Node", Status = "Critical", UplinkSpeed = "Offline" },
                new NetworkDevice { ID = "ND-104", Name = "Database Server", IPAddress = "10.0.1.100", Type = "Server", Status = "Normal", UplinkSpeed = "10 Gbps" }
            };
        }

        public static List<SystemLog> GetLogs()
        {
            return new List<SystemLog>
            {
                new SystemLog { Timestamp = DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss"), User = "Admin", Action = "Login", Module = "Access Control" },
                new SystemLog { Timestamp = DateTime.Now.AddMinutes(-12).ToString("yyyy-MM-dd HH:mm:ss"), User = "System", Action = "Anomaly Detected: Ping Flood", Module = "Threat Detection" },
                new SystemLog { Timestamp = DateTime.Now.AddMinutes(-45).ToString("yyyy-MM-dd HH:mm:ss"), User = "User1", Action = "Data Decrypted", Module = "Encryption" }
            };
        }

        public static List<ThreatAlert> GetThreats()
        {
            return new List<ThreatAlert>
            {
                new ThreatAlert { ThreatID = "TR-9901", Description = "Unauthorized Access Attempt from IP 45.2.1.9", RiskLevel = "High", Timestamp = DateTime.Now.AddMinutes(-10).ToString("HH:mm:ss") },
                new ThreatAlert { ThreatID = "TR-9902", Description = "Unusual encrypted payload size detected", RiskLevel = "Medium", Timestamp = DateTime.Now.AddMinutes(-25).ToString("HH:mm:ss") },
                new ThreatAlert { ThreatID = "TR-9903", Description = "Multiple failed login attempts", RiskLevel = "Low", Timestamp = DateTime.Now.AddMinutes(-60).ToString("HH:mm:ss") }
            };
        }

        public static List<UserAccount> GetUsers()
        {
            return new List<UserAccount>
            {
                new UserAccount { Username = "Admin", Role = "Administrator", LastLogin = DateTime.Now.ToString("yyyy-MM-dd"), Status = "Active" },
                new UserAccount { Username = "Operator1", Role = "User", LastLogin = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), Status = "Active" },
                new UserAccount { Username = "JohnDoe", Role = "User", LastLogin = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"), Status = "Suspended" }
            };
        }
    }
}
