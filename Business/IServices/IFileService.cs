using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, string folder);

        Task DeleteFile(string fileName, string folder);
    }
}
