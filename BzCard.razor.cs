using Microsoft.AspNetCore.Components;

namespace Blazing;

public partial class BzCard : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}