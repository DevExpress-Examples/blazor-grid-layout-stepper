using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection.Metadata.Ecma335;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;

namespace blazor_stepper.Components.Stepper {
    /// <summary>
    /// DxStepper is based on DxGridLayout component.
    /// Nodes (steps) are located in even columns/rows.
    /// Connectors are located in odd columns/rows.
    /// Labels are located in even columns/rows below/next to nodes.
    /// </summary>
    public partial class DxStepper : ComponentBase {
        #region Fields
        private List<DxStep> steps = [];
        #endregion

        #region Properties
        private int LabelCount => steps.Count;
        private int ConnectorCount => steps.Count - 1;
        private int NodeCount => steps.Count;
        #endregion

        #region Parameters
        [Parameter]
        public RenderFragment? Steps { get; set; }

        [Parameter]
        public int SelectedIndex { get; set; }

        [Parameter]
        public EventCallback<int> SelectedIndexChanged { get; set; }

        [Parameter]
        public Orientation Orientation { get; set; }
        #endregion

        #region Event Handlers
        private async Task OnSelectedIndexChanged(int nodeIndex) {
            if(nodeIndex != SelectedIndex)
                await SelectedIndexChanged.InvokeAsync(nodeIndex);
        }
        #endregion

        #region Utility Methods
        public void AddStep(DxStep step) {
            steps.Add(step);
            StateHasChanged();
        }

        public void RemoveStep(DxStep step) {
            steps.Remove(step);
            StateHasChanged();
        }

        private string? GetStepText(int nodeIndex) {
            if(nodeIndex < 0 || nodeIndex >= steps.Count)
                throw new ArgumentOutOfRangeException(nameof(nodeIndex), "Invalid node index");
            if(steps[nodeIndex] is DxStep step
                && String.IsNullOrEmpty(step.IconCssClass)) {
                return step.Text;
            }
            return string.Empty;
        }

        private string GetRootCssClasses() {
            var classes = "stepper-html-root";
            if(Orientation == Orientation.Vertical) {
                classes += " stepper-html-root-vertical";
            }
            else {
                classes += " stepper-html-root-horizontal";
            }
            return classes;
        }

        private string GetStepCssClasses(int nodeIndex) {
            var classes = "st-node";
            if(!StepCompleted(nodeIndex)) {
                classes += " st-node-incomplete";
            }
            if(StepSelected(nodeIndex)) {
                classes += " st-node-selected";
            }
            return classes;
        }

        private string GetConnectorContainerCssClasses() {
            return Orientation == Orientation.Horizontal ? "st-connector-container" : "st-connector-container-vertical";
        }

        private string GetConnectorCssClasses(int nextNodeIndex) {
            string classes = Orientation == Orientation.Horizontal ? "st-connector" : "st-connector-vertical";
            if(!StepCompleted(nextNodeIndex)) {
                classes += " st-connector-incomplete";
            }
            return classes;
        }

        private bool StepCompleted(int nodeIndex) => SelectedIndex >= nodeIndex;

        private bool StepSelected(int nodeIndex) => SelectedIndex == nodeIndex;

        private int GetNodeColumnIndex(int nodeIndex) => Orientation == Orientation.Horizontal ? 2 * nodeIndex : 0;

        private int GetNodeRowIndex(int nodeIndex) => Orientation == Orientation.Horizontal ? 0 : 2 * nodeIndex;

        private int GetConnectorColumnIndex(int nodeIndex) => Orientation == Orientation.Horizontal ? 2 * nodeIndex + 1 : 0;

        private int GetConnectorRowIndex(int nodeIndex) => Orientation == Orientation.Horizontal ? 0 : 2 * nodeIndex + 1;

        private int GetLabelColumnIndex(int nodeIndex) => Orientation == Orientation.Horizontal ? 2 * nodeIndex : 1;

        private int GetLabelRowIndex(int nodeIndex) => Orientation == Orientation.Horizontal ? 1 : 2 * nodeIndex;
        #endregion
    }
}
