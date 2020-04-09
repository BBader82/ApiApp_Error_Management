namespace ApiApp_Male.helper
{
    public interface IErrorClass
    {
        string ErrorCode { get; set; }
        string ErrorMessage { get; set; }
        string ErrorProp { get; set; }

        void LoadError(string ErrorCode);
    }
}