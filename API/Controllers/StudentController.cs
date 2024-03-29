using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructue.Data;
using Microsoft.AspNetCore.Mvc;
using static Core.Entities._Enums;

namespace API.Controllers
{
    public class StudentController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork _unitOfWork;
        public StudentController(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _uploadService = uploadService;
            _mapper = mapper;
            
        }
        

        [HttpGet]
        public async Task<ActionResult<Pagination<StudentDto>>> GetStudents(
            [FromQuery] StudentSpecParams studentParams)
        {
            var spec = new StudentSpecification(studentParams);
            var countSpec = new StudentSpecification(studentParams);

            var totalItems = await _unitOfWork.Repository<Student>().CountAsync(countSpec);
            var students = await _unitOfWork.Repository<Student>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<StudentDto>>(students);

            return Ok(new Pagination<StudentDto>(studentParams.PageIndex,
                studentParams.PageSize, totalItems, data));
        }
        

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);

            if (student == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Student, StudentDto>(student);
        }
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent([FromForm] StudentCDto studentCDto)
        {

            var student = _mapper.Map<StudentCDto, Student>(studentCDto);

            student.StudentStatus = StudentStatuses.Pending;
            
            if (studentCDto.Photo != null)
            {
                var uploadFile = await _uploadService.UploadAsync(studentCDto.Photo, "students/photos");

                student.StudentPhotoUrl = "images/students/photos/" + uploadFile.FileName;
            }// Photo Upload

            if (student.AdmissionType == (AdmissionTypes.Direct)) student.IsEvening = true;// evening student check

            _unitOfWork.Repository<Student>().Add(student);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem Creating Student"));

            return Created("test", student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, StudentCDto studentCDto)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);

            if (student == null) return NotFound(new ApiResponse(404));

            _mapper.Map<StudentCDto, Student>(studentCDto, student);

            _unitOfWork.Repository<Student>().Update(student);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem Updating Student"));

            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);

            if (student == null) return NotFound(new ApiResponse(404));

            _unitOfWork.Repository<Student>().Delete(student);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem Deleting Student"));

            return NoContent();
        }

        [HttpPost("Upload/{id}")]
        public async Task<ActionResult> Upload(IFormFile file, int id)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);

            if (student == null) return NotFound(new ApiResponse(404));

            var uploadFile = await _uploadService.UploadAsync(file, "images/students/photos");

            student.StudentPhotoUrl = "images/students/photos/" + uploadFile.FileName;

            _unitOfWork.Repository<Student>().Update(student);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem Updating Product"));
            
            return Ok(new { uploadFile.FileName });
        }
    }
}