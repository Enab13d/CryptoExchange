using SharedContracts;
using Workflow.Workflows.FiatOnRampWorkflow.Steps;
using WorkflowCore.Interface;

namespace Workflow.Workflows.FiatOnRampWorkflow
{
    public class FiatOnRampWorkflow : IWorkflow<FiatOnRampMessage>
    {
        public string Id => "FiatOnRampWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<FiatOnRampMessage> builder)
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
