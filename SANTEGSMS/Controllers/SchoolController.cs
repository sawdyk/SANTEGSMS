﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SANTEGSMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolRepo _schoolRepo;

        public SchoolController(ISchoolRepo schoolRepo)
        {
            _schoolRepo = schoolRepo;
        }

        [HttpPost("schoolSignUp")]
        [Authorize]
        public async Task<IActionResult> schoolSignUpAsync([FromBody] SchoolSignUpReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.schoolSignUpAsync(obj);

            return Ok(result);
        }

        [HttpPut("activateAccount")]
        [Authorize]
        public async Task<IActionResult> activateAccountAsync([FromBody] ActivateSchoolAccountReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.activateAccountAsync(obj);

            return Ok(result);
        }


        [HttpGet("resendActivationCode")]
        [Authorize]
        public async Task<IActionResult> resendActivationCodeAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.resendActivationCodeAsync(email);

            return Ok(result);
        }

        [HttpPut("updateSchoolDetails")]
        [Authorize]
        public async Task<IActionResult> updateSchoolDetailsAsync(long schoolId, UpdateSchoolDetailsReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.updateSchoolDetailsAsync(schoolId, obj);

            return Ok(result);
        }        
    }
}
