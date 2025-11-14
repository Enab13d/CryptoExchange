using SharedContracts;
using Workflow.Workflows.Steps;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Workflows
{
    public class FiatToCryptoWorkflow : IWorkflow<FiatToCryptoMessage>
    {
        public string Id => "FiatToCryptoWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<FiatToCryptoMessage> builder)
        {
            builder
                .StartWith<SendToLiqPayProviderStep>()
                    .Input(step => step.Payload, data => data)

                .WaitFor("form-data-prepared", data => data.CorrelationId.ToString())
                .Output(data => data.PaymentData, step => (PaymentDataDTO)step.EventData)
                .WaitFor("websocket-connection-established", data => data.PaymentId.ToString())
                .Output(data => data.WebsocketConnectionMessage, step => (WebsocketConnectionMessage)step.EventData)
                .WaitFor("payment-data-requested", data => data.PaymentId.ToString())
                .Then<SendToSignalRProviderStep>()
                    .Input(step => step.Input, data => data)

                .EndWorkflow();

        }
    }
}
