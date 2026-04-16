using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace SecurityMonitoringSystem.Services
{
    public static class DatabaseInitializer
    {
        public static readonly string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SecurityMonitoringDB;Integrated Security=True;";

        public static void Initialize()
        {
            CreateDatabaseIfNotExists();
            CreateTables();
            SeedData();
        }

        private static void CreateDatabaseIfNotExists()
        {
            string masterConnection = @"Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;";
            using (var conn = new SqlConnection(masterConnection))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'SecurityMonitoringDB')
                    BEGIN
                        CREATE DATABASE [SecurityMonitoringDB];
                    END", conn);
                cmd.ExecuteNonQuery();
            }
        }

        private static void CreateTables()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
                    CREATE TABLE Users (
                        Username NVARCHAR(50) PRIMARY KEY,
                        PasswordHash NVARCHAR(256),
                        Role NVARCHAR(20),
                        LastLogin DATETIME,
                        Status NVARCHAR(20)
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='NetworkDevices' and xtype='U')
                    CREATE TABLE NetworkDevices (
                        ID NVARCHAR(50) PRIMARY KEY,
                        Name NVARCHAR(100),
                        IPAddress NVARCHAR(50),
                        Type NVARCHAR(30),
                        Status NVARCHAR(30),
                        UplinkSpeed NVARCHAR(30)
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SystemLogs' and xtype='U')
                    CREATE TABLE SystemLogs (
                        LogID INT IDENTITY(1,1) PRIMARY KEY,
                        Timestamp DATETIME,
                        Username NVARCHAR(50),
                        Action NVARCHAR(255),
                        Module NVARCHAR(50)
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ThreatAlerts' and xtype='U')
                    CREATE TABLE ThreatAlerts (
                        ThreatID NVARCHAR(50) PRIMARY KEY,
                        Description NVARCHAR(255),
                        RiskLevel NVARCHAR(20),
                        Timestamp DATETIME
                    );
                ", conn);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedData()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                
                // Seed Admin User
                var c1 = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = 'Admin'", conn);
                int count = (int)c1.ExecuteScalar();
                if (count == 0)
                {
                    string hash = ComputeSha256Hash("admin123");
                    var insertAdmin = new SqlCommand("INSERT INTO Users (Username, PasswordHash, Role, Status) VALUES ('Admin', @hash, 'Administrator', 'Active')", conn);
                    insertAdmin.Parameters.AddWithValue("@hash", hash);
                    insertAdmin.ExecuteNonQuery();
                }

                // Seed operator user
                c1 = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = 'Operator1'", conn);
                if ((int)c1.ExecuteScalar() == 0)
                {
                    string hash = ComputeSha256Hash("operator123");
                    var insertOp = new SqlCommand("INSERT INTO Users (Username, PasswordHash, Role, Status) VALUES ('Operator1', @hash, 'User', 'Active')", conn);
                    insertOp.Parameters.AddWithValue("@hash", hash);
                    insertOp.ExecuteNonQuery();
                }

                // Initial network devices
                var c2 = new SqlCommand("SELECT COUNT(*) FROM NetworkDevices", conn);
                if ((int)c2.ExecuteScalar() == 0)
                {
                    var cmd = new SqlCommand(@"
                        INSERT INTO NetworkDevices (ID, Name, IPAddress, Type, Status, UplinkSpeed) VALUES ('ND-101', 'Core Router A', '192.168.10.1', 'Router', 'Normal', '100 Gbps');
                        INSERT INTO NetworkDevices (ID, Name, IPAddress, Type, Status, UplinkSpeed) VALUES ('ND-102', 'Sat-Link Terminal', '10.0.5.21', 'Satellite', 'Warning', '2 Gbps');
                        INSERT INTO NetworkDevices (ID, Name, IPAddress, Type, Status, UplinkSpeed) VALUES ('ND-103', 'Edge Node 51', '192.168.12.50', '6G Node', 'Critical', 'Offline');
                        INSERT INTO NetworkDevices (ID, Name, IPAddress, Type, Status, UplinkSpeed) VALUES ('ND-104', 'Database Server', '10.0.1.100', 'Server', 'Normal', '10 Gbps');
                    ", conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string ComputeSha256Hash(string rawData)
        {
            if (string.IsNullOrEmpty(rawData)) return string.Empty;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
