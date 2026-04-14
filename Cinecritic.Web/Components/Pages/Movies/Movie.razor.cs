using System.Security.Claims;
using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Domain.Models;
using Cinecritic.Web.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MongoDB.Bson;

namespace Cinecritic.Web.Components.Pages.Movies
{
    public partial class Movie : BasePage
    {
        private const int StarSize = 40;

        private const int ReviewPageSize = 6;

        private bool _isHoover = false;

        private int? _tempRate;

        private DotNetObjectReference<Movie>? _objRef;

        private string IsWatchedClass
        {
            get
            {
                if (MovieVm.CurrentUserInteraction.IsWatched)
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
                if (MovieVm.CurrentUserInteraction is { IsLiked: true })
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
                if (MovieVm.CurrentUserInteraction.IsInWatchList)
                {
                    return "in-watch-list-color";
                }

                return string.Empty;
            }
        }

        [Parameter]
        public string MovieId { get; set; } = string.Empty;

        public ObjectId MovieObjectId
        {
            get => ObjectId.Parse(MovieId);
        }

        private ObjectId? _userId;

        [Parameter]
        public string MovieTitle { get; set; } = string.Empty;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject]
        private IMovieService MovieService { get; set; } = null!;
        [Inject]
        private IMovieUserService MovieUserService { get; set; } = null!;
        [Inject]
        private IJSInteropService JSInteropService { get; set; } = null!;
        [Inject]
        private IMapper Mapper { get; set; } = null!;

        private MovieWithReviewsDto MovieVm { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            _userId = await GetUserIdAsync();
            
            var movieDto = await MovieService.GetMovieAsync(MovieObjectId, _userId, ReviewPageSize);
            if (!movieDto.IsSuccess)
            {
                await JSInteropService.ShowAlertAsync();
                return;
            }
            MovieVm = movieDto.Value;
            if (MovieVm.CurrentUserInteraction == null)
            {
                MovieVm.CurrentUserInteraction = new MovieUser
                {
                    MovieId = MovieVm.Id
                };
            }
            await base.OnInitializedAsync();
        }
        
        private async Task ConfirmDelete()
        {
            bool confirmed = await JS.InvokeAsync<bool>("confirm", $"Are you sure you want to delete '{MovieTitle}'?");
    
            if (confirmed)
            {
                var result = await MovieService.DeleteMovieAsync(MovieObjectId);
                if (result.IsSuccess)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    await JSInteropService.ShowAlertAsync();
                }
            }
        }

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        private static string GetButtonClass(int starRate, int rate)
        {
            if ((starRate + 1) * 2 <= rate)
            {
                return "fa-star color-gold";
            } 
            if ((starRate + 1) * 2 == rate + 1)
            {
                return "fa-star-half-stroke color-gold";
            }

            return "fa-star";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _objRef = DotNetObjectReference.Create(this);
                await JSInteropService.AddScrollHandler(_objRef);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private string GetAverageRateButtonClass(int rate)
        {
            return GetButtonClass(rate, (int)Math.Round(MovieVm.AverageRating));
        }

        private string GetYourRateButtonClass(int starRate)
        {
            int currentRate = 0;
            if (_tempRate != null)
            {
                currentRate = _tempRate.Value;
            }
            else if (MovieVm.CurrentUserInteraction is { Rate: not null })
            {
                currentRate = MovieVm.CurrentUserInteraction.Rate.Value;
            }

            return GetButtonClass(starRate, currentRate);
        }

        private string GetUserRateButtonClass(int starRate, int userRate)
        {
            return GetButtonClass(starRate, userRate);
        }

        private void EnterOnStars(int rate, double offsetX)
        {
            _isHoover = true;
            rate = rate * 2;
            if (offsetX < StarSize / 2)
            {
                rate--;
            }
            _tempRate = rate;
        }

        private void LeaveFromStars()
        {
            _isHoover = false;
            _tempRate = null;
        }

        private void MoveInStars(int rate, double offsetX)
        {
            if (!_isHoover)
            {
                return;
            }

            EnterOnStars(rate, offsetX);
        }

        private async Task ClickOnStarAsync(int rate, double offsetX)
        {
            if (_userId == null)
            {
                return;
            }
            rate = rate * 2;
            if (offsetX < StarSize / 2)
            {
                rate--;
            }
            
            var result = await MovieUserService.RateMovieAsync(MovieVm.Id, _userId.Value, rate);
            if (!result.IsSuccess)
            {
                await JSInteropService.ShowAlertAsync();
                return;
            }

            MovieVm.CurrentUserInteraction = result.Value;
        }

        private async Task ToggleWatchAsync()
        {
            if (_userId == null)
            {
                return;
            }
            var result = await MovieUserService.ToggleWatchMovieAsync(MovieVm.Id, _userId.Value);
            if (!result.IsSuccess)
            {
                await JSInteropService.ShowAlertAsync();
                return;
            }

            MovieVm.CurrentUserInteraction = result.Value;
        }

        private async Task ToggleLikeAsync()
        {
            if (_userId == null)
            {
                return;
            }
            var result = await MovieUserService.ToggleLikeMovieAsync(MovieVm.Id, _userId.Value);
            if (!result.IsSuccess)
            {
                await JSInteropService.ShowAlertAsync();
                return;
            }

            MovieVm.CurrentUserInteraction = result.Value;
        }

        private async Task ToggleInWatchListAsync()
        {
            if (_userId == null)
            {
                return;
            }
            var result = await MovieUserService.ToggleIsInWatchListAsync(MovieVm.Id, _userId.Value);
            if (!result.IsSuccess)
            {
                await JSInteropService.ShowAlertAsync();
                return;
            }

            MovieVm.CurrentUserInteraction = result.Value;
        }

        private async Task HandleValidSubmit()
        {
            if (_userId == null)
            {
                return;
            }
            var result = await MovieUserService.CreateOrUpdateReviewAsync(MovieVm.Id, _userId.Value, MovieVm.CurrentUserInteraction.ReviewText!);
            if (!result.IsSuccess)
            {
                await JSInteropService.ShowAlertAsync();
                return;
            }
        
            MovieVm.CurrentUserInteraction = result.Value;
        }

        private string ReviewText
        {
            get => MovieVm.CurrentUserInteraction.ReviewText ?? string.Empty;
            set => MovieVm.CurrentUserInteraction.ReviewText = value;
        }

        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}