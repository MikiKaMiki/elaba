using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DescriptiveField3RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 3;

        public DescriptiveField3RuleSet() : base ()
        {
            _rules.Add(new DescriptiveField3Rule1());
        }

        internal partial class DescriptiveField3Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DescriptiveChar19_20();

            public override string ErrorMessage => "Char 19-20, Size 2, Reel Sequence Number, Must be numeric commencing at 01. Right justified. Zero filled.";

            public override bool IsValid(string line)
            {   
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.{18}[0-9]{2}.*$")]
            private static partial Regex DescriptiveChar19_20();
        }
    }
}
