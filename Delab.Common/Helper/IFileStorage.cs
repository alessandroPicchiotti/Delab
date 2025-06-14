using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Common.Helper;

public interface IFileStorage
{
    Task<string> UploadImage(IFormFile imageFile, string path, string guid);

    Task<string> UploadImage(byte[] imageFile, string path, string guid);

    bool DeleteImage(string path, string guid);
}
