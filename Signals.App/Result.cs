namespace Signals.App
{
    public class Result<TValue>
    {
        public TValue Value { get; set; }
        public List<ResultProblem> Problems { get; set; }

        public Result(TValue value)
        {
            Value = value;
        }

        public Result(params ResultProblem[] problems)
        {
            Problems = problems.ToList();
        }

        public static implicit operator Result<TValue>(TValue value)
        {
            return new Result<TValue>(value);
        }

        public static implicit operator Result<TValue>(ResultProblem problem)
        {
            return new Result<TValue>(problem);
        }
    }

    public class ResultProblem
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public ResultProblem(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
