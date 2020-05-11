using System.Threading.Tasks;

namespace TestAsync.Interfaces
{
    public interface IBusinessLogicService
    {
        Task<string> GetCountAsync(int index);
    }
}
