namespace library.Services
{
    public class ReservationResult
    {
        public bool Succeeded { get; private init; }
        public string Message { get; private init; } = "";
        public static ReservationResult Ok(string message) => new() { Succeeded = true, Message = message };
        public static ReservationResult Fail(string message) => new() { Message = message };
    }
}
