using System.Text.RegularExpressions;
using static FastExpressionCompiler.ExpressionCompiler;

namespace ABAValidatorAPI.Engine.Rules
{
    /// <summary>
    /// Transaction Amount checks.
    /// </summary>
    public partial class DetailField6RuleSet : IFieldRuleSet
    {
        private IList<IFieldRule> _rules;
        private AbaValidationContext _aContext;


        public DetailField6RuleSet(AbaValidationContext context)
        {
            _aContext = context;

            _rules = [
                new DetailField6Rule1(), // todo: make sure the order of rule execution. add order. or make rules aggregate rules
                new DetailField6Rule2(_aContext, new DetailField6Rule3(_aContext))
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
                else
                {
                    if (rule is IFieldCompositRule)
                    {
                        foreach (var r in (rule as IFieldCompositRule)!.InteralRules)
                        {
                            result.Errors.Add(r.ErrorMessage);
                        }
                    }
                }
            }

            result.IsValid = result.Errors.Count == 0;

            return result;
        }

        internal partial class DetailField6Rule1 : RegexFieldRule
        {
            protected override Regex Regex => DetailChar21_30();

            public override string ErrorMessage 
                => "Char 21, Size 10, Only numeric valid. Must be greater than zero. Shown in cents without punctuations. Right justified, zero filled. Unsigned.";

            public override bool IsValid(string line)
            {
                return Regex.IsMatch(line);
            }

            [GeneratedRegex(@"^.{20}[0-9]{9}[1-9].*$")]
            private static partial Regex DetailChar21_30();
        }

        internal partial class DetailField6Rule2 : IFieldRule, IFieldCompositRule
        {
            public string ErrorMessage => "Cannot identify is it Credit or is it Debit transaction";

            public IList<IFieldRule> InteralRules => [];

            public bool IsValid(string line)
            {
                string stringValue = line.Substring(21, 10);

                var _rContext = _aContext.CurrentDetailRecordValidationContext;

                if (_rContext?.IsCredit == null)
                {
                    return false;
                }
                                
                return true;
            }

            /// <summary>
            /// ABA file context
            /// </summary>
            private AbaValidationContext _aContext;

            public DetailField6Rule2(AbaValidationContext aContext, IFieldRule dependentRule)
            {
                _aContext = aContext;
                InteralRules.Add(dependentRule);
            }
        }

        internal partial class DetailField6Rule3 : IFieldRule
        {
            public string ErrorMessage => "Error parsing Transction Amount";

            public bool IsValid(string line)
            {
                string stringValue = line.Substring(21, 10);

                if (int.TryParse(stringValue.TrimStart('0'), out int amount))
                {
                    var _rContext = _aContext.CurrentDetailRecordValidationContext;

                    // for sure it is not null
                    // because of check in dependant rule DetailField6Rule2
                    if (_rContext!.IsCredit!.Value)
                    {
                        _aContext.DetailRecordCreditTotal += amount;
                    }
                    else
                    {
                        if (_rContext.IsCredit != null && !_rContext.IsCredit.Value)
                        {
                            _aContext.DetailRecordDebitTotal += amount;
                        }
                    }

                    return true;
                }
                else
                {
                    // todo:
                    //_logger.Warning("Cannot parese value of the field DetailRecod.Position_21_30. Value: {value}; {file name/identifier} {record type} {record number}");
                    // theoretically we never should rize this point
                    // maybe throw an exception
                    // maybe write an error with additional info
                    // generraly the parse will succeed because it is backed by <ref="DetailField6Rule1">
                    return false;
                }
            }

            /// <summary>
            /// ABA file context
            /// </summary>
            private AbaValidationContext _aContext;

            public DetailField6Rule3(AbaValidationContext aContext)
            {
                _aContext = aContext;
            }
        }
    }
}
