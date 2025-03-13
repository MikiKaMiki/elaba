using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DetailField2RuleSet : IFieldRuleSet
    {
        private IList<IFieldRule> _rules;

        private IFieldRule _specialCaseRule;

        public DetailField2RuleSet()
        {
            _rules = 
                [
                    new DetailField4Rule1(),
                    new DetailField4Rule2(),
                    new DetailField4Rule3(),
                ];

            _specialCaseRule = new DetailField4SpecialCaseRule();
        }

        public FieldValidationResutl ValidateField(string line)
        {
            // figure aout if we do really need this special case
            var scResult = ValidateSpecialCase(line);

            if (scResult != null)
            {
                return scResult!;
            }

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

        // todo: figure out the flag for
        // — is this transaction — credits to Employee Benefits Card account?
        private FieldValidationResutl ValidateSpecialCase(string line)
        {
            var isSpecialCase = false; // todo: IsSpecialCase();


            if (!isSpecialCase)
            {
                return null;
            }

            var result = new FieldValidationResutl();

            if (!_specialCaseRule.IsValid(line))
            {
                result.Errors.Add(_specialCaseRule.ErrorMessage);
            }

            return result;
        }

        /// <summary>
        /// The rule checks the value of the field.
        /// For a specia case, when the transaction is
        /// — credits to Employee Benefits Card account transaction
        /// </summary>
        internal partial class DetailField4SpecialCaseRule : RegexFieldRule
        {
            protected override Regex Regex => DetailChar2_8();

            public override string ErrorMessage 
                => "Char 2-8; Size 7; For credits to Employee Benefits Card accounts, field must always contain BSB 032-898";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.032-898.*$")]
            private static partial Regex DetailChar2_8();
        }

        internal partial class DetailField4Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DetailChar2_8();

            public override string ErrorMessage => "Char 2-8; Size 7; Must be numeric with hyphen in character position 5.";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.\d{3}-\d{3}.*$")]
            private static partial Regex DetailChar2_8();
        }

        internal partial class DetailField4Rule2 : LookupFieldRule<string>
        {
            public override string ErrorMessage => "Character positions 2 and 3 must equal valid Financial Institution number";

            public override bool IsValid(string line)
            {
                var value = line.Substring(2, 2);

                return LookupList.Contains(value);
            }

            //todo: figure out valid Financial Institution (FI) numbers
            protected override IList<string> LookupList { get; } = [
                "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",

                "91", "92", "93", "94", "95", "96", "97", "98", "99"
            ];
        }

        internal partial class DetailField4Rule3 : LookupFieldRule<string>
        {
            public override string ErrorMessage => "Character positions 2 and 3 must equal valid Financial Institution number";

            public override bool IsValid(string line)
            {
                var value = line.Substring(3, 1);

                return LookupList.Contains(value);
            }

            //todo: figure out valid State Number (SN)
            protected override IList<string> LookupList { get; } = [
                "1", "2", "3", "4", "5", "6", "7", "8", "9"
            ];
        }
    }
}
