namespace AiCompany.Exceptions;

public class WorkflowException : Exception
{
    public string WorkflowName { get; }
    public string? StepId { get; }

    public WorkflowException(string workflowName, string message)
        : base(message)
    {
        WorkflowName = workflowName;
    }

    public WorkflowException(string workflowName, string stepId, string message, Exception? inner = null)
        : base(message, inner)
    {
        WorkflowName = workflowName;
        StepId = stepId;
    }
}

public class AgentExecutionException : Exception
{
    public string AgentName { get; }
    public string TaskId { get; }

    public AgentExecutionException(string agentName, string taskId, string message, Exception? inner = null)
        : base(message, inner)
    {
        AgentName = agentName;
        TaskId = taskId;
    }
}

public class ToolExecutionException : Exception
{
    public string ToolName { get; }

    public ToolExecutionException(string toolName, string message, Exception? inner = null)
        : base(message, inner)
    {
        ToolName = toolName;
    }
}
