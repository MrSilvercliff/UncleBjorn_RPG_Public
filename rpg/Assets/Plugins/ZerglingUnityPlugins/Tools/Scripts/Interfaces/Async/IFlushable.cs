using System.Threading.Tasks;

namespace ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async
{
    public interface IFlushable
    {
        Task<bool> Flush();
    }
}
