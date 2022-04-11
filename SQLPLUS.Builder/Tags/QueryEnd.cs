namespace SQLPLUS.Builder.Tags
{
    public class QueryEnd : BaseTag
    {
        public QueryEnd()
            : base(null, null)
        {
            this.TagContext = TagContexts.Query;
            this.TagType = TagTypes.QueryEnd;
        }
    }
}
