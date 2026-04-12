using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieTypes;
using Cinecritic.Domain.Models;
using Cinecritic.Web.ViewModels.Movies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Cinecritic.Web.Components.Pages.Manager
{
    public partial class EditMovie
    {
        [Parameter] public string MovieId { get; set; } = string.Empty;
        private Guid MovieGuid => Guid.Parse(MovieId);

        private CreateMovieViewModel UpdateMovieViewModel { get; set; } = new();
        private string? _previewUrl;
        private IBrowserFile? _browserFile;
        private string? _statusMessage;
        private List<MovieType> MovieTypes { get; set; } = new();

        [Inject] private IMovieService MovieService { get; set; } = default!;
        [Inject] private IMovieTypeService MovieTypeService { get; set; } = default!;
        [Inject] private IMapper Mapper { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var typesResult = await MovieTypeService.GetMovieTypes();
            MovieTypes = typesResult.Value.ToList();

            var movieResult = await MovieService.GetMovieAsync(MovieGuid, null, 0);
            if (movieResult.IsSuccess)
            {
                UpdateMovieViewModel = Mapper.Map<CreateMovieViewModel>(movieResult.Value);
                _previewUrl = movieResult.Value.ImagePath;
            }
        }

        private async Task HandleSubmit()
        {
            var dto = Mapper.Map<CreateMovieDto>(UpdateMovieViewModel);
            dto.MovieTypeId = UpdateMovieViewModel.SelectedMovieTypeId.Value;
            dto.MovieTypeName = MovieTypes.First(mt => mt.Id == UpdateMovieViewModel.SelectedMovieTypeId).Name;
            
            Stream? stream = null;
            string? extension = null;

            if (_browserFile != null)
            {
                stream = _browserFile.OpenReadStream(2 * 1024 * 1024);
                extension = Path.GetExtension(_browserFile.Name);
            }
            
            var result = await MovieService.UpdateMovieAsync(MovieGuid, dto, stream, extension);

            if (result.IsSuccess)
                _statusMessage = "Updated successfully!";
            else
                _statusMessage = "Update failed";
        }

        private async Task HandleSelected(InputFileChangeEventArgs e)
        {
            _browserFile = e.File;
            using var ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            _previewUrl = $"data:image/jpg;base64,{Convert.ToBase64String(ms.ToArray())}";
        }
    }
}