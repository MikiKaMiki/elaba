using ABAValidatorAPI.Controllers;
using ABAValidatorAPI.Services.Rules;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ABAValidatorAPI.Services
{
    public class AbaService
    {
        private readonly ILogger<AbaService> _logger;

        private readonly RecordRules _ruleService;

        public AbaService(ILogger<AbaService> logger, RecordRules ruleService)
        {
            _logger = logger;
            _ruleService = ruleService;
        }

        public async IAsyncEnumerable<object> ValidateAbaStreamAsync(StreamReader r)
        {
            var reader = r;
            int lineNumber = 0;
            string? line;

            var @true = true;

            var countdown = new CountdownEvent(1);

            ConcurrentQueue<object> responsesQueue = new();

            while ((line = await reader.ReadLineAsync()) != null)
            {
                _logger.LogInformation(
                    "=== inqueue line {line}",
                    line);

                lineNumber++;

                List<(int lineNumber, string line)> queue = new();
                queue.Add((lineNumber, line));

                var i =1;

                while ((line = await reader.ReadLineAsync()) != null && i > 0)
                {
                    _logger.LogInformation(
                    "=== inqueue line {line}",
                    line);

                    i--;

                    queue.Add((++lineNumber, line));
                }


                var task = Task.Factory.StartNew(
                    state =>
                    {

                        countdown.AddCount(1);
                        //Thread.Sleep(5000);

                        var data = ((Queue<(int lineNumber, string line)> queue, ConcurrentQueue<object> responsesQueue, RecordRules rules)) state!;
                        var l = AnalizeLines(queue, data.rules);

                        foreach (var rr in l)
                        {
                            responsesQueue.Enqueue(rr);
                        }

                        countdown.Signal();
                    },

                    (queue, responsesQueue, _ruleService));
            }

            countdown.Signal();

            var setFalseTask = Task.Factory.StartNew((t) =>
            {
                countdown.Wait();

                @true = false;

            }, @true);

            while (responsesQueue.TryDequeue(out var res) || @true)
            {
                Thread.Sleep(100);

                if (res != null)
                {
                    yield return res;
                }
            }
        }


        public static List<object> AnalizeLines(
            List<(int lineNumber, string line)> queue, RecordRules rulesService)
        {

            var lines = new List<object>();

            foreach (var r in queue)
            {
                lines.Add(AnalizeLine(r.line, r.lineNumber, rulesService));
            }
           
            return lines;
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
