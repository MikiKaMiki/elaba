using ABAValidatorAPI.Services.Rules;
using System.Text.RegularExpressions;

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

                if(!CollectErrors(line, errors, rules: _ruleService.LineLength).Any())
                {
                    CollectErrors(line, errors, rules: _ruleService.GetRules(recordType: line[0]));
                }

                var validationResult = new
                {
                    LineNumber = lineNumber,
                    LineContent = line,
                    IsValid = errors.Count == 0,
                    ErrorMessages = errors.ToArray()
                };

                await Task.Delay(1000);

                yield return validationResult;
            }
        }


        private static List<string> CollectErrors(string line, List<string> errors, Dictionary<Regex, string> rules)
        {
            foreach (var rule in rules)
            {
                if (!rule.Key.IsMatch(line))
                {
                    errors.Add(rule.Value);
                }
            }

            return errors;
        }
    }
}
