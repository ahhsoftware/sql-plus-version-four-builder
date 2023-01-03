namespace SQLPlusExtension.Models
{
    public class StaticQuery : QueryBase
    {
        public StaticQuery(Action<object> deleteCallback) : base(deleteCallback) { }
    }
}
