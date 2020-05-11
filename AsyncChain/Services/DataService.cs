using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestAsync.Helpers;
using TestAsync.Interfaces;

namespace TestAsync.Services
{
    public class DataService : IDataService, IDisposable
    {
        private readonly ILogger<IDataService> _logger;

        private string _tmpFileName;

        private const int DataSize = 5000; // KB

        public DataService(ILogger<IDataService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetCountAsync(int index)
        {
            CommonHelper.LogRequestTreadInfo(_logger, index, "Async HTTP I/O point 1");

            string result;

            using (var client = new HttpClient())
            {
                var url = @"http://www.mocky.io/v2/5eb6a45a3100002b1ac89f8d";

                var response = await client.GetAsync(new Uri(url));
                //var response = client.GetAsync(new Uri(url)).Result;

                CommonHelper.LogRequestTreadInfo(_logger, index, "Async HTTP I/O point 2");

                result = await response.Content.ReadAsStringAsync();
                //result = response.Content.ReadAsStringAsync().Result;
            }

            CommonHelper.LogRequestTreadInfo(_logger, index, "Async HTTP I/O point 3");

            return result;
        }

        public async Task<int> LongDataProcessingAsync(int index)
        {
            _tmpFileName = GetTmpFileName(index);
            var data = GetByteArray(DataSize);

            CommonHelper.LogRequestTreadInfo(_logger, index, "Async file I/O point 1");

            await WriteBytesAsync(_tmpFileName, data);

            CommonHelper.LogRequestTreadInfo(_logger, index, "Async file I/O point 2");
            
            data = await ReadBytesAsync(_tmpFileName);

            CommonHelper.LogRequestTreadInfo(_logger, index, "Async file I/O point 3");
            return data.Length;
        }

        private static string GetTmpFileName(int index)
        {
            string path = Assembly.GetExecutingAssembly().Location;

            var tmpDir = Path.Combine(Path.GetDirectoryName(path), "tmp");
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }

            var fileName = $"{index}.dat";

            var tmpFileName = Path.Combine(tmpDir, fileName);

            return tmpFileName;
        }

        private byte[] GetByteArray(int sizeInKb)
        {
            Random rnd = new Random();
            Byte[] b = new Byte[sizeInKb * 1024]; // convert kb to byte
            rnd.NextBytes(b);
            return b;
        }

        public async Task WriteBytesAsync(string filePath, byte[] data)
        {
            using (FileStream sourceStream =
                new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(data, 0, data.Length);
            }
        }

        public async Task<byte[]> ReadBytesAsync(string filePath)
        {
            byte[] result;

            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
            }

            return result;
        }

        public void Dispose()
        {
            if (File.Exists(_tmpFileName)) File.Delete(_tmpFileName);
        }
    }
}
