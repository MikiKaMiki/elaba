namespace ABAValidatorAPI.Engine
{
    public class AbaRecordService
    {
        private readonly AbaValidationContext _context;

        private IRecordValidator _currentValidator;

        private IRecordValidator _descriptiveRecordValidator;

        private IRecordValidator _detailRecordValidator;

        private IRecordValidator _totalRecordValidator;

        private IRecordValidator _unknownRecordValidator;

        #region Constructors

        public AbaRecordService(AbaValidationContext context)
        {
            _context = context;

            _descriptiveRecordValidator = new DescriptiveRecordValidator();

            _detailRecordValidator = new DetailRecordValidator(_context);

            _totalRecordValidator = new TotalRecordValidator(_context);

            _unknownRecordValidator = new UnknownRecordValidator();

            _currentValidator = _descriptiveRecordValidator;
        }

        #endregion Constructors

        internal RecordValidationResult ValidateRecord(int lineNumber, string line)
        {
            SetCurrentValidator(recordType: line.First());

            RecordValidationResult result = _currentValidator.ValidateRecord(lineNumber, line);

            return result;
        }

        private void SetCurrentValidator(char recordType)
        {
            switch (recordType)
            {
                case '0':
                    _currentValidator = _descriptiveRecordValidator;
                    break;
                case '1':
                    _currentValidator = _detailRecordValidator;
                    break;
                case '7':
                    _currentValidator = _totalRecordValidator;
                    break;
                default:
                    _currentValidator = _unknownRecordValidator;
                    break;
            }
        }
    }
}
