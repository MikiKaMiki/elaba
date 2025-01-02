
namespace ABAValidatorAPI.Engine.Rules
{
    public interface IFieldRuleSet
    {
        FieldValidationResutl ValidateField(string line);
    }
}
