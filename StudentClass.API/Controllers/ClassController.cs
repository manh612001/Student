using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;

namespace StudentClass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }
        [HttpGet]
        
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _classService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); 
            }
        }
        [HttpPost]
        
        public async Task<IActionResult> Create(ClassViewModel.CreateClass model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _classService.Add(model);
                    return Ok("Add successfully");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                    
                }
            }
            return BadRequest("Chua nhap day du");
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.Delete(id);
                    return Ok("Delete successfully");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);

                }
            }
            return BadRequest();
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(ClassViewModel.ClassDetail model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _classService.Update(model);
                    return Ok("Update successfully");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);

                }
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _classService.GetById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
