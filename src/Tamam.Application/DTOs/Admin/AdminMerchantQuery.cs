using Tamam.Application.Common;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Admin;

public class AdminMerchantQuery : PagedQuery
{
    public MerchantStatus? Status { get; set; }
}
