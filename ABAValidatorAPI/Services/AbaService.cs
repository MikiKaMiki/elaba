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
    }
}
