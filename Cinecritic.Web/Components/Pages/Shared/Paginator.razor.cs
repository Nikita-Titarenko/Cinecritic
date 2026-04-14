using Microsoft.AspNetCore.Components;

namespace Cinecritic.Web.Components.Pages.Shared;

public partial class Paginator
{
    public const int PageSize = 12;
    public const int PagePaginatorCount = 3;
    public const int PaginatorAdditionalCount = 4;
    [Parameter]
    public int TotalPageNumber { get; set; }
    [Parameter]
    public int CurrentPage { get; set; }
    [Parameter]
    public EventCallback<int> OnClick { get; set; }
}