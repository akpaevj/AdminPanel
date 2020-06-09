using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WebCommonInfoBasesController : Controller
    {
        private readonly AppDbContext _context;

        public WebCommonInfoBasesController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpHead]
        public IActionResult Cap()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("CheckInfoBases")]
        public async Task<IActionResult> CheckInfoBases(string ClientId, string InfoBasesCheckCode)
        {
            if (!Guid.TryParse(ClientId, out Guid clientIdGuid) ||
                !Guid.TryParse(InfoBasesCheckCode, out Guid infoBasesCheckCodeGuid))
                return BadRequest();

            var infoBasesChanged = false;
            var url = "";

            if (clientIdGuid == Guid.Empty)
                infoBasesChanged = true;
            else
            {
                var user = await _context.Users
                    .Include(c => c.InfoBasesList)
                    .FirstOrDefaultAsync(c => c.Id == clientIdGuid);

                if (user != null)
                {
                    if (infoBasesCheckCodeGuid != user.InfoBasesList?.ListId)
                        infoBasesChanged = true;
                }
                else
                    infoBasesChanged = true;
            }

            return Json(new { root = new { infoBasesChanged, url } });
        }

        [HttpGet("GetInfoBases")]
        public async Task<IActionResult> GetInfoBases(string ClientId, string InfoBasesCheckCode)
        {
            if (!Guid.TryParse(ClientId, out Guid clientIdGuid) ||
                !Guid.TryParse(InfoBasesCheckCode, out Guid infoBasesCheckCodeGuid))
                return BadRequest();

            string infoBases = "";

            // Это первое обращение к сервису
            if (clientIdGuid == Guid.Empty)
            {
                if (HttpContext.User.Identity is WindowsIdentity windowsIdentity && windowsIdentity.IsAuthenticated)
                {
                    var sid = windowsIdentity.User.Value;

                    // Ищем пользователя по sid
                    var user = await GetUserBySid(sid);

                    GetInfoBases(user, ref clientIdGuid, ref infoBasesCheckCodeGuid, ref infoBases);
                }
                else // Пользователь не аутентифицирован, отправим путой список
                {
                    infoBasesCheckCodeGuid = Guid.NewGuid();

                    infoBases = "";
                }
            }
            else // Это не первое обращение к сервису
            {
                var user = await GetUserById(clientIdGuid);

                // Может быть так, что пользователь был пересоздан в базе данных сервиса, попробуем его найти по SID
                if (HttpContext.User.Identity is WindowsIdentity windowsIdentity && windowsIdentity.IsAuthenticated)
                {
                    var sid = windowsIdentity.User.Value;

                    // Ищем пользователя по sid
                    user = await GetUserBySid(sid);
                }
                
                GetInfoBases(user, ref clientIdGuid, ref infoBasesCheckCodeGuid, ref infoBases);
            }

            return Json(new { root = new { ClientId = clientIdGuid, InfoBasesCheckCode = infoBasesCheckCodeGuid, InfoBases = infoBases } });
        }

        private async Task<User> GetUserById(Guid id)
        {
            return await _context.Users
                .Include(c => c.InfoBasesList)
                .ThenInclude(c => c.InfoBaseInfoBasesLists)
                .ThenInclude(c => c.InfoBase)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        private async Task<User> GetUserBySid(string sid)
        {
            return await _context.Users
                .Include(c => c.InfoBasesList)
                .ThenInclude(c => c.InfoBaseInfoBasesLists)
                .ThenInclude(c => c.InfoBase)
                .FirstOrDefaultAsync(c => c.Sid == sid);
        }
        private void GetInfoBases(User user, ref Guid clientIdGuid, ref Guid infoBasesCheckCodeGuid, ref string infoBases)
        {
            // Пользователь найден
            if (user != null)
            {
                clientIdGuid = user.Id;

                // У пользователя установлен список информационных баз
                if (user.InfoBasesList != null)
                {
                    // Проверка в первый раз, либо изменилось содержимое списка баз
                    if (user.InfoBasesList.ListId != infoBasesCheckCodeGuid)
                        infoBasesCheckCodeGuid = user.InfoBasesList.ListId;

                    infoBases = user.InfoBasesList.GetIBasesContent();
                }
                else // Список баз мог быть очищен
                {
                    infoBasesCheckCodeGuid = Guid.NewGuid();

                    infoBases = "";
                }
            }
            else // пользователь не найден, отправим ему пустой список баз
            {
                infoBasesCheckCodeGuid = Guid.NewGuid();

                infoBases = "";
            }
        }
    }
}