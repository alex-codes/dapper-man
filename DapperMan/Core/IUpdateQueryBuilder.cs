namespace DapperMan.Core
{
    public interface IUpdateQueryBuilder
    {
        IUpdateQueryBuilder Where(string filter);
    }
}
