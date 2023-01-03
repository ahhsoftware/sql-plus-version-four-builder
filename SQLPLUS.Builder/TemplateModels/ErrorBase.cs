namespace SQLPLUS.Builder.TemplateModels
{
    public class ErrorBase
    {
        public string ErrorMessage { set; get; }

        public bool HasError
        {
            get
            {
                return !string.IsNullOrEmpty(ErrorMessage);
            }
        }

        public bool HasNoError
        {
            get
            {
                return !HasError;
            }
        }
    }
}
