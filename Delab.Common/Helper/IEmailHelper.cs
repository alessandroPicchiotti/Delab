using Delab.Shared.DTOs;
using Delab.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Common.Helper;

public interface IEmailHelper
{
    //Invio email
    Task<bool> SendAsync(ContactDTO contacto);

    //Sistema per conferma
    Task<Response> ConfirmarAccount(string to, string NomeDestinatario, string subject, string body);

}
