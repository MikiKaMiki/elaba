using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DescriptiveField1RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 1;

        public DescriptiveField1RuleSet()
        {
            _rules.Add(new DescriptiveField1Rule1());
        }

        internal partial class DescriptiveField1Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DescriptiveChar1();

            public override string ErrorMessage => "Char 1, Size 1, Record type 0, Must be '0'";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^0.*$")]
            private static partial Regex DescriptiveChar1();
        }
    }
}
