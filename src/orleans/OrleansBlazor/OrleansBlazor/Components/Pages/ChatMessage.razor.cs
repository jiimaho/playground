using Microsoft.AspNetCore.Components;
using Orleans.Silo;

namespace OrleansBlazor.Components.Pages;

public partial class ChatMessage : ComponentBase
{
   [Parameter] 
    public Orleans.Silo.Primitives.ChatMessage Message { get; set; }
}