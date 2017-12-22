using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MoneyMarket.Common;
using MoneyMarket.DataAccess.Models;

namespace MoneyMarket.OAuth.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId != null)
            {
                _publicClientId = publicClientId;
            }else
            {
                throw new ArgumentNullException("publicClientId");
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var channelTypeKeyValuePair = context.Request.Headers.FirstOrDefault(h => h.Key == RequestHeader.ChannelType);

            if (channelTypeKeyValuePair.Value == null)
            {
                context.SetError("invalid_grant", ErrorMessage.ChannelTypeNotFound);
                return;
            }

            string channel = channelTypeKeyValuePair.Value[0];

            if (channel != ChannelType.Android &&
                (channel != ChannelType.Ios) &&
                (channel != ChannelType.LandingPage) &&
                (channel != ChannelType.WebApp))
            {
                // return 401 (Unauthorized)
                context.SetError("invalid_grant", ErrorMessage.ChannelTypeIncorrect);
                return;
            }

            var apiKeyValuePair = context.Request.Headers.FirstOrDefault(h => h.Key == RequestHeader.ApiKey);

            if (apiKeyValuePair.Value == null)
            {
                context.SetError("invalid_grant", ErrorMessage.ApiKeyNotFound);
                return;
            }

            if (apiKeyValuePair.Value[0] != MoneyMarketConstant.ApiKeyHash)
            {
                context.SetError("invalid_grant", ErrorMessage.ApiKeyIncorrect);
                return;
            }

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user;

            if (context.UserName.Contains("@")) // login via email
            {
                user = await userManager.FindByEmailAsync(context.UserName);

                if (user == null)
                {
                    context.SetError("invalid_grant", ErrorMessage.UserNotFound);
                    return;
                }
                user = await userManager.FindAsync(user.UserName, context.Password);
            }
            else // login via username
                user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", ErrorMessage.UserNotFound);
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError("unverified_user", ErrorMessage.UserEmailNotConfirmed);
                return;
            }

            if (user.Status != Status.Active)
            {
                context.SetError("unactive_user", ErrorMessage.UserNotActive);
                return;
            }

            var roles = await userManager.GetRolesAsync(user.Id);
            var claims = await userManager.GetClaimsAsync(user.Id);

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);

            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

            //if (channel == ChannelType.WebApp)
            //{ // only allow author or admin from web admin panel entry
            //    bool webAdminEntryGranted = roles.Contains(ApplicationRole.Author) || roles.Contains(ApplicationRole.Admin);

            //    if (!webAdminEntryGranted)
            //    {
            //        context.SetError("invalid_grant", ErrorMessage.NotAuthorized);
            //        return;
            //    }
            //}

            AuthenticationProperties properties = CreateProperties(user, roles, claims);

            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user, IList<string> roles, IList<Claim> claims)
        {

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {
                    "user_name", user.UserName
                },
                {
                    "id",user.Id
                },
                {
                    "name_surname",user.NameSurname ?? ""
                },
                {
                    "email",user.Email
                } ,
                {
                    "phone_number",user.PhoneNumber ??""
                },
                //{
                //    "imageUrl",string.IsNullOrEmpty( user.ImageName) ?  "" : Statics.GetApiUrl() + ImagePath.User + user.ImageName
                //},
            };

            string roleArray = string.Empty;
            bool userHasRoles = false;

            for (int i = 0; i < roles.Count; i++)
            {
                userHasRoles = true;
                roleArray += roles[i];

                if (i < roles.Count - 1)
                {
                    roleArray += ",";
                }
            }

            if (userHasRoles)
            {
                data.Add("user_roles", roleArray);
            }

            foreach (var claim in claims)
            {
                data.Add(claim.Type, claim.Value);
            }

            return new AuthenticationProperties(data);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.OwinContext.Request.Method == "OPTIONS" && context.IsTokenEndpoint)
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "POST" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "accept", "authorization", "content-type", "Apikey", "ChannelType" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.StatusCode = 200;
                context.RequestCompleted();

                return Task.FromResult<object>(null);
            }

            return base.MatchEndpoint(context);
        }

    }
}