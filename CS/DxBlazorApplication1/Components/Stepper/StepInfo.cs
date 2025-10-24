using Microsoft.AspNetCore.Components;

namespace DxBlazorApplication1.Components.Stepper {
    public class StepInfo {
        public string? Text { get; set; }
        public string? IconCssClass { get; set; }
        public string? Label { get; set; }
        public RenderFragment? ChildContent { get; set; }
    }
}
