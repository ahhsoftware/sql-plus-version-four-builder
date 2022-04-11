namespace SQLPLUS.Builder
{
    using System;
    public class ProgressStatusArgs : EventArgs
    {
        public ProgressStatusArgs(int progress, string message)
        {
            this.Progress = progress;
            this.Message = message;
        }

        public int Progress { set; get; }

        public string Message { set; get; }
    }
}
