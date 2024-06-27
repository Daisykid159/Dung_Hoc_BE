using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hoc_BE.Data;
using Hoc_BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hoc_BE.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("api/getAllEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _dbContext.Employees.ToListAsync();
                if (employees == null || !employees.Any())
                {
                    return NotFound(new ApiResponse {
                        IsSuccess = false,
                        Message = "No employees found.",
                        Data = { }
                    });
                }
                return Ok(new ApiResponse {
                    IsSuccess = true,
                    Message = "Employees retrieved successfully.",
                    Data = employees
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = { }
                });
            }
        }

        [HttpGet("api/getEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound(new ApiResponse {
                        IsSuccess = false,
                        Message = $"Employee with ID {id} not found.",
                        Data = { }
                    });
                }
                return Ok(new ApiResponse {
                    IsSuccess = true,
                    Message = $"Employee with ID {id} retrieved  successfully.",
                    Data = employee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = { }
                });
            }
        }

        [HttpPost]
        [Route("api/postEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid employee data.",
                        Data = { }
                    });
                }

                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();

                var employees = await _dbContext.Employees.ToListAsync();
                if (employees == null || !employees.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "No employees found.",
                        Data = { }
                    });
                }
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Employee created successfully.",
                    Data = employee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = { }
                });
            }
        }

        [HttpPut("api/UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployeeById(int id, [FromBody] Employee updatedEmployee)
        {
            try
            {
                if (updatedEmployee == null)
                {
                    return BadRequest(new ApiResponse {
                        IsSuccess = false,
                        Message = "Invalid employee data.",
                    });
                }

                var existingEmployee = await _dbContext.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound(new ApiResponse {
                        IsSuccess = false,
                        Message = $"Employee with ID {id} not found."
                    });
                }

                // Update employee properties with values from updatedEmployee
                existingEmployee.id = updatedEmployee.id;
                existingEmployee.name = updatedEmployee.name;
                existingEmployee.email = updatedEmployee.email;

                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse {
                    IsSuccess = true,
                    Message = "Employee updated successfully.",
                    Data = existingEmployee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = { }
                });
            }
        }

        [HttpDelete("api/DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            try
            {
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound(new ApiResponse {
                        IsSuccess = false,
                        Message = $"Employee with ID {id} not found."
                    });
                }

                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();

                var employees = await _dbContext.Employees.ToListAsync();
                if (employees == null || !employees.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "No employees found.",
                        Data = { }
                    });
                }
                
                return Ok(new ApiResponse {
                    IsSuccess = true,
                    Message = $"Employee with ID {id} deleted successfully.",
                    Data = employees,
                });;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = { }
                });
            }
        }
    }
}
