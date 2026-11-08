using System;

namespace TapSync.Models
{
    public class LocationData
    {
        public string AttendeeId { get; set; } = string.Empty;
        public string AttendeeName { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double Accuracy { get; set; } = 10.0; // meters

        // Intensity for heatmap (0-1 scale, higher = more concentrated)
        public double HeatIntensity { get; set; } = 0.5;

        // Zone/Area information
        public string Zone { get; set; } = "General Area";
    }

    public class LocationUpdate
    {
        public string AttendeeId { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class HeatmapPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Intensity { get; set; } // 0-1
    }

    // Represents a geographic area on the map
    public class MapZone
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }
        public double RadiusMeters { get; set; } = 50;
        public string Category { get; set; } = "General"; // VIP, Staff, General, Press
        public int PeopleDensity { get; set; } = 0;
    }
}
