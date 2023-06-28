using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;

namespace StudentClass.MVC.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
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
            var result = await _studentService.StudentWithClass();
            var total = result.Count();
            if (!string.IsNullOrEmpty(param.Search!.Value))
            {
                result = result.Where(x => x.Name!.ToLower().Contains(param.Search.Value.ToLower())).ToList();
                total = result.Count();
            }

            if (param.Order !=null && param.Order[0].Column == 0)
            {
                
                result = param.Order[0].Dir == "asc" ? result.OrderBy(c => c.Name).ToList() : result.OrderByDescending(c => c.Name).ToList();
            }

            if (param.Length != -1)
            {
                result = result.Skip(param.Start).Take(param.Length).ToList();
            }    

            return Json(new { draw = param.Draw, recordsTotal = total, recordsFiltered = total, data = result });
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Add(StudentViewModel.Student student)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _studentService.Add(student);
                    TempData["msg"] = "success! Add successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return BadRequest();
                }
                
            }    
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var student = await _studentService.GetById(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _studentService.GetById(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            catch (Exception e) 
            { 
                _logger.LogError(e.Message); 
                return NotFound(); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel.Student model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                await _studentService.Update(model);
                TempData["msg"] = "success! Update-successfully";
                return RedirectToAction("Index");
            }
            catch (Exception e) 
            { 
                _logger.LogError(e.Message); 
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var student = await _studentService.GetById(id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            catch (Exception e) 
            { 
                _logger.LogError(e.Message);
                return NotFound(); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCF(int id)
        {
            try
            {
                await _studentService.Delete(id);
                TempData["msg"] = "success! Delete success";
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
