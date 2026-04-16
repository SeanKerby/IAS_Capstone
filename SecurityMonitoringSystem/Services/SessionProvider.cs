using System;

namespace SecurityMonitoringSystem.Services
{
    public static class SessionProvider
    {
        public static string CurrentUsername { get; set; } = string.Empty;
        public static string CurrentRole { get; set; } = string.Empty;
        
        public static bool IsAdmin => CurrentRole.Equals("Administrator", StringComparison.OrdinalIgnoreCase);
    }
}
