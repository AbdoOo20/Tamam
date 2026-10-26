using Tamam.Application.Common;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.DataRequests;

public class DataRequestQuery : PagedQuery
{
    public DataRequestStatus? Status { get; set; }

    public DataRequestType? Type { get; set; }
}
