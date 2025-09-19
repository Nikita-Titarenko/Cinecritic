using System.Security.Claims;
using AutoMapper;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.DTOs.Reviews;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Application.Services.Reviews;
using Cinecritic.Application.Services.WatchLists;
using Cinecritic.Web.ViewModels;
using Cinecritic.Web.ViewModels.Movies;
using Cinecritic.Web.ViewModels.MovieUsers;
using Cinecritic.Web.ViewModels.Reviews;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Cinecritic.Web.Components.Pages.User
{
    public partial class Movie
    {
        private const int starSize = 40;

        private const int reviewPageSize = 6;

        private const string AddScrollHandlerFunctionName = "addScrollHandler";

        private bool isHoover = false;

        private int? tempRate;

        private bool isReviewLoading = false;

        private bool allReviewLoaded = false;

        private int reviewPageCount = 1;

        private string displayName = string.Empty;

        private DotNetObjectReference<Movie>? objRef;

        private string IsWatchedClass
        {
            get
            {
                if (MovieUserStatusViewModel.IsWatched)
                {
                    return "watched-color";
                }

                return string.Empty;
            }
        }

        private string IsLikedClass
        {
            get
            {
                if (MovieUserStatusViewModel.IsLiked)
                {
                    return "liked-color";
                }

                return string.Empty;
            }
        }

        private string IsInWatchedListClass
        {
            get
            {
                if (MovieUserStatusViewModel.IsInWatchList)
                {
                    return "in-watch-list-color";
                }

                return string.Empty;
            }
        }

        [Parameter]
        public string MovieId { get; set; } = string.Empty;
        [Parameter]
        public string MovieTitle { get; set; } = string.Empty;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject]
        private IMovieService MovieService { get; set; } = default!;
        [Inject]
        private IMovieUserService MovieUserService { get; set; } = default!;
        [Inject]
        private IWatchListService WatchListService { get; set; } = default!;
        [Inject]
        private IReviewService ReviewService { get; set; } = default!;
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;

        private MovieViewModel MovieViewModel { get; set; } = new MovieViewModel();

        private MovieUserStatusViewModel MovieUserStatusViewModel
        {
            get
            {
                return MovieViewModel.MovieUserStatus;
            }
            set
            {
                MovieViewModel.MovieUserStatus = value;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var movieDto = await MovieService.GetMovieAsync(int.Parse(MovieId), auth.User.FindFirstValue(ClaimTypes.NameIdentifier)!, reviewPageSize);
            if (!movieDto.IsSuccess)
            {
                return;
            }
            MovieViewModel = Mapper.Map<MovieViewModel>(movieDto.Value);
            displayName = auth.User.FindFirstValue("DisplayName")!;

            if (AllReviewLoaded(MovieViewModel.Reviews))
            {
                allReviewLoaded = true;
            }
            await base.OnInitializedAsync();
        }

        private static string GetButtonClass(int starRate, int rate)
        {
            if ((starRate + 1) * 2 <= rate)
            {
                return "fa-star color-gold";
            }
            else if ((starRate + 1) * 2 == rate + 1)
            {
                return "fa-star-half-stroke color-gold";
            }

            return "fa-star";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                objRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync(AddScrollHandlerFunctionName, objRef);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private string GetAverageRateButtonClass(int rate)
        {
            return GetButtonClass(rate, (int)Math.Round(MovieViewModel.Rate));
        }

        private string GetYourRateButtonClass(int starRate)
        {
            int currentRate = 0;
            if (tempRate != null)
            {
                currentRate = tempRate.Value;
            }
            else if (MovieUserStatusViewModel.Rate != null)
            {
                currentRate = MovieUserStatusViewModel.Rate.Value;
            }

            return GetButtonClass(starRate, currentRate);
        }

        private string GetUserRateButtonClass(int starRate, int userRate)
        {
            return GetButtonClass(starRate, userRate);
        }

        [JSInvokable]
        public async Task OnScrollAsync(ScrollInfo scrollInfo)
        {
            if (isReviewLoading || allReviewLoaded)
            {
                return;
            }
            isReviewLoading = true;
            if (scrollInfo.ScrollTop + scrollInfo.ClientHeight < scrollInfo.ScrollHeight * 0.9)
            {
                isReviewLoading = false;
                return;
            }

            await LoadReviewsAsync();
            isReviewLoading = false;
        }

        private void EnterOnStars(int rate, double offsetX)
        {
            isHoover = true;
            rate = rate * 2;
            if (offsetX < starSize / 2)
            {
                rate--;
            }
            tempRate = rate;
        }

        private void LeaveFromStars()
        {
            isHoover = false;
            tempRate = null;
        }

        private void MoveInStars(int rate, double offsetX)
        {
            if (!isHoover)
            {
                return;
            }

            EnterOnStars(rate, offsetX);
        }

        private async Task ClickOnStarAsync(int rate, double offsetX)
        {
            rate = rate * 2;
            if (offsetX < starSize / 2)
            {
                rate--;
            }
            var dto = Mapper.Map<RateMovieDto>(MovieViewModel);
            dto.Rate = rate;
            var result = await MovieUserService.RateMovieAsync(dto);
            if (!result.IsSuccess)
            {
                return;
            }

            MovieUserStatusViewModel = Mapper.Map<MovieUserStatusViewModel>(result.Value);
        }

        private async Task ToggleWatchAsync()
        {
            var result = await MovieUserService.ToggleWatchMovieAsync(MovieViewModel.Id, MovieUserStatusViewModel.UserId);
            if (!result.IsSuccess)
            {
                return;
            }

            MovieUserStatusViewModel = Mapper.Map<MovieUserStatusViewModel>(result.Value);
        }

        private async Task ToggleLikeAsync()
        {
            var result = await MovieUserService.ToggleLikeMovieAsync(MovieViewModel.Id, MovieUserStatusViewModel.UserId);
            if (!result.IsSuccess)
            {
                return;
            }

            MovieUserStatusViewModel = Mapper.Map<MovieUserStatusViewModel>(result.Value);
        }

        private async Task ToggleInWatchListAsync()
        {
            var result = await WatchListService.ToggleWatchListMovieAsync(MovieViewModel.Id, MovieUserStatusViewModel.UserId);
            if (!result.IsSuccess)
            {
                return;
            }

            MovieUserStatusViewModel = Mapper.Map<MovieUserStatusViewModel>(result.Value);
        }

        private async Task CreateMovieReviewAsync()
        {
            var result = await ReviewService.CreateMovieReviewAsync(Mapper.Map<UpsertMovieReviewDto>(MovieViewModel));
            if (!result.IsSuccess)
            {
                return;
            }

            MovieUserStatusViewModel = Mapper.Map<MovieUserStatusViewModel>(result.Value);
        }

        private async Task UpdateMovieReviewAsync()
        {
            var result = await ReviewService.UpdateMovieReviewAsync(Mapper.Map<UpsertMovieReviewDto>(MovieViewModel));
            if (!result.IsSuccess)
            {
                return;
            }

            MovieUserStatusViewModel = Mapper.Map<MovieUserStatusViewModel>(result.Value);
        }

        private async Task LoadReviewsAsync()
        {
            reviewPageCount++;
            var getMovieReviewsResult = await ReviewService.GetMovieReviews(MovieViewModel.Id, reviewPageCount, reviewPageSize);
            if (!getMovieReviewsResult.IsSuccess)
            {
                return;
            }
            List<MovieReviewViewModel> reviews = Mapper.Map<List<MovieReviewViewModel>>(getMovieReviewsResult.Value);
            if (AllReviewLoaded(reviews))
            {
                allReviewLoaded = true;
            }
            MovieViewModel.Reviews.AddRange(reviews);
            StateHasChanged();
        }

        public void Dispose()
        {
            objRef?.Dispose();
        }

        private static bool AllReviewLoaded(List<MovieReviewViewModel> reviews)
        {
            return reviews.Count < reviewPageSize;
        }
    }
}