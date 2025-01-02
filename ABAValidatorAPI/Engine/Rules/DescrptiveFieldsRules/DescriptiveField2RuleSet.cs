using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DescriptiveField2RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 2;

        public DescriptiveField2RuleSet()
        {
            _rules.Add(new DescriptiveField2Rule1());
        }

        internal partial class DescriptiveField2Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DescriptiveChar2_18();

            public override string ErrorMessage => "Char 2-18, Size 17, Blank, Must be filled";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.[ ]{17}.*$")]
            private static partial Regex DescriptiveChar2_18();
        }
    }
}
