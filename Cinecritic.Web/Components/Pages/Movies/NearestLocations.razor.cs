using Cinecritic.Domain.Models;
using Microsoft.JSInterop;

namespace Cinecritic.Web.Components.Pages.Movies;

public partial class NearestLocations
{
    private double lat;
    private double lon;
    private bool isSearching;
    private IEnumerable<FilmingLocation>? locations;
    
    private string FormatDistance(double meters)
    {
        if (meters < 1000)
            return $"{meters:F0} m";
        
        return $"{meters / 1000:F1} km";
    }

    private async Task GetUserLocationAsync()
    {
        try
        {
            var result = await JS.InvokeAsync<LocationResult>("getBrowserLocation");
            
            if (result != null)
            {
                lat = result.Latitude;
                lon = result.Longitude;
                
                StateHasChanged();
                
                await SearchNearestAsync();
            }
        }
        catch (JSException ex)
        {
            Console.WriteLine($"Geolocation error: {ex.Message}");
        }
    }

    private async Task SearchNearestAsync()
    {
        if (lat == 0 && lon == 0) return;

        isSearching = true;
        
        var result = await MovieService.GetNearestLocationsAsync(lat, lon, 6);
        
        if (result.IsSuccess)
        {
            locations = result.Value;
        }
        
        isSearching = false;
    }

    private class LocationResult
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}