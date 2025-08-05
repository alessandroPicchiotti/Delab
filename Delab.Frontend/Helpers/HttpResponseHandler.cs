using CurrieTechnologies.Razor.SweetAlert2;

using Delab.AccessService.Repositories;
using Delab.Frontend.AuthenticationProviders;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace Delab.Frontend.Helpers;

public class HttpResponseHandler
{
    private readonly ILoginService _loginService;
    private readonly NavigationManager _navigationManager;
    private readonly SweetAlertService _sweetAlert;

    public HttpResponseHandler(ILoginService loginService,
        NavigationManager navigationManager,
        SweetAlertService sweetAlert)
    {
        _loginService = loginService;
        _navigationManager = navigationManager;
        _sweetAlert = sweetAlert;
    }

    public async Task<bool> HandleErrorAsync<T>(HttpResponseWrapper<T> responseHttp) where T : class
    {
        if (responseHttp.HttpResponseMessage == null) return false; // No hay respuesta HTTP

        var statusCode = responseHttp.HttpResponseMessage.StatusCode;

        switch (statusCode)
        {
            case HttpStatusCode.Unauthorized:
                await _sweetAlert.FireAsync("Error", "Devi effettuare nuovamente l'accesso", SweetAlertIcon.Error);
                await _loginService.LogoutAsync();
                _navigationManager.NavigateTo($"/");
                return true;

            case HttpStatusCode.Forbidden:
                await _sweetAlert.FireAsync("Error", "Non hai l'autorizzazione per accedere a questa risorsa", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.NotFound:
                await _sweetAlert.FireAsync("Error", "Registrazione non trovata", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.InternalServerError:
                await _sweetAlert.FireAsync("Error", "Errore interno del server. Riprova più tardi.", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.BadRequest:
                var badRequestMessage = await responseHttp.GetErrorMessageAsync();
                await _sweetAlert.FireAsync("Error", $"Bad Request: {badRequestMessage}", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.GatewayTimeout:
                await _sweetAlert.FireAsync("Error", "Il server non ha risposto in tempo. Riprova più tardi.", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.ServiceUnavailable:
                await _sweetAlert.FireAsync("Error", "Il servizio è temporaneamente non disponibile. Riprova più tardi.", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.BadGateway:
                await _sweetAlert.FireAsync("Error", "Il server di backup non ha risposto correttamente. Riprova più tardi.", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.RequestTimeout:
                await _sweetAlert.FireAsync("Error", "La richiesta ha richiesto troppo tempo. Riprova più tardi.", SweetAlertIcon.Error);
                return true;

            case HttpStatusCode.UnprocessableEntity:
                await _sweetAlert.FireAsync("Error", "I dati inviati non sono validi. Controlla le informazioni inserite.", SweetAlertIcon.Error);
                return true;

            default:
                var messageError = await responseHttp.GetErrorMessageAsync();
                if (messageError != null)
                {
                    await _sweetAlert.FireAsync("Error", messageError, SweetAlertIcon.Error);
                    return true;
                }
                return false;
        }
    }
}
