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
using AdminPanel.ViewModels.InfoBases;
using AutoMapper.QueryableExtensions;
using AdminPanel.ViewModels.InfoBasesLists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdminPanel.Controllers
{
    [Authorize(Policy = "OnlyAdmins")]
    public class InfoBasesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InfoBasesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: InfoBases
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 100;

            var source = _context.InfoBases;
            var count = await source.CountAsync();
            var pagesAmount = (int)Math.Ceiling((double)count / pageSize);

            var viewModel = new InfoBaseIndexViewModel
            {
                Items = await source
                    .OrderBy(c => c.Name)
                    .Skip((pageSize * (page - 1)))
                    .Take(pageSize)
                    .ProjectTo<InfoBaseViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(),
                CurrentPage = page,
                PagesAmount = pagesAmount
            };

            return View(viewModel);
        }

        // GET: InfoBases/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new InfoBaseViewModel();

            await PopulateViewBagData(viewModel.Id);

            return View(viewModel);
        }

        // POST: InfoBases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InfoBaseViewModel viewModel, Guid[] selectedInfoBasesLists)
        {
            EditModelStateForConnectionType(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                var infoBase = _mapper.Map<InfoBase>(viewModel);
                infoBase.Id = Guid.NewGuid();
                infoBase.SetIBasesContent();

                await UpdateInfoBasesListsAsync(infoBase, selectedInfoBasesLists);

                _context.Add(infoBase);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateViewBagData(viewModel.Id);

            return View(viewModel);
        }

        // GET: InfoBases/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _context.InfoBases.ProjectTo<InfoBaseViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(c => c.Id == id);

            if (viewModel == null)
            {
                return NotFound();
            }

            await PopulateViewBagData(viewModel.Id);

            return View(viewModel);
        }

        // POST: InfoBases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, InfoBaseViewModel viewModel, Guid[] selectedInfoBasesLists)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            EditModelStateForConnectionType(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                var infoBase = _mapper.Map<InfoBase>(viewModel);
                var infoBaseToUpdate = await _context.InfoBases
                    .Include(c => c.InfoBaseInfoBasesLists)
                    .ThenInclude(c => c.InfoBasesList)
                    .FirstOrDefaultAsync(c => c.Id == id);

                _mapper.Map(infoBase, infoBaseToUpdate);  
                
                infoBaseToUpdate.SetIBasesContent();

                try
                {
                    await UpdateInfoBasesListsAsync(infoBaseToUpdate, selectedInfoBasesLists);

                    _context.Update(infoBaseToUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InfoBaseExists(infoBaseToUpdate.Id))
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

            await PopulateViewBagData(viewModel.Id);

            return View(viewModel);
        }

        // GET: InfoBases/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoBase = await _context.InfoBases
                .ProjectTo<InfoBaseViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (infoBase == null)
            {
                return NotFound();
            }

            return View(infoBase);
        }

        // POST: InfoBases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var infoBase = await _context.InfoBases.FindAsync(id);
            _context.InfoBases.Remove(infoBase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InfoBaseExists(Guid id)
        {
            return _context.InfoBases.Any(e => e.Id == id);
        }

        private async Task PopulateViewBagData(Guid? Id)
        {
            var selectedInfoBasesLists = new List<InfoBaseInfoBasesList>();

            if (Id != null)
                selectedInfoBasesLists = await _context.InfoBaseInfoBasesLists
                    .Where(c => c.InfoBaseId == Id)
                    .Include(c => c.InfoBasesList)
                    .ToListAsync();

            PopulateInfoBasesLists(selectedInfoBasesLists);

            await PopulateAllInfoBasesListsAsync(selectedInfoBasesLists);
        }
        private void PopulateInfoBasesLists(IEnumerable<InfoBaseInfoBasesList> selectedInfoBasesLists)
        {
            ViewBag.SelectedInfoBasesLists = selectedInfoBasesLists.Select(c => (Id: c.InfoBasesListId, c.InfoBasesList.Name)).ToList();
        }
        private async Task PopulateAllInfoBasesListsAsync(IEnumerable<InfoBaseInfoBasesList> selectedInfoBasesLists)
        {
            var exceptions = selectedInfoBasesLists.Select(c => c.InfoBasesListId).ToList();

            var data = await _context.InfoBasesLists
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            data.RemoveAll(c => exceptions.Contains(c.Id));

            ViewBag.AllInfoBasesLists = new SelectList(data, "Id", "Name");
        }
        private async Task UpdateInfoBasesListsAsync(InfoBase infoBaseToUpdate, Guid[] selectedInfoBasesLists)
        {
            if (selectedInfoBasesLists == null)
                return;

            // Удалим не выбранные позиции
            for (int i = 0; i < infoBaseToUpdate.InfoBaseInfoBasesLists.Count; i++)
            {
                var item = infoBaseToUpdate.InfoBaseInfoBasesLists[i];

                if (!selectedInfoBasesLists.Contains(item.InfoBasesListId))
                {
                    UpdateInfoBasesListId(item.InfoBasesList);

                    infoBaseToUpdate.InfoBaseInfoBasesLists.RemoveAt(i);
                }
            }

            // Добавим новые позиции и обновим 
            foreach (var selectedId in selectedInfoBasesLists)
            {
                var infoBaseInfoBasesList = infoBaseToUpdate.InfoBaseInfoBasesLists.FirstOrDefault(c => c.InfoBasesListId == selectedId);

                if (infoBaseInfoBasesList == null)
                {
                    var infoBasesList = await _context.InfoBasesLists.FindAsync(selectedId);

                    infoBaseInfoBasesList = new InfoBaseInfoBasesList()
                    {
                        InfoBasesList = infoBasesList,
                        InfoBasesListId = selectedId
                    };

                    infoBaseToUpdate.InfoBaseInfoBasesLists.Add(infoBaseInfoBasesList);
                }
            }

            // Обновим внутренние GUID списков для корректной работы веб-сервиса
            foreach (var item in infoBaseToUpdate.InfoBaseInfoBasesLists)
            {
                UpdateInfoBasesListId(item.InfoBasesList);
            }
        }
        private void UpdateInfoBasesListId(InfoBasesList infoBasesList)
        {
            infoBasesList.ListId = Guid.NewGuid();

            _context.Entry(infoBasesList).State = EntityState.Modified;
        }

        private void EditModelStateForConnectionType(InfoBaseViewModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel.ConnectionType == InfoBaseConnectionType.File)
            {
                ModelState.Remove(nameof(InfoBaseViewModel.URL));
                ModelState.Remove(nameof(InfoBaseViewModel.Server));
                ModelState.Remove(nameof(InfoBaseViewModel.InfoBaseName));
            }
            else if (viewModel.ConnectionType == InfoBaseConnectionType.Server)
            {
                ModelState.Remove(nameof(InfoBaseViewModel.URL));
                ModelState.Remove(nameof(InfoBaseViewModel.Path));
            }
            else if (viewModel.ConnectionType == InfoBaseConnectionType.WebServer)
            {
                ModelState.Remove(nameof(InfoBaseViewModel.Path));
                ModelState.Remove(nameof(InfoBaseViewModel.Server));
                ModelState.Remove(nameof(InfoBaseViewModel.InfoBaseName));
            }
        }
    }
}
