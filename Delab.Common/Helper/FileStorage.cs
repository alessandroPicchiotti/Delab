using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Common.Helper;

public class FileStorage : IFileStorage
{
    public async Task<string> UploadImage(IFormFile imageFile, string ruta, string guid)
    {
        var file = guid;
        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            ruta,
            file);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return $"{file}";
    }

    public async Task<string> UploadImage(byte[] imageFile, string path, string guid)
    {
        var file = guid;
        var pathLocal = Path.Combine(
            Directory.GetCurrentDirectory(),
            path,
            file);

        var NIformFile = new MemoryStream(imageFile);
        using (var stream = new FileStream(pathLocal, FileMode.Create))
        {
            await NIformFile.CopyToAsync(stream);
        }

        return $"{file}";
    }

    public bool DeleteImage(string ruta, string guid)
    {
        string path;
        path = Path.Combine(
            Directory.GetCurrentDirectory(),
            ruta,
            guid);

        File.Delete(path);

        return true;
    }
}
