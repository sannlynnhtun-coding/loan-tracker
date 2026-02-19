using System.ComponentModel.DataAnnotations;

namespace LoanTracker.Shared.Models;

public class CustomerRequest
{
    public int CustomerId { get; set; }

    [Required]
    [Display(Name = "Full Name")]
    public string CustomerName { get; set; } = null!;

    [Required]
    [Display(Name = "NRC Number")]
    public string Nrc { get; set; } = null!;

    [Required]
    [Phone]
    [Display(Name = "Mobile Number")]
    public string MobileNo { get; set; } = null!;

    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }

    public DateTime? CreatedDate { get; set; }
}

public class CustomerResponse
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string Nrc { get; set; } = null!;
    public string MobileNo { get; set; } = null!;
    public string? Address { get; set; }
    public DateTime? CreatedDate { get; set; }
}
