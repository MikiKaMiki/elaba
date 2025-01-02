using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DetailField1RuleSet : IFieldRuleSet
    {
        private IList<IFieldRule> _rules;

        public DetailField1RuleSet()
        {
            _rules = [
                new DetailField1Rule1()
                ];
        }

        public FieldValidationResutl ValidateField(string line)
        {
            var result = new FieldValidationResutl();

            foreach(var rule in _rules)
            {
                if (!rule.IsValid(line))
                {
                    result.Errors.Add(rule.ErrorMessage);
                }
            }

            result.IsValid = result.Errors.Count == 0;

            return result;
        }

        internal partial class DetailField1Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DetailChar1();

            public override string ErrorMessage => "Char 1, Size 1, Record type 1, Must be '1'";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^1.*$")]
            private static partial Regex DetailChar1();
        }
    }
}
