using Delab.AccessData.Data;
using Delab.Backend.Class;
using Delab.Common.Helper;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Delab.Backend.Controllers
{
    [Route("api/corporations")]
    [ApiController]
    public class CorporationsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IFileStorage _fileStorage;
        private readonly string  _pahFileStorage;
        private readonly AppSettings _appSetting;

        public CorporationsController(DataContext dataContext,IFileStorage fileStorage,IOptions<AppSettings> options)
        {
            this._dataContext = dataContext;
            this._fileStorage = fileStorage;
            _appSetting = options.Value;
            _pahFileStorage = _appSetting.PathImages;

        }
        //[HttpGet("coporation-all/{long:id}")]
        [HttpGet]
        public async Task<IActionResult> getCorporationsAll()
        {
            var corporations = await _dataContext.Corporations.ToListAsync();

            return Ok(corporations);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Corporation>> GetIdAsync(int id)
        {
            try
            {
                var modelo = await _dataContext.Corporations.FindAsync(id);
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
        public async Task<ActionResult<Corporation>> PostAsync(Corporation model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.ImgBase64))
                {
                    string guid = Guid.NewGuid().ToString() + ".jpg";
                    var imageId = Convert.FromBase64String(model.ImgBase64);
                    model.Imagen = await _fileStorage.UploadImage(imageId, _pahFileStorage, guid);
                }
                _dataContext.Corporations.Add(model);
                await _dataContext.SaveChangesAsync();

                return Ok(model);
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya Existe un Registro con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbEx.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult> PutAsync(Corporation model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.ImgBase64))
                {
                    string guid;
                    if (model.Imagen == null)
                    {
                        guid = Guid.NewGuid().ToString() + ".jpg";
                    }
                    else
                    {
                        guid = model.Imagen;
                    }
                    var imageId = Convert.FromBase64String(model.ImgBase64);
                    model.Imagen = await _fileStorage.UploadImage(imageId, _pahFileStorage, guid);
                }
                _dataContext.Corporations.Update(model);
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya Existe un Registro con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbEx.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var DataRemove = await _dataContext.Corporations.FindAsync(id);
                if (DataRemove == null)
                {
                    return BadRequest("Problemas para conseguir el registro");
                }
                _dataContext.Corporations.Remove(DataRemove);
                await _dataContext.SaveChangesAsync();

                if (DataRemove.Imagen is not null)
                {
                    var response = _fileStorage.DeleteImage(_pahFileStorage, DataRemove.Imagen);
                    if (!response)
                    {
                        return BadRequest("Se Elimino el Registro pero sin la Imagen");
                    }
                }

                return Ok();
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException!.Message.Contains("REFERENCE"))
                {
                    return BadRequest("No puede Eliminar el registro porque tiene datos Relacionados");
                }
                else
                {
                    return BadRequest(dbEx.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
