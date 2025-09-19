using AutoMapper;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Web.ViewModels.Movies;
using Microsoft.AspNetCore.Components;

namespace Cinecritic.Web.Components.Pages.User
{
    public partial class Movies
    {
        private const int PageSize = 12;
        private const int PagePaginatorCount = 3;
        private const int PaginatorAdditionalCount = 4;
        private string? statusMessage;
        private const int ColumnCount = 4;
        [Parameter]
        [SupplyParameterFromQuery(Name = "page")]
        public int CurrentPage { get; set; }
        private MovieListViewModel _movies = new MovieListViewModel();
        [Inject]
        private IMovieService MovieService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;

        private async Task LoadMovies()
        {
            CurrentPage = CurrentPage == 0 ? 1 : CurrentPage;
            var getMoviesResult = await MovieService.GetMoviesAsync(PageSize, CurrentPage);
            if (!getMoviesResult.IsSuccess)
            {
                statusMessage = "Error when loading data";
                return;
            }
            _movies = Mapper.Map<MovieListViewModel>(getMoviesResult.Value);
            _movies.TotalPageNumber = (int)Math.Ceiling((double)getMoviesResult.Value.TotalMovieNumber / PageSize);
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadMovies();
        }
    }
}