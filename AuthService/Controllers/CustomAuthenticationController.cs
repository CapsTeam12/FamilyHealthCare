using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using AuthService.ViewModel.CustomAuthentication;
using Data.Entities;
using IdentityServer4.Stores;
using System.Linq;

namespace AuthService.Controllers
{
    public class CustomAuthenticationController : Controller
    {
        private IIdentityServerInteractionService _interaction;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEventService _events;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public CustomAuthenticationController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider)
        {
            _interaction = interaction;
            _events = events;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
        }

        [HttpGet]
        public async Task<IActionResult> LoginAsync(string returnUrl)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginAuthModelVM loginVm)
        {
            var context = await _interaction.GetAuthorizationContextAsync(loginVm.ReturnUrl);

            var result = await _signInManager.PasswordSignInAsync(loginVm.Username, loginVm.Password, false, false);

            if (!result.Succeeded)
            {
                ViewBag.Error = "Invalid Username or Password";
                return View("Login", loginVm);
            }

            ViewBag.Error = null;

            return Redirect(loginVm.ReturnUrl);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterAuthModelVM { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAuthModelVM RegisterVm)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", RegisterVm);
            }

            var user = new User();
            user.UserName = RegisterVm.Username;
            user.Email = RegisterVm.Username;

            var result = await _userManager.CreateAsync(user, RegisterVm.Password);

            if (!result.Succeeded)
            {
                return View("Register", RegisterVm);
            }

            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, false);

            return Redirect(RegisterVm.ReturnUrl);
        }

        [HttpGet]
        public IActionResult ChangePassword(string returnUrl)
        {
            return View(new ChangePasswordAuthModelVM { ReturnUrl = returnUrl });
        }

        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(ChangePasswordAuthModelVM changepwdVM)
        //{
        //    var context = await _interaction.GetAuthorizationContextAsync(changepwdVM.ReturnUrl);
        //    var user = _userManager.GetUserIdAsync(changepwdVM.Id);

        //    if (changepwdVM.CurrentPassword != user.Password)
        //    {
        //        ViewBag.Error = "Enter a valid password please try again";
        //        return View("ChangePassword", changepwdVM);
        //    }

        //    if (changepwdVM.NewPassword == changepwdVM.CurrentPassword)
        //    {
        //        ViewBag.Error = "Password must differ from old password";
        //        return View("ChangePassword", changepwdVM);
        //    }

        //    if (changepwdVM.NewPassword == changepwdVM.NewPasswordConfirm)
        //    {
        //        ViewBag.Error = "Passwords do not match";
        //        return View("ChangePassword", changepwdVM);
        //    }

        //    ViewBag.Error = null;

        //    return Redirect(changepwdVM.ReturnUrl);
        //}
        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }


        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}
