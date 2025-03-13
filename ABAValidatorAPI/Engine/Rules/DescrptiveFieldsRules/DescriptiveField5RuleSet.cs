using System.Text.RegularExpressions;
namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DescriptiveField5RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 5;


        public DescriptiveField5RuleSet()
        {
            _rules.Add(new DescriptiveField5Rule1());
        }
        internal partial class DescriptiveField5Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DescriptiveChar24_30();

            public override string ErrorMessage => "Char 24-30; Size 7; Must be blank filled.";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.{23} {7}.*$")]
            private static partial Regex DescriptiveChar24_30();
        }
    }
}
