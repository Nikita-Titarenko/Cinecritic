using System.IO;
using AutoMapper;
using Cinecritic.Application.DTOs;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Web.ViewModels;
using FluentResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Cinecritic.Web.Components.Pages.Manager
{
    public partial class CreateMovie
    {
        private const int MaxFileSize = 2 * 1024 * 1024;

        private const string AcceptedImageType = "image/jpeg";

        [SupplyParameterFromForm]
        private CreateMovieViewModel CreateMovieViewModel { get; set; } = new CreateMovieViewModel();

        private string? _previewUrl;

        private IBrowserFile? _browserFile;

        private string StatusMessage { get; set; } = string.Empty;

        [Inject]
        private IMovieService MovieService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private async Task HandleSubmit()
        {
            var dto = Mapper.Map<CreateMovieDto>(CreateMovieViewModel);
            Result<int> createMovieResult = default!;
            if (_browserFile == null)
            {
                createMovieResult = await MovieService.CreateMovieAsync(dto, null, null);
            } else
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