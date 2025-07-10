namespace Airplane.Best.Routes.Application.Dtos
{
    public sealed record Output<T> : Output where T : notnull
    {
        public T Data { get; init; }

        public Output(T data, bool isSuccess)
        {
            Data = data;
            IsSuccess = isSuccess;
        }

        public override Output<T> AddMessage(string message)
        {
            base.AddMessage(message);
            return this;
        }

        public override Output<T> AddErrorMessage(string errorMessage)
        {
            base.AddErrorMessage(errorMessage);
            return this;
        }

        public override Output<T> AddMessages(List<string> messages)
        {
            base.AddMessages(messages);
            return this;
        }

        public override Output<T> AddErrorMessages(List<string> errorMessages)
        {
            base.AddErrorMessages(errorMessages);
            return this;
        }
    }

    public record Output
    {
        public bool IsSuccess { get; init; }
        public List<string> Messages { get; init; } = new();
        public List<string> ErrorMessages { get; init; } = new();

        public Output(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public Output()
        {
        }
        public virtual Output AddMessage(string message)
        {
            Messages.Add(message);
            return this;
        }

        public virtual Output AddErrorMessage(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
            return this;
        }

        public virtual Output AddMessages(List<string> messages)
        {
            Messages.AddRange(messages);
            return this;
        }
        public virtual Output AddErrorMessages(List<string> errorMessages)
        {
            ErrorMessages.AddRange(errorMessages);
            return this;
        }
    }
}
