﻿using Microsoft.EntityFrameworkCore;
using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.Helpers;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using SANTEGSMS.Reusables;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class AssignmentRepo : IAssignmentRepo
    {
        private readonly AppDbContext _context;

        public AssignmentRepo(AppDbContext context)
        {
            _context = context;
        }

        //-----------------------------------ASSIGNMENTS---------------------------------------------------------------

        public async Task<GenericRespModel> createAssignmentAsync(AssignmentCreationReqModel obj)
        {
            try
            {
                //checks if a assignment with description exists
                var checkAssignment =  _context.Assignments.Where(x => x.Description == obj.Description && x.TeacherId == obj.TeacherId && x.ClassId == obj.ClassId && x.SubjectId == obj.SubjectId
                && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                if (checkAssignment == null)
                {
                    var ass = new Assignments
                    {
                        Description = obj.Description,
                        ObtainableScore = obj.ObtainableScore,
                        FileUrl = obj.FileUrl,
                        SubjectId = obj.SubjectId,
                        TeacherId = obj.TeacherId,
                        ClassId = obj.ClassId,
                        ClassGradeId = obj.ClassGradeId,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        TermId = obj.TermId,
                        SessionId = obj.SessionId,
                        IsActive = true,
                        DueDate = obj.DueDate,
                        DateUploaded = DateTime.Now,
                        LastDateUpdated = DateTime.Now,
                    };

                    await _context.Assignments.AddAsync(ass);
                    await _context.SaveChangesAsync();
                   
                    var result = from asgnmt in _context.Assignments
                                 where asgnmt.Id == ass.Id
                                 select new
                                 {
                                     asgnmt.Id,
                                     asgnmt.Description,
                                     asgnmt.ObtainableScore,
                                     asgnmt.FileUrl,
                                     asgnmt.SchoolSubjects.SubjectName,
                                     asgnmt.TeacherId,
                                     asgnmt.Classes.ClassName,
                                     asgnmt.ClassGrades.GradeName,
                                     asgnmt.SchoolId,
                                     asgnmt.CampusId,
                                     asgnmt.Sessions.SessionName,
                                     asgnmt.Terms.TermName,
                                     asgnmt.IsActive,
                                     asgnmt.DueDate,
                                     asgnmt.DateUploaded,
                                     asgnmt.LastDateUpdated,
                                 };


                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Assignment Created Successfully!", Data = result };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "An Assignment with this Description Already Exists!" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAssignmentByIdAsync(long assignmentId, long schoolId, long campusId)
        {
            try
            {
                var result = from ass in _context.Assignments
                             where ass.Id == assignmentId && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.ObtainableScore,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.IsActive,
                                 ass.DueDate,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
                             };


                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAssignmentBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.Assignments
                             where ass.SubjectId == subjectId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.ObtainableScore,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.IsActive,
                                 ass.DueDate,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
                             };


                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericRespModel> updateAssignmentAsync(long assignmentId, AssignmentCreationReqModel obj)
        {
            try
            {
                var check =  _context.Assignments.Where(x => x.Id == assignmentId).FirstOrDefault();

                if (check != null)
                {
                    //checks if a assignment with description exists
                    var assgnmt =  _context.Assignments.Where(x => x.Description == obj.Description && x.TeacherId == obj.TeacherId && x.ClassId == obj.ClassId && x.SubjectId == obj.SubjectId
                    && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (assgnmt == null)
                    {
                        check.Description = obj.Description;
                        check.ObtainableScore = obj.ObtainableScore;
                        check.FileUrl = obj.FileUrl;
                        check.SubjectId = obj.SubjectId;
                        check.TeacherId = obj.TeacherId;
                        check.ClassId = obj.ClassId;
                        check.ClassGradeId = obj.ClassGradeId;
                        check.SchoolId = obj.SchoolId;
                        check.CampusId = obj.CampusId;
                        check.TermId = obj.TermId;
                        check.SessionId = obj.SessionId;
                        check.IsActive = true;
                        check.DueDate = obj.DueDate;
                        check.LastDateUpdated = DateTime.Now;
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Assignment Updated Successfully!" };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "An Assignment with this Description Already Exists!" };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Assignment with the speicified ID!" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteAssignmentAsync(long assignmentId, long schoolId, long campusId)
        {
            try
            {
                //checks if a assignment exists
                var assgnmt = _context.Assignments.Where(x => x.Id == assignmentId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();

                if (assgnmt != null)
                {
                    _context.Assignments.Remove(assgnmt);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Assignment Deleted Successfully!" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Assignment With the Specified ID!" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        //-----------------------------------------------SUBMIT AND GRADE ASSIGNMENTS-------------------------------------------------

        public async Task<GenericRespModel> submitAssignmentAsync(SubmitAssignmentReqModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(obj.SchoolId);
                long currentTermId = new SessionAndTerm(_context).getCurrentTermId(obj.SchoolId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    if (currentSessionId > 0 && currentTermId > 0)
                    {
                        //get the Student Class and ClassGrade
                        var getStudent = _context.GradeStudents.Where(x => x.StudentId == obj.StudentId && x.SessionId == currentSessionId).FirstOrDefault();

                        //Students Class and ClassGrade
                        long classId = getStudent.ClassId;
                        long classGradeId = getStudent.ClassGradeId;

                        if (getStudent != null)
                        {
                            //checks if assignment has been submitted by student
                            var checkAssignment = _context.AssignmentsSubmitted.Where(x => x.AssignmentId == obj.AssignmentId && x.StudentId == obj.StudentId && x.ClassId == classId
                           && x.ClassGradeId == classGradeId && x.TermId == currentTermId && x.SessionId == currentSessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                            if (checkAssignment == null)
                            {
                                var assSub = new AssignmentsSubmitted
                                {
                                    FileUrl = obj.FileUrl,
                                    ClassId = classId,
                                    ClassGradeId = classGradeId,
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    TermId = currentTermId,
                                    SessionId = currentSessionId,
                                    AssignmentId = obj.AssignmentId,
                                    StudentId = obj.StudentId,
                                    ScoreStatusId = (long)EnumUtility.ScoreStatus.Pending,
                                    DateSubmitted = DateTime.Now,
                                };

                                await _context.AssignmentsSubmitted.AddAsync(assSub);
                                await _context.SaveChangesAsync();

                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Assignment Submitted Successfully!" };

                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "This Assignmnet has been submitted!" };
                        }

                        return new GenericRespModel { StatusCode = 409, StatusMessage = "This Student does not Belong to a Class for the session Specified" };
                    }

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "School Current Academic Session and Term has not been Set" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid School or CampusId" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSubmittedAssignmentByIdAsync(long assignmentSubmittedId, long schoolId, long campusId)
        {
            try
            {
                var result = from ass in _context.AssignmentsSubmitted
                             where ass.Id == assignmentSubmittedId && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.AssignmentId,
                                 ass.Assignments.Description,
                                 ass.StudentId,
                                 ass.Students.FirstName,
                                 ass.Students.LastName,
                                 ass.ObtainableScore,
                                 ass.ScoreObtained,
                                 ass.FileUrl,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.ScoreStatus.ScoreStatusName,
                                 ass.DateSubmitted,
                                 ass.DateGraded
                             };


                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllSubmittedAssignmentsByAssignmentIdAsync(long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.AssignmentsSubmitted
                             where ass.ClassId == classId && ass.ClassGradeId == classGradeId && ass.SchoolId == schoolId && ass.CampusId == campusId
                             && ass.AssignmentId == assignmentId && ass.TermId == termId && ass.SessionId == sessionId
                             select new
                             {
                                 ass.Id,
                                 ass.AssignmentId,
                                 ass.Assignments.Description,
                                 ass.StudentId,
                                 ass.Students.FirstName,
                                 ass.Students.LastName,
                                 ass.ObtainableScore,
                                 ass.ScoreObtained,
                                 ass.FileUrl,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.ScoreStatus.ScoreStatusName,
                                 ass.DateSubmitted,
                                 ass.DateGraded
                             };


                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllSubmittedAssignmentsByStudentIdAndAssignmentIdAsync(Guid studentId, long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.AssignmentsSubmitted
                             where ass.StudentId == studentId && ass.ClassId == classId && ass.ClassGradeId == classGradeId
                             && ass.AssignmentId == assignmentId && ass.SchoolId == schoolId && ass.CampusId == campusId
                             && ass.TermId == termId && ass.SessionId == sessionId
                             select new
                             {
                                 ass.Id,
                                 ass.AssignmentId,
                                 ass.Assignments.Description,
                                 ass.StudentId,
                                 ass.Students.FirstName,
                                 ass.Students.LastName,
                                 ass.ObtainableScore,
                                 ass.ScoreObtained,
                                 ass.FileUrl,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.ScoreStatus.ScoreStatusName,
                                 ass.DateSubmitted,
                                 ass.DateGraded
                             };


                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllUnSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {

                var reslt = from ass in _context.Assignments
                            where !(from asts in _context.AssignmentsSubmitted
                                    where asts.StudentId == studentId && asts.ClassId == classId && asts.ClassGradeId == classGradeId
                                    && asts.SchoolId == schoolId && asts.CampusId == campusId
                                    && asts.TermId == termId && asts.SessionId == sessionId
                                    select asts.AssignmentId).Contains(ass.Id) && ass.ClassId == classId && ass.ClassGradeId == classGradeId
                                     && ass.SchoolId == schoolId && ass.CampusId == campusId
                                     && ass.TermId == termId && ass.SessionId == sessionId
                            select new
                            {
                                ass.Id,
                                ass.Description,
                                ass.ObtainableScore,
                                ass.FileUrl,
                                ass.SchoolSubjects.SubjectName,
                                ass.TeacherId,
                                ass.Classes.ClassName,
                                ass.ClassGrades.GradeName,
                                ass.SchoolId,
                                ass.CampusId,
                                ass.Sessions.SessionName,
                                ass.Terms.TermName,
                                ass.IsActive,
                                ass.DueDate,
                                ass.DateUploaded,
                                ass.LastDateUpdated,
                            };

                if (reslt.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = reslt };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllUnSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                long currentTermId = new SessionAndTerm(_context).getCurrentTermId(schoolId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    if (currentSessionId > 0 && currentTermId > 0)
                    {
                        //get the Student Class and ClassGrade
                        var getStudent = _context.GradeStudents.Where(x => x.StudentId == studentId && x.SessionId == currentSessionId).FirstOrDefault();

                        if (getStudent != null)
                        {
                            //Students Class and ClassGrade
                            long classId = getStudent.ClassId;
                            long classGradeId = getStudent.ClassGradeId;

                            var reslt = from ass in _context.Assignments
                                        where !(from asts in _context.AssignmentsSubmitted
                                                where asts.StudentId == studentId && asts.ClassId == classId && asts.ClassGradeId == classGradeId
                                                && asts.SchoolId == schoolId && asts.CampusId == campusId
                                                && asts.TermId == currentTermId && asts.SessionId == currentSessionId
                                                select asts.AssignmentId).Contains(ass.Id) && ass.ClassId == classId && ass.ClassGradeId == classGradeId
                                                 && ass.SchoolId == schoolId && ass.CampusId == campusId
                                                 && ass.TermId == currentTermId && ass.SessionId == currentSessionId
                                        select new
                                        {
                                            ass.Id,
                                            ass.Description,
                                            ass.ObtainableScore,
                                            ass.FileUrl,
                                            ass.SchoolSubjects.SubjectName,
                                            ass.TeacherId,
                                            ass.Classes.ClassName,
                                            ass.ClassGrades.GradeName,
                                            ass.SchoolId,
                                            ass.CampusId,
                                            ass.Sessions.SessionName,
                                            ass.Terms.TermName,
                                            ass.IsActive,
                                            ass.DueDate,
                                            ass.DateUploaded,
                                            ass.LastDateUpdated,
                                        };

                            if (reslt.Count() > 0)
                            {
                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = reslt };
                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                        }

                        return new GenericRespModel { StatusCode = 400, StatusMessage = "Student has not been Assigned to a Class/ClassGrade!"};
                    }

                    return new GenericRespModel { StatusCode = 400, StatusMessage = "School Current Academic Session and Term has not been Set" };
                }

                return new GenericRespModel { StatusCode = 400, StatusMessage = "No School or Campus with the specified ID!" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                long currentTermId = new SessionAndTerm(_context).getCurrentTermId(schoolId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    if (currentSessionId > 0 && currentTermId > 0)
                    {
                        //get the Student Class and ClassGrade
                        var getStudent = _context.GradeStudents.Where(x => x.StudentId == studentId && x.SessionId == currentSessionId).FirstOrDefault();

                        if (getStudent != null)
                        {
                            //Students Class and ClassGrade
                            long classId = getStudent.ClassId;
                            long classGradeId = getStudent.ClassGradeId;

                            var result = from ass in _context.AssignmentsSubmitted
                                         where ass.StudentId == studentId && ass.ClassId == classId && ass.ClassGradeId == classGradeId
                                         && ass.SchoolId == schoolId && ass.CampusId == campusId
                                         && ass.TermId == currentTermId && ass.SessionId == currentSessionId
                                         select new
                                         {
                                             ass.Id,
                                             SubjectId = ass.Assignments.SchoolSubjects.Id,
                                             ass.Assignments.SchoolSubjects.SubjectName,
                                             ass.AssignmentId,
                                             ass.Assignments.Description,
                                             ass.StudentId,
                                             StudentFullName = ass.Students.FirstName + " " + ass.Students.LastName,
                                             ass.ObtainableScore,
                                             ass.ScoreObtained,
                                             ass.FileUrl,
                                             ass.Classes.ClassName,
                                             ass.ClassGrades.GradeName,
                                             ass.SchoolId,
                                             ass.CampusId,
                                             ass.Sessions.SessionName,
                                             ass.Terms.TermName,
                                             ass.ScoreStatus.ScoreStatusName,
                                             ass.DateSubmitted,
                                             ass.DateGraded
                                         };


                            if (result.Count() > 0)
                            {
                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                        }

                        return new GenericRespModel { StatusCode = 400, StatusMessage = "Student has not been Assigned to a Class/ClassGrade!" };
                    }

                    return new GenericRespModel { StatusCode = 400, StatusMessage = "School Current Academic Session and Term has not been Set" };
                }

                return new GenericRespModel { StatusCode = 400, StatusMessage = "No School or Campus with the specified ID!" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> updateSubmittedAssignmentsAsync(long assignmentSubmittedId, SubmitAssignmentReqModel obj)
        {
            try
            {
                //Validations
                CheckerValidation checker = new CheckerValidation(_context);
                var checkSchool = checker.checkSchoolById(obj.SchoolId);
                var checkCampus = checker.checkSchoolCampusById(obj.CampusId);

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(obj.SchoolId);
                long currentTermId = new SessionAndTerm(_context).getCurrentTermId(obj.SchoolId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    if (currentSessionId > 0 && currentTermId > 0)
                    {
                        //get the Student Class and ClassGrade
                        var getStudent = _context.GradeStudents.Where(x => x.StudentId == obj.StudentId && x.SessionId == currentSessionId).FirstOrDefault();

                        //Students Class and ClassGrade
                        long classId = getStudent.ClassId;
                        long classGradeId = getStudent.ClassGradeId;

                        if (getStudent != null)
                        {
                            //check if the submitted assignment exists
                            var check = _context.AssignmentsSubmitted.Where(a => a.Id == assignmentSubmittedId).FirstOrDefault();

                            if (check != null)
                            {
                                check.FileUrl = obj.FileUrl;
                                check.ClassId = classId;
                                check.ClassGradeId = classGradeId;
                                check.SchoolId = obj.SchoolId;
                                check.CampusId = obj.CampusId;
                                check.TermId = currentTermId;
                                check.SessionId = currentSessionId;
                                check.AssignmentId = obj.AssignmentId;
                                check.StudentId = obj.StudentId;
                                check.ScoreStatusId = (long)EnumUtility.ScoreStatus.Pending;
                                await _context.SaveChangesAsync();

                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Submitted Assignment Updated Successfully!" };
                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "No Submitted Assignment with the specified ID!" };
                        }

                        return new GenericRespModel { StatusCode = 409, StatusMessage = "This Student does not Belong to a Class for the session Specified" };
                    }

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "School Current Academic Session and Term has not been Set" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid School or CampusId" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteSubmittedAssignmentsAsync(long assignmentSubmittedId, long schoolId, long campusId)
        {
            try
            {
                //check if the submitted assignment exists
                var assignmentSubmited = _context.AssignmentsSubmitted.Where(a => a.Id == assignmentSubmittedId).FirstOrDefault();

                if (assignmentSubmited != null)
                {
                    if (assignmentSubmited.IsGraded == false)
                    {
                        _context.AssignmentsSubmitted.Remove(assignmentSubmited);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Submitted Assignment Deleted Successfully!" };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Submitted Assignment has been Graded and Cannot be deleted!" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Submitted Assignment with the specified ID!" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> gradeSubmittedAssignmentsAsync(GradeAssignmentsReqModel obj)
        {
            try
            {
                //check if the submitted assignment exists
                var ass = _context.Assignments.Where(a => a.Id == obj.AssignmentId).FirstOrDefault();
                var respList = new List<GenericRespModel>();

                if (ass != null)
                {
                    //get the obtainable score
                    long obtainableScore = ass.ObtainableScore;
                    
                    foreach (var scr in obj.AssignmentScore)
                    {
                        if (scr.ScoreObtained > obtainableScore)
                        {
                            respList.Add(new GenericRespModel { StatusCode = 409, StatusMessage = $"Score Obtained {scr.ScoreObtained} cannot be greater than Obtainable Score {obtainableScore}"});
                            //return new GenericRespModel { StatusCode = 409, StatusMessage = "Score Obtained cannot be greater than Obtainable Score" };
                        }
                        else
                        {
                            //the assignment submitted obj
                            var assignmentSubmitted =  _context.AssignmentsSubmitted.Where(a => a.Id == scr.AssignmentSubmittedId && a.AssignmentId == ass.Id && a.StudentId == scr.StudentId
                            && a.ClassId == obj.ClassId && a.ClassGradeId == obj.ClassGradeId && a.TermId == obj.TermId && a.SessionId == obj.SessionId
                            && a.SchoolId == obj.SchoolId && a.CampusId == obj.CampusId).FirstOrDefault();

                            if (assignmentSubmitted != null)
                            {
                                assignmentSubmitted.ObtainableScore = obtainableScore;
                                assignmentSubmitted.ScoreObtained = scr.ScoreObtained;
                                assignmentSubmitted.IsGraded = true;
                                assignmentSubmitted.DateGraded = DateTime.Now;
                                if (scr.ScoreObtained >= (obtainableScore / 2)) // divide by 2 to get average pass mark 
                                {
                                    assignmentSubmitted.ScoreStatusId = (long)EnumUtility.ScoreStatus.Passed;
                                }
                                else
                                {
                                    assignmentSubmitted.ScoreStatusId = (long)EnumUtility.ScoreStatus.Failed;
                                }

                                await _context.SaveChangesAsync();
                            }

                            respList.Add(new GenericRespModel { StatusCode = 200, StatusMessage = "Assignment Graded Successfully"});
                        }
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Assignment Graded Successfully", Data = respList };
                }
                else
                {
                    return new GenericRespModel { StatusCode = 400, StatusMessage = "No Assignment with the specified ID!" };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSubmittedAssignmentsBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkSession == true && checkTerm == true)
                {
                    var checksubject = _context.SchoolSubjects.Where(x => x.Id == subjectId).FirstOrDefault();

                    if (checksubject != null)
                    {
                        var result = from ass in _context.Assignments.Include(c => c.AssignmentsSubmitted).AsNoTracking()
                                    where (from asts in _context.AssignmentsSubmitted
                                            where asts.SchoolId == schoolId && asts.CampusId == campusId
                                            && asts.TermId == termId && asts.SessionId == sessionId
                                            select asts.AssignmentId).Contains(ass.Id) && ass.SchoolId == schoolId && ass.CampusId == campusId
                                             && ass.TermId == termId && ass.SessionId == sessionId && ass.SubjectId == subjectId
                                    select new
                                    {
                                        ass.Id,
                                        ass.SubjectId,
                                        ass.Description,
                                        ass.ObtainableScore,
                                        ass.FileUrl,
                                        ass.SchoolSubjects.SubjectName,
                                        ass.TeacherId,
                                        ass.Classes.ClassName,
                                        ass.ClassGrades.GradeName,
                                        ass.SchoolId,
                                        ass.CampusId,
                                        ass.Sessions.SessionName,
                                        ass.Terms.TermName,
                                        ass.IsActive,
                                        ass.DueDate,
                                        ass.DateUploaded,
                                        ass.LastDateUpdated,
                                        AssignmentsSubmitted = ass.AssignmentsSubmitted.Select(sub => 
                                        new 
                                        {
                                            sub.Id,
                                            sub.AssignmentId,
                                            sub.StudentId,
                                            StudentFullName = sub.Students.FirstName + " " + sub.Students.LastName,
                                            sub.ObtainableScore,
                                            sub.ScoreObtained,
                                            sub.FileUrl,
                                            sub.Classes.ClassName,
                                            sub.ClassGrades.GradeName,
                                            sub.SchoolId,
                                            sub.CampusId,
                                            sub.Sessions.SessionName,
                                            sub.Terms.TermName,
                                            sub.ScoreStatus.ScoreStatusName,
                                            sub.DateSubmitted,
                                            sub.DateGraded
                                        }),
                                    };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result};
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                    }

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Subject with the Specified ID" };
                }

               return new GenericRespModel { StatusCode = 409, StatusMessage = "Some Parameters are Invalid" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getUnSubmittedAssignmentsByClassIdAndClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGrade = check.checkClassGradeById(classGradeId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true && checkSession == true && checkTerm == true)
                {
                    var reslt = from ass in _context.Assignments
                                where !(from asts in _context.AssignmentsSubmitted
                                        where asts.ClassId == classId && asts.ClassGradeId == classGradeId
                                        && asts.SchoolId == schoolId && asts.CampusId == campusId
                                        && asts.TermId == termId && asts.SessionId == sessionId
                                        select asts.AssignmentId).Contains(ass.Id) && ass.ClassId == classId && ass.ClassGradeId == classGradeId
                                         && ass.SchoolId == schoolId && ass.CampusId == campusId
                                         && ass.TermId == termId && ass.SessionId == sessionId
                                select new
                                {
                                    ass.Id,
                                    ass.SubjectId,
                                    ass.Description,
                                    ass.ObtainableScore,
                                    ass.FileUrl,
                                    ass.SchoolSubjects.SubjectName,
                                    ass.TeacherId,
                                    ass.Classes.ClassName,
                                    ass.ClassGrades.GradeName,
                                    ass.SchoolId,
                                    ass.CampusId,
                                    ass.Sessions.SessionName,
                                    ass.Terms.TermName,
                                    ass.IsActive,
                                    ass.DueDate,
                                    ass.DateUploaded,
                                    ass.LastDateUpdated,
                                };

                    if (reslt.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = reslt };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericRespModel { StatusCode = 400, StatusMessage = "Some Parameters are Invalid" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSubmittedAssignmentsByClassIdAndClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGrade = check.checkClassGradeById(classGradeId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true && checkSession == true && checkTerm == true)
                {
                    var reslt = from ass in _context.AssignmentsSubmitted.Include(c => c.Assignments).AsNoTracking()
                                where ass.ClassId == classId && ass.ClassGradeId == classGradeId
                                 && ass.SchoolId == schoolId && ass.CampusId == campusId
                                 && ass.TermId == termId && ass.SessionId == sessionId
                                 select new
                                 {
                                     ass.Id,
                                     ass.AssignmentId,
                                     ass.Assignments.Description,
                                     ass.StudentId,
                                     StudentFullName = ass.Students.FirstName + " " + ass.Students.LastName,
                                     ass.ObtainableScore,
                                     ass.ScoreObtained,
                                     ass.FileUrl,
                                     ass.Classes.ClassName,
                                     ass.ClassGrades.GradeName,
                                     ass.SchoolId,
                                     ass.CampusId,
                                     ass.Sessions.SessionName,
                                     ass.Terms.TermName,
                                     ass.ScoreStatus.ScoreStatusName,
                                     ass.DateSubmitted,
                                     ass.DateGraded,
                                     ass.Assignments,
                                 };

                    if (reslt.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = reslt };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericRespModel { StatusCode = 400, StatusMessage = "Some Parameters are Invalid" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.AssignmentsSubmitted
                             where ass.ClassId == classId && ass.ClassGradeId == classGradeId && ass.SchoolId == schoolId && ass.CampusId == campusId
                             && ass.TermId == termId && ass.SessionId == sessionId && ass.StudentId == studentId
                             select new
                             {
                                 ass.Id,
                                 ass.AssignmentId,
                                 ass.Assignments.Description,
                                 ass.StudentId,
                                 ass.Students.FirstName,
                                 ass.Students.LastName,
                                 ass.ObtainableScore,
                                 ass.ScoreObtained,
                                 ass.FileUrl,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.ScoreStatus.ScoreStatusName,
                                 ass.DateSubmitted,
                                 ass.DateGraded
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAssignmentByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.Assignments
                             where ass.TeacherId == teacherId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.ObtainableScore,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.IsActive,
                                 ass.DueDate,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllSubmittedAssignmentsByTeacherIdAsync(Guid teacherId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkSession == true && checkTerm == true)
                {
                    var result = from ass in _context.Assignments.Include(c => c.AssignmentsSubmitted).AsNoTracking()
                                 where ass.SchoolId == schoolId && ass.CampusId == campusId
                                          && ass.TermId == termId && ass.SessionId == sessionId && ass.ClassId == classId
                                          && ass.ClassGradeId == classGradeId && ass.TeacherId == teacherId
                                 select new
                                 {
                                     ass.Id,
                                     ass.SubjectId,
                                     ass.Description,
                                     ass.ObtainableScore,
                                     ass.FileUrl,
                                     ass.SchoolSubjects.SubjectName,
                                     ass.TeacherId,
                                     ass.Classes.ClassName,
                                     ass.ClassGrades.GradeName,
                                     ass.SchoolId,
                                     ass.CampusId,
                                     ass.Sessions.SessionName,
                                     ass.Terms.TermName,
                                     ass.IsActive,
                                     ass.DueDate,
                                     ass.DateUploaded,
                                     ass.LastDateUpdated,
                                     AssignmentsSubmitted = ass.AssignmentsSubmitted.Select(sub =>
                                     new
                                     {
                                         sub.Id,
                                         sub.AssignmentId,
                                         sub.StudentId,
                                         StudentFullName = sub.Students.FirstName + " " + sub.Students.LastName,
                                         sub.ObtainableScore,
                                         sub.ScoreObtained,
                                         sub.FileUrl,
                                         sub.Classes.ClassName,
                                         sub.ClassGrades.GradeName,
                                         sub.SchoolId,
                                         sub.CampusId,
                                         sub.Sessions.SessionName,
                                         sub.Terms.TermName,
                                         sub.ScoreStatus.ScoreStatusName,
                                         sub.DateSubmitted,
                                         sub.DateGraded
                                     }),
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Some Parameters are Invalid" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllSubmittedAssignmentsByTeacherIdAndSubjectIdAsync(Guid teacherId, long subjectId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkSession == true && checkTerm == true)
                {
                    var checksubject = _context.SchoolSubjects.Where(x => x.Id == subjectId).FirstOrDefault();

                    if (checksubject != null)
                    {
                        var result = from ass in _context.Assignments.Include(c => c.AssignmentsSubmitted).AsNoTracking()
                                     where ass.SchoolId == schoolId && ass.CampusId == campusId
                                              && ass.TermId == termId && ass.SessionId == sessionId && ass.ClassId == classId
                                              && ass.ClassGradeId == classGradeId && ass.TeacherId == teacherId && ass.SubjectId == subjectId
                                     select new
                                     {
                                         ass.Id,
                                         ass.SubjectId,
                                         ass.Description,
                                         ass.ObtainableScore,
                                         ass.FileUrl,
                                         ass.SchoolSubjects.SubjectName,
                                         ass.TeacherId,
                                         ass.Classes.ClassName,
                                         ass.ClassGrades.GradeName,
                                         ass.SchoolId,
                                         ass.CampusId,
                                         ass.Sessions.SessionName,
                                         ass.Terms.TermName,
                                         ass.IsActive,
                                         ass.DueDate,
                                         ass.DateUploaded,
                                         ass.LastDateUpdated,
                                         AssignmentsSubmitted = ass.AssignmentsSubmitted.Select(sub =>
                                         new
                                         {
                                             sub.Id,
                                             sub.AssignmentId,
                                             sub.StudentId,
                                             StudentFullName = sub.Students.FirstName + " " + sub.Students.LastName,
                                             sub.ObtainableScore,
                                             sub.ScoreObtained,
                                             sub.FileUrl,
                                             sub.Classes.ClassName,
                                             sub.ClassGrades.GradeName,
                                             sub.SchoolId,
                                             sub.CampusId,
                                             sub.Sessions.SessionName,
                                             sub.Terms.TermName,
                                             sub.ScoreStatus.ScoreStatusName,
                                             sub.DateSubmitted,
                                             sub.DateGraded
                                         }),
                                     };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result };
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                    }

                    return new GenericRespModel { StatusCode = 400, StatusMessage = "No Subject With the Specified ID", };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Some Parameters are Invalid" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}
