using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataWarehouseService
{
    public static class AuthorizationHelper
    {

        /// <returns></returns>
        public static ClientIdentity GetClientIdentity(HttpRequest request)
        {
            try
            {
                var payload = GetPayload(GetAuthorizationBearer(request));
                return new ClientIdentity(payload.UserIdentifier, GetOrganizationId(request), payload.FromKombit, payload.CvrIdentifier, payload.HasPrivilege, payload.Email);
            }
            catch
            {
                return null;
            }
        }

        internal static string GetAuthorizationBearer(HttpRequest request)
        {
            StringValues values = "";
            request.Headers.TryGetValue("Authorization", out values);
            if (values != "")
            {
                var elements = values[0].Split(' ');
                return elements != null && elements.Count() == 2 && elements.First().Equals("Bearer") ? elements[1] : null;
            }

            return null;
        }

        internal static int? GetOrganizationId(HttpRequest request)
        {
            var organizationId = int.MinValue;

            try
            {
                StringValues values = "";
                request.Headers.TryGetValue("Organization", out values);
                if (values != "" && int.TryParse(values.FirstOrDefault(), out organizationId))
                {
                    return organizationId;
                }
            }
            catch { }

            return null;
        }

        public static Payload GetPayload(string bearer)
        {
            Payload payload = null;

            try
            {
                // System.IdentityModel.Tokens.Jwt.
                var jwtHandler = new JwtSecurityTokenHandler();
                var readableToken = jwtHandler.CanReadToken(bearer);

                if (readableToken == true)
                {
                    var tokenS = jwtHandler.ReadToken(bearer) as JwtSecurityToken;

                    // Extract the payload of the JWT
                    List<Claim> claims = tokenS.Claims.ToList();
                    payload = new Payload();
                    //If a token has the following issuer, the token is created from the userflow B2C_1A_..._VIREGO for virego, by a user
                    if (claims.Exists(cl => cl.Type == "iss" && cl.Value == "https://marselisborgit.b2clogin.com/84bc7fba-b1da-48f9-aa45-a1542279f9e9/v2.0/"))
                    {
                        //used to authenticated local and kombit accounts. Rework with the same policies as used in access. note as of 27/04-21, CFA
                        if (claims.Count(cl => cl.Type.ToLower() == "extension_cvr") > 0)
                        {
                            payload.Sub = claims.First(c => c.Type == "serial").Value;
                            payload.CvrIdentifier = claims.FirstOrDefault(c => c.Type.ToLower() == "extension_cvr").Value;
                            payload.UserIdentifier = claims.First(c => c.Type == "serial").Value;
                            payload.FromKombit = true;
                            payload.HasPrivilege = true;
                            payload.Email = claims.FirstOrDefault(c => c.Type == "email").Value;
                            payload.EmailVerified = true;
                        }
                        else
                        {
                            payload.Sub = claims.FirstOrDefault(c => c.Type == "sub").Value;
                            payload.CvrIdentifier = "local";
                            payload.UserIdentifier = claims.FirstOrDefault(c => c.Type == "email").Value;
                            payload.FromKombit = false;
                            payload.HasPrivilege = false;
                            payload.Email = claims.FirstOrDefault(c => c.Type == "email").Value;
                            payload.EmailVerified = true;
                        }
                    }
                    //This is taken from master, as the app was running like this before the above changes for azure.
                    //This means that the call was from a virego application without a user.
                    else if (claims.Exists(cl => cl.Type == "iss" && cl.Value == "https://login.microsoftonline.com/84bc7fba-b1da-48f9-aa45-a1542279f9e9/v2.0"))
                    {
                        payload = new Payload();
                        payload.Sub = claims.FirstOrDefault(c => c.Type == "sub").Value;
                        payload.CvrIdentifier = "";
                        payload.UserIdentifier = claims.FirstOrDefault(c => c.Type == "azp").Value;
                        payload.FromKombit = false;
                        payload.HasPrivilege = false;
                        payload.Email = "";
                        payload.EmailVerified = false;
                    }
                    //probably unauthorized
                    else
                    {
                        throw new UnauthorizedAccessException("User did not have correct issuer or had no token at all.");
                    }
                }
            }
            catch (Exception e)
            {
            }

            return payload;
        }

    }
}
