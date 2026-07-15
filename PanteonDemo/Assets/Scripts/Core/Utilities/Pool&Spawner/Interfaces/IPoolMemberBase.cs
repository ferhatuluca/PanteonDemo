namespace Core.Utilities.Pool_Spawner.Interfaces
{
    public interface IPoolMemberBase
    {
        void OnEnterPool();
        void OnExitPool();
    }
}