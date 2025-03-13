namespace ABAValidatorAPI.Engine.Rules
{
    public interface IFieldRule
    {
        string ErrorMessage { get; }

        bool IsValid(string line);
    }
}
