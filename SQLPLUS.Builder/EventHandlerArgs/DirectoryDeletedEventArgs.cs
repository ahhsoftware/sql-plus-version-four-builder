namespace SQLPLUS.Builder
{
    using System;

    public class DirectoryDeletedEventArgs : EventArgs
    {
        public DirectoryDeletedEventArgs(string directoryName)
        {
            DirectoryName = directoryName;
        }

        public string DirectoryName { get; }
    }
}
