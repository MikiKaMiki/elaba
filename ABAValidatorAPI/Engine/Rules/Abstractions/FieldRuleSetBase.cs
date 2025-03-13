namespace ABAValidatorAPI.Engine.Rules
{
    public abstract class FieldRuleSetBase : IFieldRuleSet
    {
        protected abstract short FieldNumber { get; }

        protected readonly List<IFieldRule> _rules = [];

        public virtual FieldValidationResutl ValidateField(string line)
        {
            var result = NewResult();

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

        public FieldValidationResutl NewResult()
        {
            return new FieldValidationResutl() { FieldNumber = FieldNumber };
        }
    }
}
