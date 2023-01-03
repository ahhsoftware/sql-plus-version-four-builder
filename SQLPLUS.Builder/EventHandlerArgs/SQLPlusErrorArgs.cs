using System;

namespace SQLPLUS.Builder
{
    public class SQLPlusErrorArgs : EventArgs
    {
        public SQLPlusErrorArgs(string source, string error)
        {
            Source = source;
            Error = error;
        }

        public string Source { get; }
        public string Error { get; }
    }
}