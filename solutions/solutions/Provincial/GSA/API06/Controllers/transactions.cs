using System.Linq;
using System.Security.Claims;
using API06.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static API06.Controllers.courses;

namespace API06.Controllers {
    [Route("gsa-api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class transactions(GSAContext context) : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> GetAsync(string? courseName, string? sortBy, string? userEmail, int? page, int? size) {

            if (sortBy != null) {
                if (sortBy != "asc" && sortBy != "desc") {
                    return UnprocessableEntity(new {
                        message = "Validation error: sortBy must be 'asc' or 'desc'."
                    });
                }
            }

            if (User.FindFirstValue(ClaimTypes.Role) == "admin") {
                var transaction = context.Purchases.ToList();

                if (!String.IsNullOrEmpty(courseName)) {
                    transaction = transaction.AsEnumerable().Where(x => context.Courses.Find(x.CourseId).Title.ToUpper().Contains(courseName.ToUpper())).ToList();
                }

                if (userEmail != null) {
                    var userId = context.Users.Where(x => x.Email == userEmail).FirstOrDefault().Id;
                    transaction = transaction.AsEnumerable().Where(x =>  userId == x.UserId).ToList();
                }

                if (sortBy != null) {
                    if (sortBy == "asc") {
                        transaction.OrderBy(x => x.PurchasedAt);
                    } else {
                        transaction.OrderByDescending(x => x.PurchasedAt);
                    }
                }

                    var thisPage = page == null || page == 0? 1 : page;
                var thissize = size == null || size == 0? 10 : size;

                var totalPages = transaction.Count / thissize;

                if (totalPages % thissize != 0) {
                    totalPages++;
                }

                if (totalPages == 0 && transaction.Count > 0) {
                    totalPages++;
                }

                var returnThis = transaction.Skip(Convert.ToInt32((thisPage - 1) * thissize)).Take(Convert.ToInt32(thissize)).ToList();

                return Ok(new {
                    data = returnThis,
                    pagination = new {
                        page = thisPage,
                        size = thissize,
                        totalPages = totalPages
                    }
                });

            } else {
                var email = User.FindFirstValue(ClaimTypes.Email);
                var role = User.FindFirstValue(ClaimTypes.Role);

                var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Role == role);

                var transaction = context.Purchases.Where(x => x.UserId == user.Id).ToList();

                if (!String.IsNullOrEmpty(courseName)) {
                    transaction = transaction.AsEnumerable().Where(x => context.Courses.Find(x.CourseId).Title.ToUpper().Contains(courseName.ToUpper())).ToList();
                }

                if (sortBy != null) {
                    if (sortBy == "asc") {
                        transaction.OrderBy(x => x.PurchasedAt);
                    } else {
                        transaction.OrderByDescending(x => x.PurchasedAt);
                    }
                }

                var thisPage = page == null || page == 0 ? 1 : page;
                var thissize = size == null || size == 0 ? 10 : size;

                var totalPages = transaction.Count / thissize;

                if (totalPages % thissize != 0) {
                    totalPages++;
                }

                if (totalPages == 0 && transaction.Count > 0) {
                    totalPages++;
                }

                var returnThis = transaction.Skip(Convert.ToInt32((thisPage - 1) * thissize)).Take(Convert.ToInt32(thissize)).ToList();

                return Ok(new {
                    data = returnThis,
                    pagination = new {
                        page = thisPage,
                        size = thissize,
                        totalPages = totalPages
                    }
                });
            }
        }
    }
}
