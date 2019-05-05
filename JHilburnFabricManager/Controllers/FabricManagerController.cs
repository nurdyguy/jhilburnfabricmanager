using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JHilburnFabricManager.Models;
using JHilburnFabricManager.Services.Contracts;
using Microsoft.AspNetCore.Html;

namespace JHilburnFabricManager.Controllers
{
    public class FabricManagerController : Controller
    {
        private readonly IFabricDataService _fabricDataService;

        public FabricManagerController(IFabricDataService fabricDataService)
        {
            _fabricDataService = fabricDataService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _fabricDataService.Get(1, 10, null, null, null);
            var vm = new FabricManagerViewModel()
            {
                Fabrics = result.Content,
                Page = result.Page,
                PerPage = result.PerPage,
                TotalCount = result.Count,
                SortBy = null,
                SortDir = null
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetFabrics([FromBody]FabricListRequestModel request)
        {
            var result = await _fabricDataService.Get(request.Page, request.PerPage, request.Search, request.SortBy, request.SortDir);

            var vm = new FabricManagerViewModel()
            {
                Fabrics = result.Content,
                
                Page = result.Page,
                PerPage = result.PerPage,
                TotalCount = result.Count,
                SortBy = request.SortBy,
                SortDir = request.SortDir
            };

            return PartialView("_fabricListPartial", vm);
        }

        [HttpGet, Route("/FabricManager/GetDetail/{fabricId:int}")]
        public async Task<IActionResult> GetDetail(int fabricId)
        {
            Fabric fabric;
            if(fabricId == 0)
            {
                fabric = new Fabric();
            }
            else
            {
                fabric = await _fabricDataService.Get(fabricId);
            }
            
            return PartialView("_fabricDetailPartial", fabric);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDetail([FromBody] Fabric fabric)
        {
            if (fabric.id == 0)
            {
                var newFabric = new Fabric()
                {
                    active = fabric.active,
                    category = fabric.category,
                    description = fabric.description,
                    inventory = fabric.inventory,
                    price = fabric.price,
                    sku = fabric.sku
                };

                var result = await _fabricDataService.Create(newFabric);
            }
            else
            {
                var fabToUpdate = await _fabricDataService.Get(fabric.id);

                fabToUpdate.active = fabric.active;
                fabToUpdate.category = fabric.category;
                fabToUpdate.description = fabric.description;
                fabToUpdate.imgUrl = fabric.imgUrl;
                fabToUpdate.inventory = fabric.inventory;
                fabToUpdate.price = fabric.price;
                fabToUpdate.sku = fabric.sku;

                var result = await _fabricDataService.Update(fabToUpdate);
            }
            return Ok();
        }

        [HttpDelete, Route("/FabricManager/Delete/{fabricId:int}")]
        public async Task<IActionResult> Delete(int fabricId)
        {
            var result = await _fabricDataService.Delete(fabricId);
            return Ok();
        }
    }
}
