namespace Core.Objects.Orders;

public enum OrderWorkflowState
{
    Unspecified = 0,
    Created = 1,
    ConfirmedBySeller = 2,
    ConfirmedByBuyer = 3,
    Cancelled = 4
}