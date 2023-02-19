using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidService.Repositories
{
    public interface IGithubService
    {
        Task<Stream> DownloadFile();
    }
}
