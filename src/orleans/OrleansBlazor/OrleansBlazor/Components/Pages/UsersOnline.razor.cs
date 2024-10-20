using Microsoft.AspNetCore.Components;

namespace OrleansBlazor.Components.Pages;

public partial class UsersOnline : ComponentBase
{
    [Parameter]
    public List<string> AllUsersOnline { get; set; } = new();
}