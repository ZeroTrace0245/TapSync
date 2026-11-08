using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TapSync.Models;

namespace TapSync.Services
{
    public class LocationSimulationService
    {
        private static LocationSimulationService? _instance;
        private CancellationTokenSource? _simulationToken;
        private readonly LocationService _locationService;
        private readonly Random _random = new();

        public static LocationSimulationService GetInstance()
        {
            return _instance ??= new LocationSimulationService();
        }

        public LocationSimulationService()
        {
            _locationService = LocationService.GetInstance();
        }

        public async Task StartSimulation()
        {
            _simulationToken = new CancellationTokenSource();
            GenerateDemoAttendees();

            // Simulate real-time movement
            await Task.Run(() => SimulateMovement(_simulationToken.Token));
        }

        public void StopSimulation()
        {
            _simulationToken?.Cancel();
        }

        private void GenerateDemoAttendees()
        {
            // Generate demo attendees distributed across zones
            var zones = _locationService.GetZones();
            var mockData = MockDataService.GetInstance();
            var attendees = mockData.GetAllAttendees();

            int index = 0;
            foreach (var attendee in attendees)
            {
                if (index < zones.Count)
                {
                    var zone = zones[index % zones.Count];
                    var lat = zone.CenterLatitude + (_random.NextDouble() - 0.5) * 0.001;
                    var lon = zone.CenterLongitude + (_random.NextDouble() - 0.5) * 0.001;

                    _locationService.UpdateLocation(
                        attendee.Id,
                        lat,
                        lon,
                        attendee.FullName,
                        attendee.Category
                    );
                }
                index++;
            }
        }

        private async Task SimulateMovement(CancellationToken cancellationToken)
        {
            var locations = _locationService.GetAllLocations();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(2000, cancellationToken); // Update every 2 seconds

                    foreach (var location in locations)
                    {
                        // Random walk simulation
                        var randomLat = location.Latitude + (_random.NextDouble() - 0.5) * 0.00005;
                        var randomLon = location.Longitude + (_random.NextDouble() - 0.5) * 0.00005;

                        _locationService.UpdateLocation(
                            location.AttendeeId,
                            randomLat,
                            randomLon,
                            location.AttendeeName,
                            location.Category
                        );
                    }

                    // Occasionally move someone to a different zone
                    if (_random.NextDouble() < 0.1)
                    {
                        var zones = _locationService.GetZones();
                        var randomZone = zones[_random.Next(zones.Count)];
                        var randomAttendee = locations[_random.Next(locations.Count)];

                        var newLat = randomZone.CenterLatitude + (_random.NextDouble() - 0.5) * 0.001;
                        var newLon = randomZone.CenterLongitude + (_random.NextDouble() - 0.5) * 0.001;

                        _locationService.UpdateLocation(
                            randomAttendee.AttendeeId,
                            newLat,
                            newLon,
                            randomAttendee.AttendeeName,
                            randomAttendee.Category
                        );
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
