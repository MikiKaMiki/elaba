using ABAValidatorAPI.Engine.Rules;

namespace ABAValidatorAPI.Engine
{
    public class DescriptiveRecordValidator : RecordValidatorBase
    {
        protected override RecordType RecordType => RecordType.Descriptive;

        public DescriptiveRecordValidator() : base ()
        {


            _fieldRuleSet.AddRange(
                [
                    new DescriptiveField1RuleSet(),
                    new DescriptiveField2RuleSet(),
                    new DescriptiveField3RuleSet(),
                    new DescriptiveField4RuleSet(),
                    new DescriptiveField5RuleSet(),
                    new DescriptiveField6RuleSet(),
                    // todo: 7
                    // todo: 8
                    new DescriptiveField9RuleSet(),
                    // todo: 10
                ]
            );
        }
    }
}
