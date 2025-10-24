using System.ComponentModel.DataAnnotations;

namespace DxBlazorApplication1.Models {
    public class BookingModel {
        public required ContactDetails ContactDetails { get; set; }
        public required ActivityDetails ActivityDetails { get; set; }
        public required PaymentDetails PaymentDetails { get; set; }
    }

    public class ContactDetails {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? Phone { get; set; }
    }

    public class ActivityDetails {
        [Required]
        public string? Language { get; set; }
    }

    public class PaymentDetails {
        [Required]
        public string? PaymentMethod { get; set; }
    }

    public class StepperModel {
        public required string Text { get; set; }
        public required string IconCssClass { get; set; }
        public required string Label { get; set; }
    }
}
