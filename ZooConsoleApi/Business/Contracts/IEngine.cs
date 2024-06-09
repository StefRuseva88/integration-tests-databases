namespace ZooConsoleAPI.Business.Contracts
{
    public  interface IEngine
    {
        Task Run(IAnimalsManager animalsManager);
    }
}
