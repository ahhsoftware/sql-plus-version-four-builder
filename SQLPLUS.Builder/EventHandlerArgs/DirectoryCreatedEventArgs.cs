namespace SQLPLUS.Builder
{
    using System;

    public class DirectoryCreatedEventArgs : EventArgs
    {
        public DirectoryCreatedEventArgs(string newDirectoryPath)
        {
            NewDirectoryPath = newDirectoryPath;
        }

        public string NewDirectoryPath { get; }
    }
}
