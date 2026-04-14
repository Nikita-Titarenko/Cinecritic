using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieTypes;
using Cinecritic.Domain.Models;
using Cinecritic.Web.ViewModels.Movies;
using FluentResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson;

namespace Cinecritic.Web.Components.Pages.Manager;

public partial class CreateMovie
{
    private const int MaxFileSize = 2 * 1024 * 1024;

    private const string AcceptedImageType = "image/jpg";

    [SupplyParameterFromForm]
    private CreateMovieViewModel CreateMovieViewModel { get; set; } = new CreateMovieViewModel();

    private List<MovieType> MovieTypes { get; set; } = [];

    private string? _previewUrl;

    private IBrowserFile? _browserFile;

    private string? _statusMessage;

    [Inject]
    private IMovieService MovieService { get; set; } = default!;
    [Inject]
    private IMovieTypeService MovieTypeService { get; set; } = default!;

    [Inject]
    private IMapper Mapper { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var getMovieTypesResult = await MovieTypeService.GetMovieTypes();
        if (!getMovieTypesResult.IsSuccess)
        {
            _statusMessage = "Error when loading data";
            return;
        }
        MovieTypes = [.. getMovieTypesResult.Value];
    }

    private async Task HandleSubmit()
    {
        var dto = Mapper.Map<CreateMovieDto>(CreateMovieViewModel);
        dto.MovieTypeId = ObjectId.Parse(CreateMovieViewModel.SelectedMovieTypeId);
        dto.MovieTypeName = MovieTypes.First(mt => mt.Id == dto.MovieTypeId).Name;
        Result<ObjectId> createMovieResult;
        if (_browserFile == null)
        {
            createMovieResult = await MovieService.CreateMovieAsync(dto, null, null);
        }
        else
        {
            await using var stream = _browserFile.OpenReadStream(MaxFileSize);
            createMovieResult = await MovieService.CreateMovieAsync(dto, stream, Path.GetExtension(_browserFile.Name));
        }

        if (!createMovieResult.IsSuccess)
        {
            _statusMessage = "Error while sending";
            return;
        }

        _statusMessage = "The movie was successfully created!";
    }

    private async Task HandleSelected(InputFileChangeEventArgs e)
    {
        if (e.File.Size > MaxFileSize)
        {
            _statusMessage = "Max size image - 2MB";
            return;
        }
        _browserFile = e.File;

        await using var stream = e.File.OpenReadStream(MaxFileSize);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        _previewUrl = $"data:{AcceptedImageType};base64,{Convert.ToBase64String(ms.ToArray())}";
    }
}