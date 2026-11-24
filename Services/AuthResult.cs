namespace library.Services
{
    public class AuthResult
    {
        public bool IsSuccess { get; private init; }
        public string? Error { get; private init; } = "";
        public string Username { get; private init; } = "";
        public int UserId { get; private init; }
        public int RoleId { get; private init; }

        public static AuthResult Success(int userid, int roleid, string username) {
            return new AuthResult
            {
                IsSuccess = true,
                UserId = userid,
                RoleId = roleid,
                Username = username
            };
        }

        public static AuthResult Fail(string error) => new AuthResult
        {
            IsSuccess = false,
            Error = error
        };
    }
}
