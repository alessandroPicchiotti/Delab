using Delab.AccessData.Data;
using Delab.Backend.Class;
using Delab.Backend.Helpers;
using Delab.Common.Helper;
using Delab.Shared.DTOs;
using Delab.Shared.Entities;
using Delab.Shared.Enum;
using Delab.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace Delab.Backend.Controllers;

[Route("api/manager")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IUserHelper _userHelper;
    private readonly IFileStorage _fileStorage;
    private readonly IConfiguration _configuration;
    private readonly IEmailHelper _emailHelper;
    private readonly string ImgRoute;
    public ManagerController(DataContext context, IUserHelper userHelper, IFileStorage fileStorage,
    IConfiguration configuration, IEmailHelper emailHelper, IOptions<AppSettings> options) 
    {
        _context = context;
        _userHelper = userHelper;
        _fileStorage = fileStorage;
        _configuration = configuration;
        _emailHelper = emailHelper;
        ImgRoute = Path.Combine( options.Value.PathImages,"ImgManager");
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Manager>>> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var queryable = _context.Managers.Include(x => x.Corporation).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.FullName!.ToLower().Contains(pagination.Filter.ToLower()));
        }

        await HttpContext.InsertParameterPagination(queryable, pagination.RecordsNumber);
        return await queryable.OrderBy(x => x.FullName).Paginate(pagination).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Manager>> GetOneAsync(int id)
    {
        try
        {
            var modelo = await _context.Managers
        .Include(x => x.Corporation).FirstOrDefaultAsync(x => x.ManagerId == id);
            if (modelo == null)
            {
                return BadRequest("Problemas para conseguir el registro");
            }
            return Ok(modelo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<Manager>> PostAsync(Manager model)
    {
        try
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                return BadRequest("L'utente non è registrato. Provare un'altro utente");
            }

            //En Caso de Fallo Restaurar la Base de Datos
            var transction = await _context.Database.BeginTransactionAsync();

            model.FullName = $"{model.FirstName} {model.LastName}";
            model.UserType = TypeUser.Cliente;
            if (!string.IsNullOrEmpty(model.ImgBase64))
            {
                string guid = Guid.NewGuid().ToString() + ".jpg";
                var imageId = Convert.FromBase64String(model.ImgBase64);
                model.Photo = await _fileStorage.UploadImage(imageId, ImgRoute, guid);
            }
            _context.Managers.Add(model);
            await _context.SaveChangesAsync();

            //Registro del Usuario en User
            if (model.Active)
            {
                Response response = await AcivateUser(model);
                if (!response.IsSuccess)
                {
                    var guid = model.Photo;
                    _fileStorage.DeleteImage(ImgRoute, guid!);
                    await transction.RollbackAsync();
                    return BadRequest("Utnte non creato");
                }
            }
            await transction.CommitAsync();
            return Ok(model);

        }
        catch (Exception)
        {

            throw;
        }


    }
    [HttpPut]
    public async Task<IActionResult> PutAsync(Manager model)
    {
        try
        {
            Manager NewModelo = new()
            {
                ManagerId = model.ManagerId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = $"{model.FirstName} {model.LastName}",
                Nro_Document = model.Nro_Document,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                UserName = model.UserName,
                CorporationId = model.CorporationId,
                Job = model.Job,
                UserType = model.UserType,
                Photo = model.Photo,
                Active = model.Active,
            };
            if (model.ImgBase64 != null)
            {
                NewModelo.ImgBase64 = model.ImgBase64;
            }

            //Respaldamos la base de datos antes de hacer operaciones
            var transaction = await _context.Database.BeginTransactionAsync();

            if (!string.IsNullOrEmpty(model.ImgBase64))
            {
                string guid;
                if (model.Photo == null)
                {
                    guid = Guid.NewGuid().ToString() + ".jpg";
                }
                else
                {
                    guid = model.Photo;
                }
                var imageId = Convert.FromBase64String(model.ImgBase64);
                NewModelo.Photo = await _fileStorage.UploadImage(imageId, ImgRoute, guid);
            }
            _context.Managers.Update(NewModelo);
            await _context.SaveChangesAsync();

            User UserCurrent = await _userHelper.GetUserAsync(model.UserName);
            if (UserCurrent != null)
            {
                UserCurrent.FirstName = model.FirstName;
                UserCurrent.LastName = model.LastName;
                UserCurrent.FullName = $"{model.FirstName} {model.LastName}";
                UserCurrent.PhoneNumber = model.PhoneNumber;
                UserCurrent.PhotoUser = model.Photo;
                UserCurrent.JobPosition = model.Job;
                UserCurrent.Active = model.Active;
                IdentityResult result = await _userHelper.UpdateUserAsync(UserCurrent);
            }
            else
            {
                if (model.Active)
                {
                    Response response = await AcivateUser(model);
                    if (response.IsSuccess == false)
                    {
                        var guid = model.Photo;
                        _fileStorage.DeleteImage(ImgRoute, guid!);
                        await transaction.RollbackAsync();
                        return BadRequest("No se ha podido crear el Usuario, Intentelo de nuevo");
                    }
                }
            }

            await transaction.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    private async Task<Response> AcivateUser(Manager manager)
    {
        User user = await _userHelper.AddUserAsync(manager.FirstName, manager.LastName, manager.UserName,
            manager.PhoneNumber, manager.Address, manager.Job, manager.CorporationId, manager.Photo!, "Manager", manager.Active, manager.UserType);

        //Envio de Correo con Token de seguridad para Verificar el correo
        string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
        string tokenLink = Url.Action("ConfirmEmail", "accounts", new
        {
            userid = user.Id,
            token = myToken
        }, HttpContext.Request.Scheme, _configuration["UrlFrontend"])!.Replace("api/managers", "api/accounts");//invio link dell'account dove conferma l'account

        string subject = "Activacion de Cuenta";
        string body = ($"De: MioSito" +
            $"<h1>Email Confirmation</h1>" +
            $"<p>" +
            $"Su Clave Temporal es: <h2> \"{user.Pass}\"</h2>" +
            $"</p>" +
            $"Para Activar su vuenta, " +
            $"Has Click en el siguiente Link:</br></br><strong><a href = \"{tokenLink}\">Confirmar Correo</a></strong>");

        return await _emailHelper.ConfirmarAccount(user.UserName!, user.FullName!, subject, body);
        
    }
}