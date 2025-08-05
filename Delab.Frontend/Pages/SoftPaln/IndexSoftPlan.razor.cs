namespace Delab.Frontend.Pages.SoftPaln;

using CurrieTechnologies.Razor.SweetAlert2;
using Delab.AccessService.Repositories;
using Delab.Frontend.AuthenticationProviders;
using Delab.Frontend.Helpers;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using MudBlazor;
using System.Runtime.CompilerServices;


public partial class IndexSoftPlan
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IDialogService _dialogService { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private string Filter { get; set; } = string.Empty;

    private int CurrentPage = 1;  //Pagina seleccionada
    private int TotalPages;      //Cantidad total de paginas
    private int PageSize = 15;  //Cantidad de registros por pagina

    private const string baseUrl = "api/softplans";
    public List<SoftPlan>? SoftPlans { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await Load();
    }

    private async Task SelectedPage(int page)
    {
        CurrentPage = page;
        await Load(page);
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await Load();
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        IDialogReference? dialog;
        if (isEdit)
        {
            var parameters = new DialogParameters
            {
                { "Id", id }
            };
            dialog = await _dialogService.ShowAsync<EditSoftPlan>($"Editar Plan", parameters, options);
        }
        else
        {
            dialog = await _dialogService.ShowAsync<CreateSoftPlan>($"Nuevo Plan", options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await Load();
        }
    }

    private async Task Load(int page = 1)
    {
        //Implementamos Interpolacion $" ";
        var url = $"{baseUrl}?page={page}&recordsNumber={PageSize}";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await _repository.GetAsync<List<SoftPlan>>(url);
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo("/");
            return;
        }

        SoftPlans = responseHttp.Response;

        TotalPages = int.Parse(responseHttp.HttpResponseMessage.Headers.GetValues("Totalpages").FirstOrDefault()!);
    }

    private async Task DeleteAsync(int id)
    {
        var result = await _sweetAlert.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmacion",
            Text = "Estas Seguro de Borrar el Registro",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true
        });
        var confirm = string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        var responseHttp = await _repository.DeleteAsync($"{baseUrl}/{id}");
        var errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo("/");
            return;
        }
        await Load();
    }
}