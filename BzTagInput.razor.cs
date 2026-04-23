using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazing;

public partial class BzTagInput : ComponentBase, IAsyncDisposable
{
    [Parameter] public EventCallback onTrySubmit { get; set; }
    Dictionary<int, ElementReference> SpacerInputElements { get; set; }
    ElementReference TagInputElement { get; set; }
    private string TagInputValue { get; set; } = string.Empty;
    List<string> Tags { get; set; }

    public BzTagInput()
    {
        Tags = new List<string>();
        SpacerInputElements = new Dictionary<int, ElementReference>();
    }

    protected override Task OnInitializedAsync()
    {
        Tags.Add("Slice of Life");
        Tags.Add("-Harem");
        Tags.Add("Loli");
        StateHasChanged();
        
        return base.OnInitializedAsync();
    }

    public async void AfterInputHandler(KeyboardEventArgs e)
    {
        if (TagInputValue.Length > 0 && (e.Code == "Enter" || e.Code == "NumpadEnter"))
        {
            Tags.Add(TagInputValue);
            TagInputValue = string.Empty;
            await TagInputElement.FocusAsync();
            StateHasChanged();
            return;
        }

        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            // trigger submit from here
            await onTrySubmit.InvokeAsync();
        }
    }

    public async void BeforeInputHandler(KeyboardEventArgs e)
    {
        if (TagInputValue.Length == 0 && e.Code == "Backspace" && Tags.Count > 0)
        {
            Tags.RemoveAt(Tags.Count - 1);
            StateHasChanged();
        }

        if (TagInputValue.Length == 0 && e.Code == "ArrowLeft" && Tags.Count > 0)
        {
            // focus input before last tag
            await SpacerInputElements[Tags.Count - 1].FocusAsync();
        }

        if (TagInputValue.Length == 0 && e.Code == "ArrowRight" && Tags.Count > 0)
        {
            // focus input before first tag
            await SpacerInputElements[0].FocusAsync();
        }
    }

    public async void SpacerKeyHandler(KeyboardEventArgs eventArgs, int index)
    {
        if (index > 0 && eventArgs.Code == "ArrowLeft")
        {
            // move one more left
            await SpacerInputElements[index - 1].FocusAsync();
            return;
        }

        if (index < Tags.Count - 1 && eventArgs.Code == "ArrowRight")
        {
            // move one more right
            await SpacerInputElements[index + 1].FocusAsync();
            return;
        }

        if (index == Tags.Count - 1 && eventArgs.Code == "ArrowRight")
        {
            // move to InputElement
            await TagInputElement.FocusAsync();
            return;
        }

        if (eventArgs.Code == "Backspace" && Tags.Count > 0)
        {
            Tags.RemoveAt(index - 1);
            StateHasChanged();
            if (index == Tags.Count)
            {
                await TagInputElement.FocusAsync();
            }
            else
            {
                await SpacerInputElements[index].FocusAsync();
            }

            return;
        }

        if (eventArgs.Code == "Delete" && Tags.Count > 0)
        {
            Tags.RemoveAt(index);
            StateHasChanged();
            if (index == Tags.Count)
            {
                await TagInputElement.FocusAsync();
            }
            else
            {
                await SpacerInputElements[index].FocusAsync();
            }

            return;
        }

        await TagInputElement.FocusAsync();
        return;
    }

    public async void FocusHandler(MouseEventArgs eventArgs)
    {
        await TagInputElement.FocusAsync();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}