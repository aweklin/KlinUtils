using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class FileComponent : ComponentBase
{
    [Parameter]
    public string Type { get; set; } = "text";

    [Parameter]
    public IBrowserFile SelectedFile { get; set; } = null!;

    [Parameter]
    public string CssClass { get; set; } = "w-full mt-1 px-4 py-2 border rounded focus:ring-green-500 focus:border-green-500";

    [Parameter]
    public string Placeholder { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<IBrowserFile> SelectedFileChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    public bool IsEnabled => !IsDisabled;

    public async Task SetSelectedFile(IBrowserFile selectedFile)
    {
        SelectedFile = selectedFile;
        await InvokeAsync(() => SelectedFileChanged.InvokeAsync(SelectedFile)).ConfigureAwait(false);
    }

    public Task OnSelectedFileChanged(InputFileChangeEventArgs inputFileChangeEventArgs)
    {
        ArgumentNullException.ThrowIfNull(inputFileChangeEventArgs);

        SelectedFile = inputFileChangeEventArgs.File;
        return SelectedFileChanged.InvokeAsync(SelectedFile);
    }

    public void Disable()
    {
        IsDisabled = true;
        InvokeAsync(StateHasChanged);
    }

    public void Enable()
    {
        IsDisabled = false;
        InvokeAsync(StateHasChanged);
    }
}