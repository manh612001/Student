using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentClass.Application.Interfaces;
using StudentClass.Application.ViewModels;

namespace StudentClass.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        [Route("StudentWithClass")]
        
        //public async Task<IActionResult> StudentWithClass()
        //{
        //    try
        //    {
        //        var result = await _studentService.StudentWithClass();
        //        return Ok(result);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
                
        //    }
            
        //}
        [HttpPost]
        
        public async Task<IActionResult> Add(StudentViewModel.Student model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _studentService.Add(model);
                    return Ok("Add successfully");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                    
                }
            }
            return BadRequest();

        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _studentService.GetById(id);
                    return Ok(result);
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
        public async Task<IActionResult> Update(StudentViewModel.Student model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _studentService.Update(model);
                    return Ok("Update successfully");
                }
                catch (Exception e)
                { 
                    return BadRequest(e.Message); 
                }
            }
            return BadRequest();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.Delete(id);
                    return Ok("Delete successfully");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);

                }
            }
            return BadRequest();

        }
        [HttpGet]
        [Route("Students")]
        public async Task<IActionResult> getStudents()
        {
            try
            {
                var result = await _studentService.GetAll();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }
    }
}
