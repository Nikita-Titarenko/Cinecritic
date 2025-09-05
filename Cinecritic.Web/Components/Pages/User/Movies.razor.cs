using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Cinecritic.Web.Components.Pages.User
{
    public partial class Movies
    {
        private const int PageSize = 20;
        private string? statusMessage;
        private const int ColumnCount = 4;
        [Parameter]
        [SupplyParameterFromQuery(Name = "page")]
        public int CurrentPage { get; set; }
        private List<MovieListItemViewModel> _movies = [];
        [Inject]
        private IMovieService MovieService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;

        private async Task LoadMovies()
        {
            CurrentPage = CurrentPage == 0 ? 1 : CurrentPage;
            var getMoviesResult = await MovieService.GetMoviesAsync(PageSize, CurrentPage == 0 ? 1 : CurrentPage);
            if (!getMoviesResult.IsSuccess)
            {
                statusMessage = "Error when loading data";
                return;
            }
            _movies = (List<MovieListItemViewModel>)Mapper.Map<IEnumerable<MovieListItemViewModel>>(getMoviesResult.Value);
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadMovies();
        }
    }
}