using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieTypes;
using Cinecritic.Web.ViewModels.Movies;
using Cinecritic.Web.ViewModels.MovieTypes;
using FluentResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web.Components.Pages.Manager
{
    public partial class EditMovie
    {
        private const int MaxFileSize = 2 * 1024 * 1024;
        private const string AcceptedImageType = "image/jpg";

        [Parameter]
        public int MovieId { get; set; }

        [SupplyParameterFromForm]
        private CreateMovieViewModel CreateMovieViewModel { get; set; } = new CreateMovieViewModel();

        private List<MovieTypeViewModel> MovieTypes { get; set; } = new();
        private string? _previewUrl;
        private IBrowserFile? _browserFile;
        private string? statusMessage;

        [Inject] private IMovieService MovieService { get; set; } = default!;
        [Inject] private IMovieTypeService MovieTypeService { get; set; } = default!;
        [Inject] private IMapper Mapper { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var getMovieTypesResult = await MovieTypeService.GetMovieTypes();
            if (!getMovieTypesResult.IsSuccess)
            {
                statusMessage = "Error when loading data";
                return;
            }
            MovieTypes = Mapper.Map<List<MovieTypeViewModel>>(getMovieTypesResult.Value);

            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var stringUserId = auth.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = stringUserId != null ? int.Parse(stringUserId) : 0;

            var movieResult = await MovieService.GetMovieAsync(MovieId, userId, 0);
            if (!movieResult.IsSuccess)
            {
                statusMessage = "Error when loading movie details";
                return;
            }

            CreateMovieViewModel = new CreateMovieViewModel
            {
                Title = movieResult.Value.Title,
                Description = movieResult.Value.Description,
                ReleaseDate = movieResult.Value.ReleaseDate,
                SelectedMovieTypeId = movieResult.Value.MovieType.Id
            };
            _previewUrl = movieResult.Value.ImagePath;
        }

        private async Task HandleSubmit()
        {
            var dto = Mapper.Map<CreateMovieDto>(CreateMovieViewModel);
            Result<int> updateMovieResult;

            if (_browserFile == null)
            {
                updateMovieResult = await MovieService.UpdateMovieAsync(MovieId, dto, null, null);
            }
            else
            {
                using var stream = _browserFile.OpenReadStream(MaxFileSize);
                updateMovieResult = await MovieService.UpdateMovieAsync(MovieId, dto, stream, Path.GetExtension(_browserFile.Name));
            }

            if (!updateMovieResult.IsSuccess)
            {
                statusMessage = "Error while sending updates";
                return;
            }

            statusMessage = "The movie was successfully updated!";
        }

        private async Task HandleSelected(InputFileChangeEventArgs e)
        {
            if (e.File.Size > MaxFileSize)
            {
                statusMessage = "Max size image - 2MB";
                return;
            }
            _browserFile = e.File;

            using var stream = e.File.OpenReadStream(MaxFileSize);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            _previewUrl = $"data:{AcceptedImageType};base64,{Convert.ToBase64String(ms.ToArray())}";
        }
    }
}