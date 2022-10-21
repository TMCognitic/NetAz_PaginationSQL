using System.Reflection.Metadata;

namespace Tools
{
    public class Command
    {
        internal string Query { get; init; }
        internal bool IsStoredProcedure { get; init; }
        internal Dictionary<string, Parameter> Parameters { get; init; }

        public object? this[string parameterName]
        {
            get 
            {
                Parameter parameter = Parameters[parameterName];
                return parameter.Value;
            }
            internal set
            {
                Parameters[parameterName].Value = value;
            }
        }

        public Command(string query, bool isStoredProcedure = false)
        {
            Query = query;
            IsStoredProcedure = isStoredProcedure;
            Parameters = new Dictionary<string, Parameter>();
        }

        public void AddParameter(string parameterName, object? value)
        {
            AddParameter(parameterName, value, Direction.Input);
        }

        public void AddParameter(string parameterName, object? value, Direction direction)
        {
            Parameters.Add(parameterName, new Parameter(value ?? DBNull.Value, direction));
        }
    }
}