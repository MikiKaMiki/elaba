using ABAValidatorAPI.Engine.Rules;

namespace ABAValidatorAPI.Engine
{
    public class TotalRecordValidator : RecordValidatorBase
    {
        protected override RecordType RecordType => RecordType.Total;

        private readonly AbaValidationContext _context;

        public TotalRecordValidator(AbaValidationContext context)
        {
            _context = context;

            _recordRule = new CommonRecordRule();

            _fieldRuleSet.AddRange(
                [
                    new TotalField1RuleSet(),

                    // todo: 2
                    // todo: 3
                    // todo: 4
                    // todo: 5
                    // todo: 6
                    // todo: 7
                    new TotalField8RuleSet(_context),
                    // todo: 9
                ]
            );
        }
    }
}
