using ABAValidatorAPI.Engine;
using ABAValidatorAPI.Models;
using ABAValidatorAPI.Services.Rules;

namespace ABAValidatorAPI.Services
{
    public class AbaService
    {

        private readonly RecordRules _ruleService;

        public AbaService(RecordRules ruleService)
        {
            _ruleService = ruleService;
        }

        [Obsolete]
        public async IAsyncEnumerable<object> ValidateAbaStreamAsync(StreamReader r)
        {
            var reader = r;
            int lineNumber = 0;
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;

                var errors = new List<string>();

                foreach (var rule in _ruleService.GetRules(recordType: line[0]))
                {
                    if (!rule.Key.IsMatch(line))
                    {
                        errors.Add(rule.Value);
                    }
                }

                var validationResult = new
                {
                    LineNumber = lineNumber,
                    LineContent = line,
                    IsValid = errors.Count == 0,
                    ErrorMessages = errors.ToArray()
                };

                //await Task.Delay(1000);

                yield return validationResult;
            }
        }

        [Obsolete]
        public async Task<AbaValidationResponse> ValidateAbaFileAsync(byte[] fileContent)
        {
            var result = new AbaValidationResponse();

            using var memoryStream = new MemoryStream(fileContent);
            using var reader = new StreamReader(memoryStream);

            int lineNumber = 0;
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var recordResult = new RecordValidationResponse();

                foreach (var rule in _ruleService.GetRules(recordType: line[0]))
                {
                    if (!rule.Key.IsMatch(line))
                    {
                        recordResult.Errors.Add(rule.Value);
                    }
                }

                recordResult.LineNumber = ++lineNumber;
                //recordResult.Record = line;
                recordResult.IsValid = recordResult.Errors.Count == 0;

                if (!recordResult.IsValid)
                {
                    result.RecordValidationResults.Add(recordResult);
                }
            }

            if (result.RecordValidationResults.Count == 0)
            {
                result.IsValid = true;
            }

            return result;
        }


        public async Task<AbaValidationResponse> ValidateFileAsync(byte[] fileContent)
        {
            var engine = new AbaValidatorEngine(fileContent);

            await engine.Validate();

            var result = engine.ProduceValidationResult();

            return result.ToResponse();
        }
    }
}
