using ABAValidatorAPI.Engine.Rules;

namespace ABAValidatorAPI.Engine
{
    public class DetailRecordValidator : RecordValidatorBase
    {
        protected override RecordType RecordType => RecordType.Detail;


        /// <summary>
        /// ABA file context.
        /// </summary>
        AbaValidationContext _aContext;
        /// <summary>
        /// ABA record context.
        /// </summary>
        //DetailRecordValidationContext _rContext;

        public DetailRecordValidator(AbaValidationContext context)// : base(context)
        {
            _aContext = context;
            //_rContext = rContext;

            _fieldRuleSet.AddRange(
                [
                    new DetailField1RuleSet(),
                    new DetailField2RuleSet(),

                    // todo: 3
                    // todo: 4
                    new DetailField5RuleSet(_aContext),
                    new DetailField6RuleSet(_aContext),
                    // todo: 7
                    // todo: 8
                    // todo: 9
                    // todo: 10

                ]
            );
        }

        public override RecordValidationResult ValidateRecord(int lineNumber, string line)
        {
            _aContext.DetailRecordCount++;

            _aContext.InjectNewDetailRecordValidationContext();

            var rResult = NewResult();

            ValidateRecord(rResult, lineNumber, line);

            if (rResult.IsValid)
            {
                ValidateFields(rResult, lineNumber, line);
            }

            rResult.LineNumber = lineNumber;

            _aContext.EjectDetailRecordValidationContext();

            return rResult;
        }

        protected override void ValidateFields(RecordValidationResult recordResult, int lineNumber, string line)
        {
            base.ValidateFields(recordResult, lineNumber, line);
        }
    }
}
