using System.Security.Claims;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieTypes;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web.Components.Pages
{
    public partial class Home
    {
        [Inject]
        protected IMovieService MovieService { get; set; } = null!;

        [Inject]
        protected IMovieTypeService MovieTypeService { get; set; } = null!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        protected IEnumerable<TopMovieQueryResult>? movies;
        protected IEnumerable<MovieTypeDto>? movieTypes;
        protected string? currentUserId;

        protected int SelectedMovieTypeId { get; set; }
        protected decimal SelectedMinRating { get; set; } = 5;

        public const int PageSize = 12;
        public const int PagePaginatorCount = 3;
        public const int PaginatorAdditionalCount = 4;

        public int TotalPageNumber { get; set; } = 1;
        public int CurrentPage { get; set; } = 1;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                currentUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var typeResult = await MovieTypeService.GetMovieTypes();
            if (typeResult.IsSuccess)
            {
                movieTypes = typeResult.Value;
                var firstType = movieTypes.FirstOrDefault();
                if (firstType != null)
                {
                    SelectedMovieTypeId = firstType.Id;
                }
            }

            await LoadMoviesAsync();
        }

        protected async Task LoadMoviesAsync()
        {
            movies = null;

            if (SelectedMovieTypeId <= 0 && movieTypes?.Any() == true)
            {
                SelectedMovieTypeId = movieTypes.First().Id;
            }

            movies = await MovieService.GetTopMoviesByTypeAsync(
                SelectedMovieTypeId,
                SelectedMinRating,
                CurrentPage,
                PageSize,
                currentUserId
            );

            TotalPageNumber = 5;
        }

        protected async Task OnMovieTypeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var typeId))
            {
                SelectedMovieTypeId = typeId;
                CurrentPage = 1;
                await LoadMoviesAsync();
            }
        }

        protected async Task OnMinRatingChanged(ChangeEventArgs e)
        {
            if (decimal.TryParse(e.Value?.ToString(), out var rating))
            {
                SelectedMinRating = rating;
                CurrentPage = 1;
                await LoadMoviesAsync();
            }
        }

        protected async Task ChangePage(int targetPage)
        {
            if (CurrentPage == targetPage) return;
            CurrentPage = targetPage;
            await LoadMoviesAsync();
        }
    }
}