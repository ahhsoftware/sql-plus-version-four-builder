namespace SQLPLUS.Builder
{
    using System;
    public class FileWriteEventArgs : EventArgs
    {
        public FileWriteEventArgs(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}
