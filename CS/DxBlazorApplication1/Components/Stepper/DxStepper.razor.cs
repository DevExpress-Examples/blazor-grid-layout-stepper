using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;

namespace DxBlazorApplication1.Components.Stepper {
    /// <summary>
    /// DxStepper is based on DxGridLayout component.
    /// Nodes (steps) are located in even columns/rows.
    /// Connectors are located in odd columns/rows.
    /// Labels are located in even columns/rows below/next to nodes.
    /// </summary>
    public partial class DxStepper : ComponentBase, IDisposable {

        #region Fields
        private ObservableCollection<StepInfo> steps = [];
        private StepperDataMapping mappings = new();
        #endregion

        #region Properties
        private int LabelCount => steps.Count;
        private int ConnectorCount => steps.Count - 1;
        private int NodeCount => steps.Count;
        #endregion

        #region Parameters

        [Parameter]
        public IEnumerable? Data { get; set; }

        [Parameter]
        public RenderFragment? DataMappings { get; set; }

        [Parameter]
        public RenderFragment? Steps { get; set; }

        [Parameter]
        public int SelectedIndex { get; set; }

        [Parameter]
        public EventCallback<int> SelectedIndexChanged { get; set; }

        [Parameter]
        public Orientation Orientation { get; set; }

        #endregion

        #region Lifecycle Methods
        protected override void OnInitialized() {
            steps.CollectionChanged += UpdateStepRender;
            mappings.DataMappingsInitialized += InitializeStepCollection;
        }

        public void Dispose() {
            steps.CollectionChanged -= UpdateStepRender;
            mappings.DataMappingsInitialized -= InitializeStepCollection;
        }
        #endregion

        #region Event Handlers
        private async Task OnSelectedIndexChanged(int nodeIndex) {
            if(nodeIndex != SelectedIndex)
                await SelectedIndexChanged.InvokeAsync(nodeIndex);
        }
        #endregion

        #region Utility Methods
        private void UpdateStepRender(object? sender, NotifyCollectionChangedEventArgs e) {
            StateHasChanged();
        }
        private void InitializeStepCollection(object? source, EventArgs e) {
            if(Data is null) {
                throw new Exception("DxStepper is not bound");
            }

            foreach(object record in Data) {
                if(!HasMappingProperties(record)) {
                    throw new Exception("Data contains objects that do not meet mappings");
                }
                steps.Add(CreateStepInfoFromSourceObject(record));
            }

            StateHasChanged();
        }
        private StepInfo CreateStepInfoFromSourceObject(dynamic record) {
            if(record is null) {
                throw new Exception("Bound record is null");
            }
            var result = new StepInfo();
            result.Text = record.Text;
            result.Label = record.Label;
            result.IconCssClass = record.IconCssClass;
            return result;
        }
        private bool HasMappingProperties(object record) {
            if(mappings is null) {
                throw new Exception("Mappings are not specified");
            }
            if(String.IsNullOrEmpty(mappings.Text)) {
                throw new Exception("Text mapping is not specified");
            }
            if(String.IsNullOrEmpty(mappings.Label)) {
                throw new Exception("Label mapping is not specified");
            }
            if(String.IsNullOrEmpty(mappings.IconCssClass)) {
                throw new Exception("IconCssClass mapping is not specified");
            }
            var textProperty = record.GetType().GetProperty(mappings.Text);
            var labelProperty = record.GetType().GetProperty(mappings.Label);
            var iconCssClassProperty = record.GetType().GetProperty(mappings.IconCssClass);
            return textProperty is not null && labelProperty is not null && iconCssClassProperty is not null;
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
