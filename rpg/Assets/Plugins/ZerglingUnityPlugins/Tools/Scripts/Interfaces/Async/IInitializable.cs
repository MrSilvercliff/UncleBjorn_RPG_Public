using System.Threading.Tasks;

namespace ZerglingUnityPlugins.Tools.Scripts.Interfaces.Async
{
    public interface IInitializable
    {
        Task<bool> Init();
    }
}

