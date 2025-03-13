using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class CommonRecordRule
    {
        private CommonRecordRule1 _rule;

        public CommonRecordRule()
        {
            _rule = new CommonRecordRule1();
        }

        public void ValidateRecord(RecordValidationResult result, string line)
        {
            if (!_rule.IsValid(line))
            {
                result.Errors.Add(_rule.ErrorMessage);

                result.IsValid = false;
            }
        }

        internal partial class CommonRecordRule1 : RegexFieldRule
        {
            protected override Regex Regex => Line120();

            public override string ErrorMessage => "An ABA record is exactly 120 characters long (excluding new line characters)";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.{120}$")]
            private static partial Regex Line120();
        }
    }
}
