using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace eStartup_Back_End.Utilities
{
    public static class FileUtility
    {
        public static async Task<string> PathFile(this IFormFile file,string route,string folder)
        {
            string fileName = file.FileName;
            string filePath = Path.Combine(route, folder);
            string fullPath = Path.Combine(filePath, fileName);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
