using Microsoft.AspNetCore.Mvc;
using StudentClass.Interfaces;
using StudentClass.Models;
using StudentClass.ViewModels;

namespace StudentClass.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;
        public StudentController(ILogger<StudentController> logger,IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentService.StudentWithClass();
            return View(students);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost] 
        public async Task<IActionResult> Add(AddStudentViewModel student)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _studentService.Add(student);
                    TempData["alert"] = "alert-success";
                    TempData["msg"] = "Add successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["alert"] = "alert-danger";
                    TempData["msg"] = e.Message;
                    return View(student);
                }
                
            }    
            return View();

        }
        public async Task<IActionResult> Detail(int? id)
        {
            var student = await _studentService.GetDetail(id);
            if(student==null)
            {
                return NotFound();

            }
            return View(student);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _studentService.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AddStudentViewModel student)
        {
            
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _studentService.Update(student);
                TempData["alert"] = "alert-success";
                TempData["msg"] = "Update-successfully";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["alert"] = "alert-danger";
                TempData["msg"] = e.Message;
                return View();
            }
            
        }

        public async Task<IActionResult> Delete(int? id) 
        {
            return View(await _studentService.GetDetail(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCF(int? id)
        {
            try
            {
                await _studentService.Delete(id);
                TempData["alert"] = "alert-success";
                TempData["msg"] = "Delete success";
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
