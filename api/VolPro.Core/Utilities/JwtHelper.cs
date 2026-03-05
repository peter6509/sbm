using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VolPro.Core.Configuration;
using VolPro.Core.Extensions;
using VolPro.Entity.DomainModels;

namespace VolPro.Core.Utilities
{
    public class JwtHelper
    {

        /// <summary>
        /// 生成JWT
        /// </summary>
        /// <param name="serInfo"></param>
        /// <returns></returns>
        public static string IssueJwt(UserInfo userInfo,int? expir=null)
        {
            string exp = $"{new DateTimeOffset(DateTime.Now.AddMinutes(expir??(ManageUser.UserContext.MenuType == 1? 43200: AppSetting.ExpMinutes))).ToUnixTimeSeconds()}";
            var claims = new List<Claim>
                {
                //new Claim(ClaimTypes.Name,userInfo.UserName ),
                //new Claim(ClaimTypes.Role,userInfo.Role_Id ),
                new Claim(JwtRegisteredClaimNames.Jti,userInfo.User_Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //JWT過期時间
                //驗証是否過期 从User讀取過期 時间，再將時间戳转換成日期，如果時间在半個小時内即將過期，通知前台刷新JWT
                //int val= HttpContext.User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Exp).FirstOrDefault().Value;
                //new DateTime(621355968000000000 + (long)val* (long)10000000, DateTimeKind.Utc).ToLocalTime()
                //默認設置jwt過期時间120分鐘
                new Claim (JwtRegisteredClaimNames.Exp,exp),
                new Claim(JwtRegisteredClaimNames.Iss,AppSetting.Secret.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud,AppSetting.Secret.Audience),
               };

            //秘鑰16位
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.Secret.JWT));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: AppSetting.Secret.Issuer, claims: claims, signingCredentials: creds);
            string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return jwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static UserInfo SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            UserInfo userInfo = new UserInfo
            {
                User_Id = Convert.ToInt32(jwtToken.Id),
                Role_Id = (jwtToken.Payload[ClaimTypes.Role] ?? 0).GetInt(),
                UserName = jwtToken.Payload[ClaimTypes.Name]?.ToString()
            };
            return userInfo;
        }
        /// <summary>
        /// 获取過期時间
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static DateTime GetExp(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            DateTime expDate = (jwtToken.Payload[JwtRegisteredClaimNames.Exp] ?? 0).GetInt().GetTimeSpmpToDate();
            return expDate;
        }
        public static bool IsExp(string jwtStr)
        {
            return GetExp(jwtStr) < DateTime.Now;
        }

        public static int GetUserId(string jwtStr)
        {
            try
            {
                return new JwtSecurityTokenHandler().ReadJwtToken(jwtStr).Id.GetInt();
            }
            catch
            {
                return 0;
            }
        }
    }


}
