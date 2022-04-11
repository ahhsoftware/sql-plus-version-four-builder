namespace SQLPLUS.Builder
{
    using System;

    public class FileCreatedEventArgs : EventArgs
    {
        public FileCreatedEventArgs(string newFileName)
        {
            NewFileName = newFileName;
        }

        public string NewFileName { get; }
    }
}
