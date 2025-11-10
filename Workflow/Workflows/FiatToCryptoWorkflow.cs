using SharedContracts;
using Workflow.Workflows.Steps;
using WorkflowCore.Interface;

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

                // wait for event with form data message
                // Then implement step to send form data to SignalR
                .WaitFor("form-data-prepared", data => data.CorrelationId.ToString())
                .Output(data => data.PaymentData, step => (PaymentDataDTO)step.EventData)

                .Then<SendToSignalRProviderStep>()
                    .Input(step => step.Payload, data => data.PaymentData)

                .WaitFor("liqpay-response", data => data.CorrelationId.ToString())
                    .Name("WaitForLiqPayResponse")
                    .Output(data => data.Response, step => (FiatToCryptoResponseMessage)step.EventData);

        }
    }
}
