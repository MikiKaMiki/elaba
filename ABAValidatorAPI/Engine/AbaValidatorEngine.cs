namespace ABAValidatorAPI.Engine
{
    public class AbaValidatorEngine
    {
        private readonly byte[] _fileContent;

        AbaValidationContext _context = new AbaValidationContext();

        private FileValidationResult _result { get; set; }

        private AbaRecordService _recordService;

 
        public AbaValidatorEngine(byte[] fileContent)
        {
            _recordService = new AbaRecordService(_context);

            _result = new FileValidationResult();

            _fileContent = fileContent;
        }

        public async Task Validate()
        {
            using var memoryStream = new MemoryStream(_fileContent);
            using var reader = new StreamReader(memoryStream);

            int lineNumber = 0;
            string? line;


            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;

                var record = _recordService.ValidateRecord(lineNumber, line);

                if (!record.IsValid)
                {
                    _result.IsValid = false;
                    _result.RecordValidationResults.Add(record);
                }
            }

            //if (_result.RecordValidationResults.Count == 0 &&
            //    _result.RecordValidationResults.All(r => r.FieldValidationResults.Count == 0))
            //{
            //    _result.IsValid = true;
            //}
        }

        public FileValidationResult ProduceValidationResult()
        {
            return _result!;
        }
    }
}
