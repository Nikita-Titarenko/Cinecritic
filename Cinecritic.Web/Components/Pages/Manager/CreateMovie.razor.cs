using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieTypes;
using Cinecritic.Web.ViewModels.Movies;
using Cinecritic.Web.ViewModels.MovieTypes;
using FluentResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Cinecritic.Web.Components.Pages.Manager
{
    public partial class CreateMovie
    {
        private const int MaxFileSize = 2 * 1024 * 1024;

        private const string AcceptedImageType = "image/jpg";

        [SupplyParameterFromForm]
        private CreateMovieViewModel CreateMovieViewModel { get; set; } = new CreateMovieViewModel();

        private List<MovieTypeViewModel> MovieTypes { get; set; } = new();

        private string? _previewUrl;

        private IBrowserFile? _browserFile;

        private string StatusMessage { get; set; } = string.Empty;

        [Inject]
        private IMovieService MovieService { get; set; } = default!;
        [Inject]
        private IMovieTypeService MovieTypeService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            var getMovieTypesResult = await MovieTypeService.GetMovieTypes();
            if (!getMovieTypesResult.IsSuccess)
            {
                StatusMessage = "Error when loading data";
                return;
            }
            MovieTypes = Mapper.Map<List<MovieTypeViewModel>>(getMovieTypesResult.Value);
        }

        private async Task HandleSubmit()
        {
            var dto = Mapper.Map<CreateMovieDto>(CreateMovieViewModel);
            Result<int> createMovieResult = default!;
            if (_browserFile == null)
            {
                createMovieResult = await MovieService.CreateMovieAsync(dto, null, null);
            }
            else
            {
                using var stream = _browserFile.OpenReadStream(MaxFileSize);
                createMovieResult = await MovieService.CreateMovieAsync(dto, stream, Path.GetExtension(_browserFile.Name));
            }

            if (!createMovieResult.IsSuccess)
            {
                StatusMessage = "Error while sending";
                return;
            }

            StatusMessage = "The movie was successfully created!";
        }

        private async Task HandleSelected(InputFileChangeEventArgs e)
        {
            if (e.File.Size > MaxFileSize)
            {
                StatusMessage = "Max size image - 2MB";
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