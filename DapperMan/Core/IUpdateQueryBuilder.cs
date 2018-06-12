namespace DapperMan.Core
{
    public interface IUpdateQueryBuilder : ICacheableNonQuery
    {
        IUpdateQueryBuilder Where(string filter);
    }
}
