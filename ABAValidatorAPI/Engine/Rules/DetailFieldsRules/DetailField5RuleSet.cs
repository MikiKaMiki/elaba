using System.Text.RegularExpressions;

namespace ABAValidatorAPI.Engine.Rules
{
    public partial class DetailField5RuleSet : FieldRuleSetBase
    {
        protected override short FieldNumber => 5;

        public AbaValidationContext _aContext { get; }

        public DetailField5RuleSet(AbaValidationContext aContext)
        {
            _aContext = aContext;

            _rules.Add(new DetailField5Rule1());
            _rules.Add(new DetailField5Rule2(_aContext));
        }

        internal partial class DetailField5Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DetailChar19_20();

            public override string ErrorMessage => "Char 19-20; Size 2; Must be numeric.";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.{18}[0-9]{2}.*$")]
            private static partial Regex DetailChar19_20();
        }

        internal partial class DetailField5Rule2 : LookupFieldRule<(string code, bool isCredit)?>
        {
            public override string ErrorMessage => "Char 19-20; Size 2 must equal valid Transaction Code";

            public override bool IsValid(string line)
            {
                var value = line.Substring(19, 2);

                var transactionCode = LookupList.FirstOrDefault(i => i!.Value.code == value);

                if (transactionCode != null)
                {
                    var _rContext = _aContext.CurrentDetailRecordValidationContext;

                    _rContext!.IsCredit = transactionCode.Value.isCredit;
                }
                else
                {
                    return false;
                }

                return transactionCode.HasValue;
            }

            //todo: figure out which codes are Credit and which are Debits
            protected override IList<(string code, bool isCredit)?> LookupList { get; } = [
                ("13", false),
                ("50", true), // todo: is it credit?
                ("51", true), // todo: is it credit?
                ("52", true), // todo: is it credit?
                ("53", true), // todo: is it credit?
                ("54", true), // todo: is it credit?
                ("55", true), // todo: is it credit?
                ("56", true), // todo: is it credit?
                ("57", true)  // todo: is it credit?
            ];

            /// <summary>
            /// ABA File Context.
            /// </summary>
            public AbaValidationContext _aContext { get; }


            public DetailField5Rule2(AbaValidationContext aContext)
            {
                _aContext = aContext;
            }
        }
    }
}
