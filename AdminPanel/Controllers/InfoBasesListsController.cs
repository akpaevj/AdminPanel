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
using AdminPanel.ViewModels.InfoBasesLists;
using AutoMapper.QueryableExtensions;
using AdminPanel.ViewModels.InfoBases;
using Microsoft.AspNetCore.Authorization;
using AdminPanel.ViewModels.Users;
using AutoMapper.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(Policy = "Admins")]
    public class InfoBasesListsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InfoBasesListsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: InfoBasesLists
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 100;

            var source = _context.InfoBasesLists;
            var count = await source.CountAsync();
            var pagesAmount = (int)Math.Ceiling((double)count / pageSize);

            var viewModel = new InfoBasesListIndexViewModel
            {
                Items = await source
                    .OrderBy(c => c.Name)
                    .Skip((pageSize * (page - 1)))
                    .Take(pageSize)
                    .ProjectTo<InfoBasesListViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(),
                CurrentPage = page,
                PagesAmount = pagesAmount
            };

            return View(viewModel);
        }

        // GET: InfoBasesLists/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new InfoBasesListViewModel();

            await PopulateViewBagData(viewModel.Id);

            return View(viewModel);
        }

        // POST: InfoBasesLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InfoBasesListViewModel viewModel, Guid[] selectedInfoBases, Guid[] selectedUsers)
        {
            if (ModelState.IsValid)
            {
                var infoBasesList = _mapper.Map<InfoBasesList>(viewModel);

                infoBasesList.Id = Guid.NewGuid();
                infoBasesList.ListId = Guid.NewGuid();

                await UpdateInfoBasesAsync(infoBasesList, selectedInfoBases);
                await UpdateUsersAsync(infoBasesList, selectedUsers);

                _context.Add(infoBasesList);
                
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateViewBagData(viewModel.Id);

            return View(viewModel);
        }

        // GET: InfoBasesLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _context.InfoBasesLists.ProjectTo<InfoBasesListViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(c => c.Id == id);

            if (viewModel == null)
            {
                return NotFound();
            }

            await PopulateViewBagData(id);

            return View(viewModel);
        }

        // POST: InfoBasesLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, InfoBasesListViewModel viewModel, Guid[] selectedInfoBases, Guid[] selectedUsers)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var infoBasesList = _mapper.Map<InfoBasesList>(viewModel);
                infoBasesList.ListId = Guid.NewGuid();

                var infoBasesListToUpdate = await _context.InfoBasesLists
                    .Include(c => c.InfoBaseInfoBasesLists)
                    .ThenInclude(c => c.InfoBase)
                    .Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.Id == id);

                _mapper.Map(infoBasesList, infoBasesListToUpdate);

                try
                {
                    await UpdateInfoBasesAsync(infoBasesListToUpdate, selectedInfoBases);
                    await UpdateUsersAsync(infoBasesListToUpdate, selectedUsers);

                    _context.Update(infoBasesListToUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InfoBasesListExists(infoBasesListToUpdate.Id))
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

            await PopulateViewBagData(id);

            return View(viewModel);
        }

        // GET: InfoBasesLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoBasesList = await _context.InfoBasesLists
                .ProjectTo<InfoBasesListViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (infoBasesList == null)
            {
                return NotFound();
            }

            return View(infoBasesList);
        }

        // POST: InfoBasesLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var infoBasesList = await _context.InfoBasesLists.FirstOrDefaultAsync(c => c.Id == id);

            await ClearUsersInfoBasesListAsync(id);

            _context.InfoBasesLists.Remove(infoBasesList);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool InfoBasesListExists(Guid id)
        {
            return _context.InfoBasesLists.Any(e => e.Id == id);
        }

        private async Task PopulateViewBagData(Guid? Id)
        {
            // Списки информационных баз
            var selectedInfoBases = new List<InfoBaseInfoBasesList>();

            if (Id != null)
                selectedInfoBases = await _context.InfoBaseInfoBasesLists
                    .Where(c => c.InfoBasesListId == Id)
                    .Include(c => c.InfoBase)
                    .ToListAsync();

            PopulateInfoBases(selectedInfoBases);

            await PopulateAllInfoBasesAsync(selectedInfoBases);

            // Списки пользователей
            var selectedUsers = new List<User>();

            if (Id != null)
                selectedUsers = await _context.Users
                    .Where(c => c.InfoBasesListId == Id)
                    .ToListAsync();

            PopulateUsers(selectedUsers);

            await PopulateAllUsersAsync(selectedUsers);
        }
        private void PopulateInfoBases(IEnumerable<InfoBaseInfoBasesList> selectedInfoBases)
        {
            ViewBag.SelectedInfoBases = selectedInfoBases.Select(c => (Id: c.InfoBaseId, c.InfoBase.Name)).ToList();
        }
        private void PopulateUsers(IEnumerable<User> selectedUsers)
        {
            ViewBag.SelectedUsers = selectedUsers.Select(c => (c.Id, c.Name)).ToList();
        }
        private async Task PopulateAllInfoBasesAsync(IEnumerable<InfoBaseInfoBasesList> selectedInfoBases)
        {
            var exceptions = selectedInfoBases.Select(c => c.InfoBaseId).ToList();

            var data = await _context.InfoBases
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            data.RemoveAll(c => exceptions.Contains(c.Id));

            ViewBag.AllInfoBases = new SelectList(data, "Id", "Name");
        }
        private async Task PopulateAllUsersAsync(IEnumerable<User> selectedUsers)
        {
            var exceptions = selectedUsers.Select(c => c.Id).ToList();

            var data = await _context.Users
                .Where(c => c.InfoBasesListId == null)
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            data.RemoveAll(c => exceptions.Contains(c.Id));

            ViewBag.AllUsers = new SelectList(data, "Id", "Name");
        }
        private async Task UpdateInfoBasesAsync(InfoBasesList infoBaseListToUpdate, Guid[] selectedInfoBases)
        {
            if (selectedInfoBases == null)
                return;

            // Удалим не выбранные позиции
            for (int i = 0; i < infoBaseListToUpdate.InfoBaseInfoBasesLists.Count; i++)
            {
                var item = infoBaseListToUpdate.InfoBaseInfoBasesLists[i];

                if (!selectedInfoBases.Contains(item.InfoBaseId))
                {
                    infoBaseListToUpdate.InfoBaseInfoBasesLists.RemoveAt(i);
                }
            }

            // Добавим новые позиции и обновим 
            foreach (var selectedId in selectedInfoBases)
            {
                var infoBaseInfoBasesList = infoBaseListToUpdate.InfoBaseInfoBasesLists.FirstOrDefault(c => c.InfoBaseId == selectedId);

                if (infoBaseInfoBasesList == null)
                {
                    var infoBase = await _context.InfoBases.FindAsync(selectedId);

                    infoBaseInfoBasesList = new InfoBaseInfoBasesList()
                    {
                        InfoBase = infoBase,
                        InfoBaseId = selectedId
                    };

                    infoBaseListToUpdate.InfoBaseInfoBasesLists.Add(infoBaseInfoBasesList);
                }
            }
        }
        private async Task UpdateUsersAsync(InfoBasesList infoBaseListToUpdate, Guid[] selectedUsers)
        {
            if (selectedUsers == null)
                return;

            // Удалим не выбранные позиции
            for (int i = 0; i < infoBaseListToUpdate.Users.Count; i++)
            {
                var item = infoBaseListToUpdate.Users[i];

                if (!selectedUsers.Contains(item.Id))
                {
                    infoBaseListToUpdate.Users.RemoveAt(i);
                }
            }

            // Добавим новые позиции и обновим 
            foreach (var selectedId in selectedUsers)
            {
                var user = infoBaseListToUpdate.Users.FirstOrDefault(c => c.Id == selectedId);

                if (user == null)
                {
                    user = await _context.Users.FindAsync(selectedId);

                    infoBaseListToUpdate.Users.Add(user);
                }
            }
        }
        private async Task ClearUsersInfoBasesListAsync(Guid infoBasesListId)
        {
            var users = await _context.Users.Where(c => c.InfoBasesListId == infoBasesListId).ToListAsync();

            foreach (var item in users)
            {
                item.InfoBasesListId = null;
                item.InfoBasesList = null;
            }

            _context.UpdateRange(users);
        }
    }
}
