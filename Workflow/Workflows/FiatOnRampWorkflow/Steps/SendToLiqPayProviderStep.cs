using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows.FiatOnRampWorkflow.Steps
{
    public class SendToLiqPayProviderStep : StepBodyAsync
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public SendToLiqPayProviderStep(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public FiatOnRampMessage Payload { get; set; } = default!;

        public override async Task<WorkflowCore.Models.ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            await _publishEndpoint.Publish(Payload);
            return WorkflowCore.Models.ExecutionResult.Next();
        }
    }
}
