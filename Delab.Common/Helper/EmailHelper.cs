using Delab.Shared.Class;
using Delab.Shared.DTOs;
using Delab.Shared.Responses;
using Microsoft.Extensions.Options;
using SendGrid;

//using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Response = Delab.Shared.Responses.Response;

namespace Delab.Common.Helper;

public class EmailHelper : IEmailHelper
{
    private readonly SendGridSettings _sendGridOption;

    public EmailHelper(IOptions<SendGridSettings> options )
    {
        this._sendGridOption = options.Value;
    }
    public async Task<Response> ConfirmarAccount(string to, string NomeDestinatario, string subject, string body)
    {
        
        var apiKey = _sendGridOption.ApiKey;
        var email = _sendGridOption.From;
        var mittente = _sendGridOption.Name;

        //Cargamos la Utilidad de SendGrid, que es el sistema de envio de datos.
        var cliente = new SendGridClient(apiKey);
        var from = new EmailAddress(email, mittente);
        var tO = new EmailAddress(to, NomeDestinatario);
        var oggetto = "Sistema di conferma";
        var singleEmail = MailHelper.CreateSingleEmail(from, tO, subject,
            oggetto, body);

        var risposta = await cliente.SendEmailAsync(singleEmail);
        if (risposta.IsSuccessStatusCode)
        {
            return new Response { IsSuccess = true };
        }
        else
        {
            return new Response { IsSuccess = false };
        }
    }

    public async Task<bool> SendAsync(ContactDTO contatto)
    {
        
        var apiKey = _sendGridOption.ApiKey;
        var email = _sendGridOption.From;
        var mittente = _sendGridOption.Name;

        //Cargamos la Utilidad de SendGrid, que es el sistema de envio de datos.
        var cliente = new SendGridClient(apiKey);
        var from = new EmailAddress(email, mittente);
        var subject = $"El Cliente {contatto.Numero} quiere contactarte";
        var to = new EmailAddress(email, mittente);
        var massaggio = contatto.Message;
        var contenidoHtml = $@"De: {contatto.Numero}
            <p>
            Email: {contatto.Email}
            <p/>
            <p>
            Mensaje: {contatto.Message}
            <p/>";
        var singleEmail = MailHelper.CreateSingleEmail(from, to, subject, massaggio, contenidoHtml);

        var risposta = await cliente.SendEmailAsync(singleEmail);

        return risposta.IsSuccessStatusCode;
    }

    
}
