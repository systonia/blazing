using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazing;

public partial class BzDialog : ComponentBase
{
    [Parameter]
    public required RenderFragment ChildContent { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference? _module;
    private ElementReference _dialogRef;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/Blazing/BzDialog.razor.js"
            );
        }
    }

    public async Task Show()
    {
        if (_module is null)
        {
            return;
        }
        
        await _module.InvokeVoidAsync("show", _dialogRef);
    }

    public async Task Hide()
    {
        if (_module is null)
        {
            return;
        }
        
        await _module.InvokeVoidAsync("hide", _dialogRef);
    }
}