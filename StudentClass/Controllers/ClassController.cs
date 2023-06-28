using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;
using StudentClass.Domain;
using System.Net;

namespace StudentClass.MVC.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;
        private readonly ILogger<ClassController> _logger;
        public ClassController(IClassService classService, IStudentService studentService, ILogger<ClassController> logger)
        {
            _classService = classService;
            _studentService = studentService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetData(JqueryDatatableParam param)
        {
            var result = await _classService.GetAll();
            var total = result.Count();
            if (!string.IsNullOrEmpty(param.Search!.Value))
            {
                result = result.Where(x => x.Name!.ToLower().Contains(param.Search.Value.ToLower())).ToList();
                total = result.Count();
            }

            if (param.Order != null && param.Order[0].Column == 0)
            {
                result = param.Order[0].Dir == "asc" ? result.OrderBy(c => c.Name).ToList() : result.OrderByDescending(c => c.Name).ToList();
            }
            if (param.Length != -1)
            {
                result = result.Skip(param.Start).Take(param.Length).ToList();
            }    

            return Json(new { draw = param.Draw, recordsTotal = total, recordsFiltered = total, data = result });
        }
        
        public  async Task<IActionResult> Add()
        {
            return View(new ClassViewModel.CreateClass() { Students = await _studentService.GetAll()});
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClassViewModel.CreateClass model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.Add(model);
                    TempData["msg"] = "success! Add successfully";
                    return RedirectToAction("Add");
                }
                catch (Exception e) 
                { 
                    _logger.LogError(e.Message);
                    return BadRequest();
                }
            }
            return View(new ClassViewModel.CreateClass() { Students = await _studentService.GetAll() });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                return View(await _classService.GetById(id));
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return NotFound();
            }   
        }

        [HttpPost]
        public async Task<IActionResult> Update(ClassViewModel.ClassDetail model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.Update(model);
                    TempData["msg"] = "success! Update successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception e) { 
                    _logger.LogError(e.Message); 
                    return BadRequest();
                }
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return View(await _classService.GetById(id));
            }
            catch (Exception e) 
            { 
                _logger.LogError(e.Message); 
                return BadRequest(); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCF(ClassViewModel.ClassDetail model)
        {
            try
            {
                await _classService.Delete(model.Id);
                TempData["msg"] = "success! Delete successfully";
                return RedirectToAction("Index");
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
                return BadRequest(); 
            }
        }
    }
}
