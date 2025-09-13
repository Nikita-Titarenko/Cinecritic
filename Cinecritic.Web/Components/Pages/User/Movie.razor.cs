using System.Security.Claims;
using AutoMapper;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Application.Services.WatchLists;
using Cinecritic.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web.Components.Pages.User
{
    public partial class Movie
    {
        private const int starSize = 40;

        private bool isHoover = false;

        private int? tempRate;

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

        private string GetStarButtonClass(int rate)
        {
            int currentRate = 0;
            if (tempRate != null)
            {
                currentRate = tempRate.Value;
            } else if (MovieUserStatusViewModel.Rate != null)
            {
                currentRate = MovieUserStatusViewModel.Rate.Value;
            }

            if ((rate + 1) * 2 <= currentRate)
            {
                return "fa-star color-gold";
            }
            else if ((rate + 1) * 2 == currentRate + 1)
            {
                return "fa-star-half-stroke color-gold";
            }

            return "fa-star";
        }

        [Parameter]
        public string MovieId { get; set; } = string.Empty;
        [Parameter]
        public string MovieTitle { get; set; } = string.Empty;

        private List<string> StarButtons { get; set; } = new List<string>(new string[10]);

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject]
        private IMovieService MovieService { get; set; } = default!;
        [Inject]
        private IMovieUserService MovieUserService { get; set; } = default!;
        [Inject]
        private IWatchListService WatchListService { get; set; } = default!;
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

        private CreateMovieReviewViewModel CreateMovieReview { get; set; } = new CreateMovieReviewViewModel();

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var movieDto = await MovieService.GetMovieAsync(int.Parse(MovieId), auth.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (!movieDto.IsSuccess)
            {
                return;
            }
            MovieViewModel = Mapper.Map<MovieViewModel>(movieDto.Value);

            await base.OnInitializedAsync();
        }

        //private void ChangeStarsColor(int rate)
        //{
        //    for (int i = 0; i < rate; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            StarButtons[i / 2] = "fa-star-half-stroke color-gold";
        //        }
        //        else
        //        {
        //            StarButtons[i / 2] = "fa-star color-gold";
        //        }
        //    }
        //    for (int i = rate + 1; i < StarButtons.Count; i++)
        //    {
        //        StarButtons[i / 2] = "fa-star";
        //    }
        //}

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
    }
}