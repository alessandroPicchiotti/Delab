using Delab.AccessService.Repositories;
using Delab.Frontend.AuthenticationProviders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Delab.Frontend.Pages.Auth;

public partial class Logout
{
    [Inject] 
    private NavigationManager _navigation { get; set; } = null!;
    
    [Inject]
    private ILoginService _loginService { get; set; } = null!;
    
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private async Task LogoutActionAsync()
    {
        await _loginService.LogoutAsync();
        _navigation.NavigateTo("/");
        CancelAction();
    }
    private void CancelAction()
    {
        MudDialog.Cancel();
    }
}