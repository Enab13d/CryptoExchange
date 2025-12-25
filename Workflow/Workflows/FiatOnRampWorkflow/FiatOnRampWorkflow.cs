using SharedContracts;
using Workflow.Workflows.FiatOnRampWorkflow.Steps;
using WorkflowCore.Interface;

namespace Workflow.Workflows.FiatOnRampWorkflow
{
    public class FiatOnRampWorkflow : IWorkflow<FiatOnRampRequested>
    {
        public string Id => "FiatOnRampWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<FiatOnRampRequested> builder)
        {
            builder
                .StartWith<SendToLiqPayProviderStep>()
                    //this step sends data to liqpayproviderservice to prepare data for payment form  
                    .Input(step => step.Payload, data => data)

                .WaitFor("form-data-prepared", data => data.CorrelationId.ToString())
                .EndWorkflow();

        }
    }
}
