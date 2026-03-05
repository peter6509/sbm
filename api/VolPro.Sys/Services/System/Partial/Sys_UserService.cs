using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolPro.Core;
using VolPro.Core.Configuration;
using VolPro.Core.Enums;
using VolPro.Core.Extensions;
using VolPro.Core.ManageUser;
using VolPro.Core.Services;
using VolPro.Core.UserManager;
using VolPro.Core.Utilities;
using VolPro.Entity.DomainModels;
using VolPro.Sys.IRepositories;

namespace VolPro.Sys.Services
{
    public partial class Sys_UserService
    {
        private Microsoft.AspNetCore.Http.HttpContext _context;
        private ISys_UserRepository _repository;

        [ActivatorUtilitiesConstructor]
        public Sys_UserService(IHttpContextAccessor httpContextAccessor, ISys_UserRepository repository)
            : base(repository)
        {
            _context = httpContextAccessor.HttpContext;
            _repository = repository;
        }
        WebResponseContent webResponse = new WebResponseContent();
        /// <summary> 
        /// WebApi登陆
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="verificationCode"></param> 
        /// <returns></returns>
        public async Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true)
        {
            WebResponseContent responseContent = new WebResponseContent();
            string msg = string.Empty;
            //   2020.06.12增加驗証碼
            IMemoryCache memoryCache = _context.GetService<IMemoryCache>();
            string cacheCode = (memoryCache.Get(loginInfo.UUID) ?? "").ToString();
            if (string.IsNullOrEmpty(cacheCode))
            {
                return responseContent.Error("驗証碼已失效".Translator());
            }
            if (cacheCode.ToLower() != loginInfo.VerificationCode.ToLower())
            {
                memoryCache.Remove(loginInfo.UUID);
                return responseContent.Error("驗証碼不正确".Translator());
            }
            try
            {
                Sys_User user = await _repository.FindAsIQueryable(x => x.UserName == loginInfo.UserName)
                    .FirstOrDefaultAsync();

                if (user == null || loginInfo.Password.Trim().EncryptDES(AppSetting.Secret.User) != (user.UserPwd ?? ""))
                    return webResponse.Error(ResponseType.LoginError);

                int expir = UserContext.MenuType == 1 ? 43200 : AppSetting.ExpMinutes;
                string token = JwtHelper.IssueJwt(new UserInfo()
                {
                    User_Id = user.User_Id,
                    UserName = user.UserName,
                    Role_Id = user.Role_Id ?? 0
                }, expir);
                user.Token = token;
                string accessToken = null;
                if (AppSetting.FileAuth)
                {
                    expir = expir + 30;
                    string dt = DateTime.Now.AddMinutes(expir).ToString("yyyy-MM-dd HH:mm");
                    accessToken = $"{user.User_Id}_{dt}".EncryptDES(AppSetting.Secret.User);
                    _context.GetService<Core.CacheManager.ICacheService>().Add(accessToken, dt, expir);
                }
                webResponse.Data = new { token, userName = user.UserTrueName, img = user.HeadImageUrl, accessToken };
                repository.Update(user, x => x.Token, true);
                UserContext.Current.LogOut(user.User_Id);

                loginInfo.Password = string.Empty;

                return webResponse.OK(ResponseType.LoginSuccess);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                //if (_context.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>().IsDevelopment())
                //{
                throw new Exception(ex.Message + ex.StackTrace);
                //}
                //return webResponse.Error(ResponseType.ServerError);
            }
            finally
            {
                memoryCache.Remove(loginInfo.UUID);
                Logger.Info(LoggerType.Login, loginInfo.Serialize(), webResponse.Message, msg);
            }
        }

        /// <summary>
        ///當token將要過期時，提前置換一個新的token
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> ReplaceToken()
        {
            string error = "";
            UserInfo userInfo = null;
            try
            {
                string requestToken = _context.Request.Headers[AppSetting.TokenHeaderName];
                requestToken = requestToken?.Replace("Bearer ", "");
                if (UserContext.Current.Token != requestToken) return webResponse.Error("Token已失效!");

                if (JwtHelper.IsExp(requestToken)) return webResponse.Error("Token已過期!");

                int userId = UserContext.Current.UserId;
                userInfo = await
                     repository.FindFirstAsync(x => x.User_Id == userId,
                     s => new UserInfo()
                     {
                         User_Id = userId,
                         UserName = s.UserName,
                         UserTrueName = s.UserTrueName,
                         Role_Id = s.Role_Id ?? 0,
                         RoleName = ""// s.RoleName
                     });

                if (userInfo == null) return webResponse.Error("未查到用户信息!");

                string token = JwtHelper.IssueJwt(userInfo);
                //移除當前缓存
                base.CacheContext.Remove(userId.GetUserIdKey());
                //只更新的token字段
                repository.Update(new Sys_User() { User_Id = userId, Token = token }, x => x.Token, true);
                webResponse.OK(null, token);
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.StackTrace + ex.Source;
                webResponse.Error("token替換出错了..");
            }
            finally
            {
                Logger.Info(LoggerType.ReplaceToeken, ($"用户Id:{userInfo?.User_Id},用户{userInfo?.UserTrueName}")
                    + (webResponse.Status ? "token替換成功" : "token替換失败"), null, error);
            }
            return webResponse;
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ModifyPwd(string oldPwd, string newPwd)
        {
            oldPwd = oldPwd?.Trim();
            newPwd = newPwd?.Trim();
            string message = "";
            WebResponseContent webResponse = new WebResponseContent();
            try
            {
                if (string.IsNullOrEmpty(oldPwd)) return webResponse.Error("舊密碼不能為空".Translator());
                if (string.IsNullOrEmpty(newPwd)) return webResponse.Error("新密碼不能為空".Translator());
                if (newPwd.Length < 6) return webResponse.Error("密碼不能少于6位".Translator());

                int userId = UserContext.Current.UserId;
                string userCurrentPwd = await base.repository.FindFirstAsync(x => x.User_Id == userId, s => s.UserPwd);

                string _oldPwd = oldPwd.EncryptDES(AppSetting.Secret.User);
                if (_oldPwd != userCurrentPwd) return webResponse.Error("舊密碼不正確".Translator());

                string _newPwd = newPwd.EncryptDES(AppSetting.Secret.User);
                if (userCurrentPwd == _newPwd) return webResponse.Error("新密碼不能與舊密碼相同".Translator());


                repository.Update(new Sys_User
                {
                    User_Id = userId,
                    UserPwd = _newPwd,
                    LastModifyPwdDate = DateTime.Now
                }, x => new { x.UserPwd, x.LastModifyPwdDate }, true);

                webResponse.OK("密碼修改成功".Translator());
            }
            catch (Exception ex)
            {
                message = ex.Message;
                webResponse.Error("服務器處理出现異常".Translator());
            }
            finally
            {
                if (message == "")
                {
                    Logger.OK(LoggerType.ApiModifyPwd, "密碼修改成功".Translator());
                }
                else
                {
                    Logger.Error(LoggerType.ApiModifyPwd, message);
                }
            }
            return webResponse;
        }
        /// <summary>
        /// 個人中心获取當前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentUserInfo()
        {
            var data = await base.repository
                .FindAsIQueryable(x => x.User_Id == UserContext.Current.UserId)
                .Select(s => new
                {
                    s.UserName,
                    s.UserTrueName,
                    //  s.Address,
                    s.PhoneNo,
                    //  s.Email,
                    s.Remark,
                    s.Gender,
                    // s.RoleName,
                    s.HeadImageUrl,
                    s.CreateDate
                })
                .FirstOrDefaultAsync();
            return webResponse.OK(null, data);
        }

        /// <summary>
        /// 設置固定排序方式及顯示用户過濾
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override PageGridData<Sys_User> GetPageData(PageDataOptions pageData)
        {
            QueryRelativeList = (List<SearchParameters> parameters) =>
            {
                string deptIdValues = null;
                string roleIdValues = null;
                foreach (var item in parameters)
                {
                    //部门查詢
                    if (item.Name == "DeptIds")
                    {
                        deptIdValues = item.Value;
                        item.Value = null;
                    }//角色子查詢
                    else if (item.Name == "RoleIds")
                    {
                        roleIdValues = item.Value;
                        //清空字段值后框架不會使用此字段
                        item.Value = null;
                    }
                }
                QueryRelativeExpression = (IQueryable<Sys_User> queryable) =>
                {
                    //部门查詢
                    if (!string.IsNullOrEmpty(deptIdValues))
                    {
                        var deptIds = deptIdValues.Split(",").Where(c => c != "").Select(s => (Guid)s.GetGuid());
                        var deptQuery = repository.DbContext.Set<Sys_UserDepartment>().Where(x => deptIds.Contains(x.DepartmentId) && x.Enable == 1);
                        queryable = queryable.Where(c => deptQuery.Any(d => d.UserId == c.User_Id));

                    }//角色子查詢
                    if (!string.IsNullOrEmpty(roleIdValues))
                    {
                        var roleIds = roleIdValues.Split(",").Where(c => c != "").Select(s => s.GetInt());
                        var deptQuery = repository.DbContext.Set<Sys_UserRole>().Where(x => roleIds.Contains(x.RoleId) && x.Enable == 1);
                        queryable = queryable.Where(c => deptQuery.Any(d => d.UserId == c.User_Id));
                    }
                    return queryable;
                };
            };
            var data = base.GetPageData(pageData);
            foreach (var item in data.rows)
            {
                item.Token = null;
            }
            return data;
        }

        /// <summary>
        /// 新建用户，根據實際情况自行處理
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Add(SaveModel saveModel)
        {
            WebResponseContent responseData = new WebResponseContent();
            base.AddOnExecute = (SaveModel userModel) =>
            {
                return responseData.OK();
            };


            ///生成6位數随機密碼
            string pwd = 6.GenerateRandomNumber();
            //在AddOnExecuting之前已經對提交的數據做過驗証是否為空
            base.AddOnExecuting = (Sys_User user, object obj) =>
            {
                user.UserName = user.UserName.Trim();
                if (repository.Exists(x => x.UserName == user.UserName))
                    return responseData.Error("用户名已經被注册".Translator());
                user.UserPwd = pwd.EncryptDES(AppSetting.Secret.User);
                //設置默認頭像
                return responseData.OK();
            };

            base.AddOnExecuted = (Sys_User user, object list) =>
            {
                var roleIds = user.RoleIds?.Split(",").Select(s => s.GetInt()).Where(x => x > 1).ToArray();
                SaveRole(roleIds, user.User_Id);
                var deptIds = user.DeptIds?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                SaveDepartment(deptIds, user.User_Id);
                //var postIds = user.PostId?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                //SavePost(postIds, user.User_Id);
                return responseData.OK("用户新建成功.帳號{$ts}密碼{$ts}".TranslatorFormat(user.UserName, pwd));
            };
            return base.Add(saveModel); ;
        }

        /// <summary>
        /// 删除用户攔截過濾
        /// 用户被删除后同時清空對應缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList"></param>
        /// <returns></returns>
        public override WebResponseContent Del(object[] keys, bool delList = false)
        {
            base.DelOnExecuting = (object[] ids) =>
            {
                int[] userIds = ids.Select(x => Convert.ToInt32(x)).ToArray();
                return new WebResponseContent().OK();
            };
            base.DelOnExecuted = (object[] userIds) =>
            {
                var objKeys = userIds.Select(x => x.GetInt().GetUserIdKey());
                base.CacheContext.RemoveAll(objKeys);
                return new WebResponseContent() { Status = true };
            };
            return base.Del(keys, delList);
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="userId"></param>
        public void SaveRole(int[] roleIds, int userId)
        {
            if (userId <= 0)
            {
                return;
            }
            if (roleIds == null)
            {
                roleIds = new int[] { };
            }

            //如果需要判断當前角色是否越權，再調用一下获取當前角色下的所有子角色判断即可

            var roles = repository.DbContext.Set<Sys_UserRole>().Where(x => x.UserId == userId)
              .Select(s => new { s.RoleId, s.Enable, s.Id })
              .ToList();
            //没有設置角色
            if (roleIds.Length == 0 && !roles.Exists(x => x.Enable == 1))
            {
                return;
            }

            UserInfo user = UserContext.Current.UserInfo;
            //新設置的角色 
            var add = roleIds.Where(x => !roles.Exists(r => r.RoleId == x)).Select(s => new Sys_UserRole()
            {
                Id = Guid.NewGuid(),
                RoleId = s,
                UserId = userId,
                Enable = 1,
                CreateDate = DateTime.Now,
                Creator = user.UserTrueName,
                CreateID = user.User_Id
            }).ToList();

            //删除的角色 
            var update = roles.Where(x => !roleIds.Contains(x.RoleId) && x.Enable == 1).Select(s => new Sys_UserRole()
            {
                Id = s.Id,
                Enable = 0,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList();

            //之前設置過的角色重新分配 
            update.AddRange(roles.Where(x => roleIds.Contains(x.RoleId) && x.Enable != 1).Select(s => new Sys_UserRole()
            {
                Id = s.Id,
                Enable = 1,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList());
            repository.AddRange(add);

            repository.UpdateRange(update, x => new { x.Enable, x.ModifyDate, x.Modifier, x.ModifyID });
            repository.SaveChanges();
        }

        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="deptIds"></param>
        /// <param name="userId"></param>
        public void SaveDepartment(Guid[] deptIds, int userId)
        {

            if (userId <= 0)
            {
                return;
            }
            if (deptIds == null)
            {
                deptIds = new Guid[] { };
            }

            //如果需要判断當前角色是否越權，再調用一下获取當前部门下的所有子角色判断即可

            var roles = repository.DbContext.Set<Sys_UserDepartment>().Where(x => x.UserId == userId)
              .Select(s => new { s.DepartmentId, s.Enable, s.Id })
              .ToList();
            //没有設置部门
            if (deptIds.Length == 0 && !roles.Exists(x => x.Enable == 1))
            {
                return;
            }

            UserInfo user = UserContext.Current.UserInfo;
            //新設置的部门
            var add = deptIds.Where(x => !roles.Exists(r => r.DepartmentId == x)).Select(s => new Sys_UserDepartment()
            {
                Id = Guid.NewGuid(),
                DepartmentId = s,
                UserId = userId,
                Enable = 1,
                CreateDate = DateTime.Now,
                Creator = user.UserTrueName,
                CreateID = user.User_Id
            }).ToList();

            //删除的部门
            var update = roles.Where(x => !deptIds.Contains(x.DepartmentId) && x.Enable == 1).Select(s => new Sys_UserDepartment()
            {
                Id = s.Id,
                Enable = 0,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList();

            //之前設置過的部门重新分配 
            update.AddRange(roles.Where(x => deptIds.Contains(x.DepartmentId) && x.Enable != 1).Select(s => new Sys_UserDepartment()
            {
                Id = s.Id,
                Enable = 1,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList());
            repository.AddRange(add);

            repository.UpdateRange(update, x => new { x.Enable, x.ModifyDate, x.Modifier, x.ModifyID });
            repository.SaveChanges();
        }

        /// <summary>
        /// 保存岗位
        /// </summary>
        /// <param name="postIds"></param>
        /// <param name="userId"></param>
        public void SavePost(Guid[] postIds, int userId)
        {

            if (userId <= 0)
            {
                return;
            }
            if (postIds == null)
            {
                postIds = new Guid[] { };
            }

            var roles = repository.DbContext.Set<Sys_UserPost>().Where(x => x.UserId == userId)
              .Select(s => new { s.PostId, s.Enable, s.Id })
              .ToList();
            //没有設置部门
            if (postIds.Length == 0 && !roles.Exists(x => x.Enable == 1))
            {
                return;
            }

            UserInfo user = UserContext.Current.UserInfo;
            //新設置的部门
            var add = postIds.Where(x => !roles.Exists(r => r.PostId == x)).Select(s => new Sys_UserPost()
            {
                Id = Guid.NewGuid(),
                PostId = s,
                UserId = userId,
                Enable = 1,
                CreateDate = DateTime.Now,
                Creator = user.UserTrueName,
                CreateID = user.User_Id
            }).ToList();

            //删除的部门
            var update = roles.Where(x => !postIds.Contains(x.PostId) && x.Enable == 1).Select(s => new Sys_UserPost()
            {
                Id = s.Id,
                Enable = 0,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList();

            //之前設置過的部门重新分配 
            update.AddRange(roles.Where(x => postIds.Contains(x.PostId) && x.Enable != 1).Select(s => new Sys_UserPost()
            {
                Id = s.Id,
                Enable = 1,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList());
            repository.AddRange(add);

            repository.UpdateRange(update, x => new { x.Enable, x.ModifyDate, x.Modifier, x.ModifyID });
            repository.SaveChanges();
        }

        /// <summary>
        /// 修改用户攔截過濾
        /// 
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Update(SaveModel saveModel)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            base.UpdateOnExecuting = (Sys_User user, object obj1, object obj2, List<object> list) =>
            {
                var _user = repository.Find(x => x.User_Id == user.User_Id,
                    s => new { s.UserName, s.UserPwd })
                    .FirstOrDefault();
                user.UserName = _user.UserName;
                //Sys_User實體的UserPwd用户密碼字段的屬性不是编輯，此處不會修改密碼。但防止代碼生成器將密碼字段的修改成了可编輯造成密碼被修改
                user.UserPwd = _user.UserPwd;
                return webResponse.OK();
            };
            //用户信息被修改后，將用户的缓存信息清除
            base.UpdateOnExecuted = (Sys_User user, object obj1, object obj2, List<object> List) =>
            {
                base.CacheContext.Remove(user.User_Id.GetUserIdKey());
                var roleIds = user.RoleIds?.Split(",").Select(s => s.GetInt()).Where(x => x > 1).ToArray();
                SaveRole(roleIds, user.User_Id);
                var deptIds = user.DeptIds?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                SaveDepartment(deptIds, user.User_Id);
                //var postIds = user.PostId?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                //SavePost(postIds, user.User_Id);
                return new WebResponseContent(true);
            };
            return base.Update(saveModel);
        }

        /// <summary>
        /// 导出處理
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override WebResponseContent Export(PageDataOptions pageData)
        {
            //限定只能导出當前角色能看到的所有用户(2023.10.17已移至TenancyExpression處理)
            //QueryRelativeExpression = (IQueryable<Sys_User> queryable) =>
            //{
            //    if (UserContext.Current.IsSuperAdmin) return queryable;
            //    List<int> roleIds = Sys_RoleService
            //     .Instance
            //     .GetAllChildrenRoleId(UserContext.Current.RoleIds);
            //    return queryable.Where(x => roleIds.Contains(x.Role_Id ?? 0) || x.User_Id == UserContext.Current.UserId);
            //};

            base.ExportOnExecuting = (List<Sys_User> list, List<string> ignoreColumn) =>
            {
                if (!ignoreColumn.Contains("Role_Id"))
                {
                    ignoreColumn.Add("Role_Id");
                }
                if (!ignoreColumn.Contains("RoleName"))
                {
                    ignoreColumn.Remove("RoleName");
                }
                WebResponseContent responseData = new WebResponseContent(true);
                return responseData;
            };
            return base.Export(pageData);
        }
    }
}

