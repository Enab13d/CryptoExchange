using MassTransit;
using SharedContracts;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows.FiatOnRampWorkflow.Steps
{
    public class SendToLiqPayProviderStep(ISendEndpointProvider sendEndpointProvider) : StepBodyAsync
    {

        private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider;

        public FiatOnRampRequested Payload { get; set; } = default!;

        public override async Task<WorkflowCore.Models.ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:deposit-requested"));
            await endpoint.Send(Payload);
            return WorkflowCore.Models.ExecutionResult.Next();
        }
    }
}
