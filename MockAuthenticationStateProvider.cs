using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentServiceApp
{
    public class MockAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private ClaimsPrincipal _user;

        public MockAuthenticationStateProvider()
        {
            _user = _anonymous;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_user));
        }

        public void SignIn(string userName)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "User")
            }, "mock");
            _user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void SignOut()
        {
            _user = _anonymous;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
