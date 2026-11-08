using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TapSync.Models;

namespace TapSync.Services
{
    public class LocationService
    {
        private static LocationService? _instance;
        private readonly Dictionary<string, LocationData> _locationCache = new();
        private readonly List<HeatmapPoint> _heatmapData = new();
        private readonly List<MapZone> _zones = new();

        public event EventHandler<LocationData>? LocationUpdated;
        public event EventHandler? HeatmapUpdated;

        public static LocationService GetInstance()
        {
            return _instance ??= new LocationService();
        }

        public LocationService()
        {
            InitializeZones();
        }

        private void InitializeZones()
        {
            // Demo zones for an event venue
            _zones.Add(new MapZone
            {
                Name = "Main Hall",
                CenterLatitude = 40.7580,
                CenterLongitude = -73.9855,
                RadiusMeters = 100,
                Category = "General"
            });

            _zones.Add(new MapZone
            {
                Name = "VIP Lounge",
                CenterLatitude = 40.7590,
                CenterLongitude = -73.9865,
                RadiusMeters = 50,
                Category = "VIP"
            });

            _zones.Add(new MapZone
            {
                Name = "Registration Desk",
                CenterLatitude = 40.7570,
                CenterLongitude = -73.9845,
                RadiusMeters = 40,
                Category = "Staff"
            });

            _zones.Add(new MapZone
            {
                Name = "Press Area",
                CenterLatitude = 40.7600,
                CenterLongitude = -73.9875,
                RadiusMeters = 60,
                Category = "Press"
            });
        }

        public List<MapZone> GetZones() => new(_zones);

        public void UpdateLocation(string attendeeId, double latitude, double longitude, string attendeeName = "", string category = "General")
        {
            var location = new LocationData
            {
                AttendeeId = attendeeId,
                AttendeeName = attendeeName,
                Category = category,
                Latitude = latitude,
                Longitude = longitude,
                Timestamp = DateTime.UtcNow,
                Zone = GetZoneAtLocation(latitude, longitude)?.Name ?? "General Area"
            };

            _locationCache[attendeeId] = location;
            LocationUpdated?.Invoke(this, location);
            UpdateHeatmap();
        }

        public LocationData? GetLocation(string attendeeId)
        {
            return _locationCache.TryGetValue(attendeeId, out var location) ? location : null;
        }

        public List<LocationData> GetAllLocations()
        {
            return new(_locationCache.Values);
        }

        public List<LocationData> GetLocationsByZone(string zoneName)
        {
            return _locationCache.Values
                .Where(l => l.Zone == zoneName)
                .ToList();
        }

        public List<LocationData> GetLocationsByCategory(string category)
        {
            return _locationCache.Values
                .Where(l => l.Category == category)
                .ToList();
        }

        public MapZone? GetZoneAtLocation(double latitude, double longitude)
        {
            return _zones.FirstOrDefault(z =>
                Distance(z.CenterLatitude, z.CenterLongitude, latitude, longitude) <= z.RadiusMeters);
        }

        private void UpdateHeatmap()
        {
            _heatmapData.Clear();

            // Aggregate nearby points for heatmap visualization
            var grouped = _locationCache.Values
                .GroupBy(l => new { lat = Round(l.Latitude, 4), lon = Round(l.Longitude, 4) })
                .ToList();

            foreach (var group in grouped)
            {
                var points = group.ToList();
                var intensity = Math.Min(1.0, points.Count / 10.0); // Normalize intensity

                _heatmapData.Add(new HeatmapPoint
                {
                    Latitude = points.Average(p => p.Latitude),
                    Longitude = points.Average(p => p.Longitude),
                    Intensity = intensity
                });
            }

            // Update zone densities
            foreach (var zone in _zones)
            {
                zone.PeopleDensity = _locationCache.Values
                    .Count(l => Distance(zone.CenterLatitude, zone.CenterLongitude, l.Latitude, l.Longitude) <= zone.RadiusMeters);
            }

            HeatmapUpdated?.Invoke(this, EventArgs.Empty);
        }

        public List<HeatmapPoint> GetHeatmapData() => new(_heatmapData);

        public int GetTotalLocationsTracked() => _locationCache.Count;

        public int GetLocationsInZone(string zoneName)
        {
            var zone = _zones.FirstOrDefault(z => z.Name == zoneName);
            if (zone == null) return 0;

            return _locationCache.Values
                .Count(l => Distance(zone.CenterLatitude, zone.CenterLongitude, l.Latitude, l.Longitude) <= zone.RadiusMeters);
        }

        // Haversine formula for distance calculation (in meters)
        private static double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadiusKm = 6371;
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadiusKm * c * 1000; // Convert to meters
        }

        private static double ToRadians(double degrees) => degrees * Math.PI / 180;
        private static double Round(double value, int decimals) => Math.Round(value, decimals);

        public void ClearAllLocations()
        {
            _locationCache.Clear();
            _heatmapData.Clear();
            foreach (var zone in _zones)
                zone.PeopleDensity = 0;

            HeatmapUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
