using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Attar.CRUD.PL.Helper
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            //1.Get Located Folder Path
            //string folderPath = $"C:\\Users\\MOHAMED\\OneDrive\\Music\\Desktop\\Attar.C41.G02\\Attar.C41.G02.PL\\wwwroot\\files\\images\\{folderName}";

            //string folderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\images\\{folderName}";

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //2.Get File Name and make it unique

            string fileName = $"{Guid.NewGuid()} {Path.GetExtension(file.FileName)}";

            //3.Get File Path 

            string filePath = Path.Combine(folderPath, fileName);

            //4. save file as Streams 

            var fileStram = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fileStram);

            return fileName;




        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
