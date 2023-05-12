using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentClass.Interfaces;
using StudentClass.Models;
using StudentClass.ViewModels;

namespace StudentClass.Controllers
{
    public class ClassController : Controller
    {
        private readonly ILogger<ClassController> _logger;
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;
        public ClassController(ILogger<ClassController> logger,IClassService classService, IStudentService studentService)
        {
            _logger = logger;
            _classService = classService;
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _classService.GetAll());
        }

        public async Task<IActionResult> Add()
        {
            List<Student> students = await _studentService.GetAll();
            SelectList selectLists = new SelectList(students, "Id", "Name");
            ViewBag.SelectList = selectLists;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.Add(model);

                    TempData["alert"] = "alert-success";
                    TempData["msg"] = "Add successfully";
                    return RedirectToAction("Add");

                }
                catch (Exception e)
                {
                    TempData["alert"] = "alert-danger";
                    TempData["msg"] = e.Message;
                    return View();
                }



            }
            else
            {
                List<Student> students = await _studentService.GetAll();
                SelectList selectLists = new SelectList(students, "Id", "Name");
                ViewBag.SelectList = selectLists;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            List<Student> students = await _studentService.GetAll();
            SelectList selectLists = new SelectList(students, "Id", "Name");
            ViewBag.SelectList = selectLists;
            return View(await _classService.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(AddClassViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.Update(vm);
                    TempData["alert"] = "alert-success";
                    TempData["msg"] = "Update successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["alert"] = "alert-danger";
                    TempData["msg"] = e.Message;
                    return View();
                }


            }
            else
            {
                TempData["alert"] = "alert-danger";
                TempData["msg"] = "Error";
                return View();
            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _classService.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCF(ListClassViewModel vm)
        {
            try
            {
                await _classService.Delete(vm.Id);
                TempData["alert"] = "alert-success";
                TempData["msg"] = "Delete successfully";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["alert"] = "alert-danger";
                TempData["msg"] = e.Message;
                return RedirectToAction("Index");
                
            }
            

        }

    }
}
