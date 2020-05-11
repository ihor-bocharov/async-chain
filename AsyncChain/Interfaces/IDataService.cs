using System.Threading.Tasks;

namespace TestAsync.Interfaces
{
    public interface IDataService
    {
        Task<string> GetCountAsync(int index);

        Task<int> LongDataProcessingAsync(int index);
    }
}
