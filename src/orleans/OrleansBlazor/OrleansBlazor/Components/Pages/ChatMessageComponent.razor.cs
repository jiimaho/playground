using Microsoft.AspNetCore.Components;
using Orleans.Silo;

namespace OrleansBlazor.Components.Pages;

public partial class ChatMessageComponent : ComponentBase
{
   [Parameter] 
    public ChatMessage Message { get; set; }


    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }
}