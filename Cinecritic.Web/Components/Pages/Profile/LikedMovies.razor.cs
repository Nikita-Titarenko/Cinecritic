using AutoMapper;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Web.Components.Pages.Shared;
using Cinecritic.Web.JSInterop;
using Cinecritic.Web.ViewModels.Movies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web.Components.Pages.Profile;

public partial class LikedMovies : BasePage
{
    private string? statusMessage;

    [Parameter]
    [SupplyParameterFromQuery(Name = "page")]
    public int CurrentPage { get; set; }
    private MovieListViewModel _movies = new();
    [Inject]
    private IMovieUserService MovieUserService { get; set; } = default!;
    [Inject]
    private IMapper Mapper { get; set; } = default!;
    [Inject]
    private IJSInteropService JSInteropService { get; set; } = default!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    private async Task LoadMovies()
    {
        CurrentPage = CurrentPage == 0 ? 1 : CurrentPage;
        var userId = await GetUserIdAsync();
        var getMoviesResult = await MovieUserService.GetLikedMoviesAsync(
            userId!.Value,
            Paginator.PageSize,
            CurrentPage);
        if (!getMoviesResult.IsSuccess)
        {
            statusMessage = "Error when loading data";
            return;
        }
        _movies = Mapper.Map<MovieListViewModel>(getMoviesResult.Value);
        _movies.TotalPageNumber = (int)Math.Ceiling((double)getMoviesResult.Value.TotalMovieNumber / Paginator.PageSize);
    }

    public async Task OnClick(int page)
    {
        await JSInteropService.BlurActiveElement();
        NavigationManager.NavigateTo($"/profile/likedmovies?page={page}");
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadMovies();
    }
}