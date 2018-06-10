namespace DapperMan.Core
{
    public interface IDeleteQueryBuilder : IDapperNonQuery
    {
        IDeleteQueryBuilder Where(string filter);
    }
}
