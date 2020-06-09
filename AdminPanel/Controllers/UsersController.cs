using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdminPanel;
using AdminPanel.Models;
using AutoMapper;
using AdminPanel.ViewModels.Users;
using AutoMapper.QueryableExtensions;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection.PortableExecutable;
using System.DirectoryServices;
using System.Text;
using System.Security.Principal;
using AdminPanel.ViewModels.InfoBasesLists;
using Microsoft.AspNetCore.Authorization;

namespace AdminPanel.Controllers
{
    [Authorize(Policy = "OnlyAdmins")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Users
        public IActionResult Index()
        {
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _context.Users
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (viewModel == null)
            {
                return NotFound();
            }

            await PopulateViewBagData();

            return View(viewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (viewModel.InfoBasesListId == Guid.Empty)
                viewModel.InfoBasesListId = null;

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(viewModel);
                user.InfoBasesList = null;

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateViewBagData();

            return View(viewModel);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _context.Users
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(m => m.Id == id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool UserExists(string sid)
        {
            return _context.Users.Any(e => e.Sid == sid);
        }

        private async Task<User> GetUserAsync(string sid)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.Sid == sid);
        }

        public async Task<IActionResult> UpdateFromActiveDirectory()
        {
            var listToAdd = new List<User>();
            var listToUpdate = new List<User>();

            var entry = Domain.GetCurrentDomain().GetDirectoryEntry();

            var searcher = new DirectorySearcher(entry, "(&(objectCategory=person)(objectclass=user))");
            searcher.PropertiesToLoad.Add("name");
            searcher.PropertiesToLoad.Add("samAccountName");
            searcher.PropertiesToLoad.Add("objectSID");

            var result = searcher.FindAll();

            foreach (SearchResult searchResult in result)
            {
                var sid = GetPropertyValue(searchResult, "objectSID");

                if (!UserExists(sid))
                {
                    var user = new User
                    {
                        Name = GetPropertyValue(searchResult, "name"),
                        Sid = sid,
                        SamAccountName = GetPropertyValue(searchResult, "samAccountName")
                    };

                    listToAdd.Add(user);
                }
                else
                {
                    var existsUser = await GetUserAsync(sid);

                    var user = new User
                    {
                        Name = GetPropertyValue(searchResult, "name"),
                        Sid = sid,
                        SamAccountName = GetPropertyValue(searchResult, "samAccountName")
                    };

                    if (!existsUser.Equals(user))
                    {
                        existsUser.Name = user.Name;
                        existsUser.SamAccountName = user.SamAccountName;
                        existsUser.Sid = user.Sid;

                        listToUpdate.Add(existsUser);
                    }
                }
            }

            await _context.Users.AddRangeAsync(listToAdd);
            _context.Users.UpdateRange(listToUpdate);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private string GetPropertyValue(SearchResult result, string propertyName)
        {
            if (result.Properties.Contains(propertyName))
            {
                var value = result.Properties[propertyName]?[0];

                if (value.GetType() == typeof(byte[]))
                {
                    var si = new SecurityIdentifier((byte[])value, 0);

                    return si.Value;
                }
                else
                    return result.Properties[propertyName]?[0]?.ToString();
            }

            return "";
        }

        private async Task PopulateViewBagData()
        {
            var data = await _context.InfoBasesLists
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.AllInfoBasesLists = new SelectList(data, "Id", "Name");
        }

        public async Task<IActionResult> GetUsers(int pageIndex = 1, string term = "")
        {
            const int pageSize = 50;

            var source = _context.Users;

            var count = 0;

            if (string.IsNullOrEmpty(term))
                count = await source.CountAsync();
            else
                count = await source.Where(c => c.Name.Contains(term)).CountAsync();

            var pagesAmount = (int)Math.Ceiling((double)count / pageSize);

            var viewModel = new UserIndexViewModel
            {
                CurrentPage = pageIndex,
                PagesAmount = pagesAmount
            };

            if (string.IsNullOrEmpty(term))
            {
                viewModel.Items = await source
                    .OrderBy(c => c.Name)
                    .Skip((pageSize * (pageIndex - 1)))
                    .Take(pageSize)
                    .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            else
            {
                viewModel.Items = await source
                    .Where(c => c.Name.Contains(term))
                    .OrderBy(c => c.Name)
                    .Skip((pageSize * (pageIndex - 1)))
                    .Take(pageSize)
                    .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }

            return Json(viewModel);
        }
    }
}
