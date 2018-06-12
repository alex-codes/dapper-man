namespace DapperMan.Core
{
    public interface IDeleteQueryBuilder : INonQuery
    {
        IDeleteQueryBuilder Where(string filter);
    }
}
