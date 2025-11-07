namespace blazor_stepper.Components.Stepper {
    public class StepperDataMappings {
        public string Text { get; set; }
        public string IconCssClass { get; set; }
        public string Label { get; set; }

        public StepperDataMappings(string text, string label, string iconCssClass) {
            Text = text;
            IconCssClass = iconCssClass;
            Label = label;
        }
    }
}
