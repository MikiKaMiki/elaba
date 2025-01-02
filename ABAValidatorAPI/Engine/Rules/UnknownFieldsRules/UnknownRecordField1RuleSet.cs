namespace ABAValidatorAPI.Engine.Rules
{
    public class UnknownRecordField1RuleSet : IFieldRuleSet
    {
        private IList<IFieldRule> _rules;

        public UnknownRecordField1RuleSet()
        {
            _rules = [
                new UnknownRecordField1Rule1()
                ];
        }

        public FieldValidationResutl ValidateField(string line)
        {
            var result = new FieldValidationResutl();

            foreach (var rule in _rules)
            {
                if (!rule.IsValid(line))
                {
                    result.Errors.Add(rule.ErrorMessage);
                }
            }

            result.IsValid = result.Errors.Count == 0;

            return result;
        }

        internal partial class UnknownRecordField1Rule1 : IFieldRule
        {
            public string ErrorMessage => "Char 1, Size 1, Record type Unknown, Must be '0' or '1' or '7'";

            public bool IsValid(string line)
            {
                return false;
            }
        }
    }
}
