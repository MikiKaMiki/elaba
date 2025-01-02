using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DescriptiveField6RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 6;

        public DescriptiveField6RuleSet()
        {
            _rules.Add(new DescriptiveField6Rule1());
        }

        public override FieldValidationResutl ValidateField(string line)
        {
            var result = new FieldValidationResutl();

            var field = line.Substring(31, 26);

            foreach (var rule in _rules)
            {
                if (!rule.IsValid(field))
                {
                    result.Errors.Add(rule.ErrorMessage);
                }
            }

            result.IsValid = result.Errors.Count == 0;

            return result;
        }

        internal partial class DescriptiveField6Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DescriptiveField6();

            public override string ErrorMessage 
                => "Must be User Preferred Specification as advised by User's FI. Left justified, blank filled. All coded character set valid. Must not be all blanks.";

            public override bool IsValid(string field)
            {
                return Regex.IsMatch(field);
            }

            [GeneratedRegex(@"^\S+( \S+)* *$")]
            private static partial Regex DescriptiveField6();
        }
    }
}
