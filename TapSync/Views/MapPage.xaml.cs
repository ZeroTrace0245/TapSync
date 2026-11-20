using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using TapSync.Models;
using TapSync.Services;
using Windows.UI;

namespace TapSync.Views
{
    public sealed partial class MapPage : Page
    {
        private readonly LocationService _locationService;
        private readonly LocationSimulationService _simulationService;
        private bool _showHeatmap = true;
        private bool _showZones = true;
        private string _currentFilter = "All";

        private readonly ObservableCollection<LocationData> _displayedLocations = new();
        private readonly ObservableCollection<MapZone> _zones = new();

        // Map parameters (NYC demo - Times Square area)
        private const double BaseLatitude = 40.7580;
        private const double BaseLongitude = -73.9855;

        public MapPage()
        {
            InitializeComponent();

            _locationService = LocationService.GetInstance();
            _simulationService = LocationSimulationService.GetInstance();

            foreach (var zone in _locationService.GetZones())
            {
                _zones.Add(zone);
            }

            ZoneStatsItemsControl.ItemsSource = _zones;
            AttendeeListItemsControl.ItemsSource = _displayedLocations;

            CategoryFilterComboBox.SelectedIndex = 0;
            CategoryFilterComboBox.SelectionChanged += CategoryFilter_Changed;

            StartSimulationButton.Click += StartSimulation_Click;
            StopSimulationButton.Click += StopSimulation_Click;
            ClearAll.Click += ClearAll_Click;

            HeatmapToggleButton.Checked += (s, e) => { _showHeatmap = true; DrawMap(); };
            HeatmapToggleButton.Unchecked += (s, e) => { _showHeatmap = false; DrawMap(); };
            ZonesToggleButton.Checked += (s, e) => { _showZones = true; DrawMap(); };
            ZonesToggleButton.Unchecked += (s, e) => { _showZones = false; DrawMap(); };

            MapCanvas.SizeChanged += MapCanvas_SizeChanged;

            Loaded += MapPage_Loaded;
            Unloaded += MapPage_Unloaded;
            ActualThemeChanged += (_, _) => DrawMap();
        }

        private void MapPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Subscribe while the page is on screen; the services are singletons and outlive the page.
            _locationService.LocationUpdated += OnLocationUpdated;
            _locationService.HeatmapUpdated += OnHeatmapUpdated;

            TotalTrackedText.Text = _locationService.GetTotalLocationsTracked().ToString();
            UpdateDisplayedLocations();
            DrawMap();
        }

        private void MapPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _locationService.LocationUpdated -= OnLocationUpdated;
            _locationService.HeatmapUpdated -= OnHeatmapUpdated;
        }

        private void OnLocationUpdated(object? sender, LocationData location)
        {
            DispatcherQueue?.TryEnqueue(() =>
            {
                TotalTrackedText.Text = _locationService.GetTotalLocationsTracked().ToString();

                UpdateDisplayedLocations();
                DrawMap();
            });
        }

        private void OnHeatmapUpdated(object? sender, EventArgs e)
        {
            DispatcherQueue?.TryEnqueue(() =>
            {
                DrawMap();
            });
        }

        private void UpdateDisplayedLocations()
        {
            var locations = _currentFilter == "All"
                ? _locationService.GetAllLocations()
                : _locationService.GetLocationsByCategory(_currentFilter);

            _displayedLocations.Clear();
            foreach (var loc in locations.OrderByDescending(l => l.Timestamp).Take(10))
            {
                _displayedLocations.Add(loc);
            }
        }

        private void DrawMap()
        {
            if (MapCanvas == null) return;

            MapCanvas.Children.Clear();

            var width = MapCanvas.ActualWidth;
            var height = MapCanvas.ActualHeight;

            if (width <= 0 || height <= 0) return;

            // Subtle grey wash that works on light, dark and picture backgrounds
            MapCanvas.Background = new SolidColorBrush(Color.FromArgb(20, 128, 128, 128));

            // Draw zones if enabled
            if (_showZones)
            {
                DrawZones(MapCanvas, width, height);
            }

            // Draw heatmap if enabled
            if (_showHeatmap)
            {
                DrawHeatmap(MapCanvas, width, height);
            }

            // Draw attendee markers
            DrawAttendeeMarkers(MapCanvas, width, height);

            // Draw legend
            DrawLegend(MapCanvas, width, height);
        }

        private void DrawZones(Canvas canvas, double width, double height)
        {
            var zones = _locationService.GetZones();

            foreach (var zone in zones)
            {
                var x = LongitudeToX(zone.CenterLongitude, width);
                var y = LatitudeToY(zone.CenterLatitude, height);
                var radius = MetersToPixels(zone.RadiusMeters, width);

                // Zone circle
                var circle = new Ellipse
                {
                    Width = radius * 2,
                    Height = radius * 2,
                    Stroke = GetZoneColor(zone.Category),
                    StrokeThickness = 2,
                    Fill = new SolidColorBrush(Color.FromArgb(30, 100, 150, 200))
                };

                Canvas.SetLeft(circle, x - radius);
                Canvas.SetTop(circle, y - radius);
                canvas.Children.Add(circle);

                // Zone label
                var label = new TextBlock
                {
                    Text = $"{zone.Name} ({zone.PeopleDensity})",
                    FontSize = 11,
                    Foreground = GetZoneColor(zone.Category)
                };

                Canvas.SetLeft(label, x - 40);
                Canvas.SetTop(label, y - 30);
                canvas.Children.Add(label);
            }
        }

        private void DrawHeatmap(Canvas canvas, double width, double height)
        {
            var heatmapData = _locationService.GetHeatmapData();

            foreach (var point in heatmapData)
            {
                var x = LongitudeToX(point.Longitude, width);
                var y = LatitudeToY(point.Latitude, height);

                // Heatmap point - larger and more intense for higher density
                var size = 10 + (point.Intensity * 40);
                var opacity = 0.3 + (point.Intensity * 0.5);

                var heatPoint = new Ellipse
                {
                    Width = size,
                    Height = size,
                    Fill = new SolidColorBrush(GetHeatColor(point.Intensity, opacity))
                };

                Canvas.SetLeft(heatPoint, x - size / 2);
                Canvas.SetTop(heatPoint, y - size / 2);
                Canvas.SetZIndex(heatPoint, -10);
                canvas.Children.Add(heatPoint);
            }
        }

        private void DrawAttendeeMarkers(Canvas canvas, double width, double height)
        {
            var locations = _currentFilter == "All"
                ? _locationService.GetAllLocations()
                : _locationService.GetLocationsByCategory(_currentFilter);

            foreach (var location in locations)
            {
                var x = LongitudeToX(location.Longitude, width);
                var y = LatitudeToY(location.Latitude, height);

                // Attendee marker
                var marker = new Ellipse
                {
                    Width = 12,
                    Height = 12,
                    Fill = GetCategoryColor(location.Category),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1
                };

                Canvas.SetLeft(marker, x - 6);
                Canvas.SetTop(marker, y - 6);
                Canvas.SetZIndex(marker, 10);
                canvas.Children.Add(marker);

                // Label
                var nameLabel = new TextBlock
                {
                    Text = location.AttendeeName,
                    FontSize = 9,
                    Foreground = TextBrush(),
                    Padding = new Thickness(2, 1, 2, 1)
                };

                Canvas.SetLeft(nameLabel, x + 8);
                Canvas.SetTop(nameLabel, y - 6);
                Canvas.SetZIndex(nameLabel, 5);
                canvas.Children.Add(nameLabel);
            }
        }

        private void DrawLegend(Canvas canvas, double width, double height)
        {
            const double legendX = 20;
            const double legendY = 20;
            const double itemHeight = 25;

            // Legend background
            bool dark = ActualTheme == ElementTheme.Dark;
            var background = new Rectangle
            {
                Width = 140,
                Height = 150,
                RadiusX = 6,
                RadiusY = 6,
                Fill = new SolidColorBrush(dark ? Color.FromArgb(200, 44, 44, 44) : Color.FromArgb(220, 255, 255, 255)),
                Stroke = new SolidColorBrush(Color.FromArgb(40, 128, 128, 128)),
                StrokeThickness = 1
            };
            Canvas.SetLeft(background, legendX);
            Canvas.SetTop(background, legendY);
            canvas.Children.Add(background);

            var legendTitle = new TextBlock
            {
                Text = "Legend",
                FontSize = 12,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                Foreground = TextBrush()
            };
            Canvas.SetLeft(legendTitle, legendX + 8);
            Canvas.SetTop(legendTitle, legendY + 8);
            canvas.Children.Add(legendTitle);

            var categories = new[] { ("General", "General"), ("VIP", "VIP"), ("Staff", "Staff"), ("Press", "Press") };
            for (int i = 0; i < categories.Length; i++)
            {
                var y = legendY + 30 + (i * itemHeight);

                var dot = new Ellipse
                {
                    Width = 12,
                    Height = 12,
                    Fill = GetCategoryColor(categories[i].Item1)
                };
                Canvas.SetLeft(dot, legendX + 8);
                Canvas.SetTop(dot, y);
                canvas.Children.Add(dot);

                var label = new TextBlock
                {
                    Text = categories[i].Item2,
                    FontSize = 11,
                    Foreground = TextBrush()
                };
                Canvas.SetLeft(label, legendX + 28);
                Canvas.SetTop(label, y);
                canvas.Children.Add(label);
            }
        }

        private SolidColorBrush TextBrush() =>
            new(ActualTheme == ElementTheme.Dark ? Colors.White : Colors.Black);

        private double LongitudeToX(double longitude, double width)
        {
            const double mapWidth = 0.02;
            return (longitude - BaseLongitude) / mapWidth * width + width / 2;
        }

        private double LatitudeToY(double latitude, double height)
        {
            const double mapHeight = 0.01;
            return (BaseLatitude - latitude) / mapHeight * height + height / 2;
        }

        private double MetersToPixels(double meters, double width)
        {
            return (meters / 1000.0) * (width / 2.0);
        }

        private SolidColorBrush GetCategoryColor(string category)
        {
            return category switch
            {
                "VIP" => new SolidColorBrush(Color.FromArgb(255, 255, 193, 7)),
                "Staff" => new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),
                "Press" => new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)),
                _ => new SolidColorBrush(Color.FromArgb(255, 33, 150, 243))
            };
        }

        private SolidColorBrush GetZoneColor(string category)
        {
            return category switch
            {
                "VIP" => new SolidColorBrush(Color.FromArgb(255, 255, 193, 7)),
                "Staff" => new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),
                "Press" => new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)),
                _ => new SolidColorBrush(Color.FromArgb(255, 33, 150, 243))
            };
        }

        private Color GetHeatColor(double intensity, double opacity)
        {
            byte alpha = (byte)(opacity * 255);

            if (intensity < 0.25)
                return Color.FromArgb(alpha, 0, 0, 255);
            else if (intensity < 0.5)
                return Color.FromArgb(alpha, 0, 255, 0);
            else if (intensity < 0.75)
                return Color.FromArgb(alpha, 255, 255, 0);
            else
                return Color.FromArgb(alpha, 255, 0, 0);
        }

        private void MapCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawMap();
        }

        private async void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            StartSimulationButton.IsEnabled = false;
            StopSimulationButton.IsEnabled = true;

            await _simulationService.StartSimulation();
        }

        private void StopSimulation_Click(object sender, RoutedEventArgs e)
        {
            _simulationService.StopSimulation();

            StartSimulationButton.IsEnabled = true;
            StopSimulationButton.IsEnabled = false;
        }

        private void CategoryFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _currentFilter = (string)(e.AddedItems[0] ?? "All");
                UpdateDisplayedLocations();
                DrawMap();
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            _locationService.ClearAllLocations();
            UpdateDisplayedLocations();

            TotalTrackedText.Text = "0";

            DrawMap();
        }
    }
}
