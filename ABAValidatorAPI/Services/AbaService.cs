using ABAValidatorAPI.Services.Rules;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


                var task = Task.Factory.StartNew<object>(
                    state =>
                    {
                        Thread.Sleep(5000);

                        var data = ((string line, int lineNumber, RecordRules rules)) state!;
                        return AnalizeLine(data.line, data.lineNumber, data.rules);
                    }, 
                    (line, lineNumber, _ruleService));

                yield return task.Result;
            }
        }

        public static object AnalizeLine(string line, int lineNumber, RecordRules rulesService)
        {
            var errors = new List<string>();

            if (!CollectErrors(line, errors, rules: rulesService.LineLength).Any())
            {
                CollectErrors(line, errors, rules: rulesService.GetRules(recordType: line[0]));
            }

            var validationResult = new
            {
                LineNumber = lineNumber,
                LineContent = line,
                IsValid = errors.Count == 0,
                ErrorMessages = errors.ToArray()
            };

            return validationResult;
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
