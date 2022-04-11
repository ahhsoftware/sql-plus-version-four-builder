namespace SQLPLUS.Builder
{
    using System;

    public class FileDeletedEventArgs :EventArgs
    {
        public FileDeletedEventArgs(string deletedFileName)
        {
            DeletedFileName = deletedFileName;
        }

        public string DeletedFileName { get; }
    }
}
