using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class TotalField1RuleSet : IFieldRuleSet
    {
        private IList<IFieldRule> _rules;

        public TotalField1RuleSet()
        {
            _rules = [
                new TotalField1Rule1()
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

        internal partial class TotalField1Rule1 : RegexFieldRule
        {
            protected override Regex Regex => TotalChar1();

            public override string ErrorMessage => "Char 1, Size 1, Record type 7, Must be '7'";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^7.*$")]
            private static partial Regex TotalChar1();
        }
    }
}
