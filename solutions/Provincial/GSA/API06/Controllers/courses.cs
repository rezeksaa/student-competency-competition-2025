using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using API06.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace API06.Controllers {
    [Route("gsa-api/v1/[controller]")]
    [ApiController]
    public class courses(GSAContext context) : ControllerBase {

        public class response {
            [Required] public string message { get; set; }
        }

        public class responseGet {
            [Required] public List<Models.Course> data { get; set; }
        }

        public class Pagination {
            [Required] public int page { get; set; }
            [Required] public int size { get; set; }
            [Required] public int totalPages { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(string? title, string? sort, int? page, int? size) {
            var course = await context.Courses.ToListAsync();

            if (!string.IsNullOrEmpty(title)) {
                course = await context.Courses.Where(x => x.Title.ToUpper().Contains(title.ToUpper())).ToListAsync();
            }

            if ((Convert.ToInt32(page)) < 0) {
                return UnprocessableEntity(new response {
                    message = "Validation error: 'page' must be a positive integer."
                });
            }

            var thisPage = page == null || page == 0 ? 1 : page;
            
            var thisSize = size == null || size == 0 ? 10 : size;

            var courseThisPage = course;

            courseThisPage = courseThisPage.Skip(Convert.ToInt32((thisPage - 1) * thisSize)).Take(Convert.ToInt32(thisSize)).ToList();

            var totalPages = course.Count() / thisSize;

            if (course.Count() % thisSize != 0) {
                totalPages++;
            }

            if (thisSize > course.Count()) {
                totalPages = 1;
            }

            return Ok(
                new {
                    data = new responseGet {
                        data = courseThisPage,
                    },
                    pagination = new Pagination {
                        page = Convert.ToInt32(thisPage),
                        size = Convert.ToInt32(thisSize),
                        totalPages = Convert.ToInt32(totalPages)
                    }

                }
            );

        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetAsync(int courseId) {

            var course = await context.Courses.Include(x => x.Modules).FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null) {
                return NotFound("Course not found.");
            }

            return Ok(course);
        }

        public class PurchaseBody {
            [Required] public string paymentMethod { get; set; }
            public string? couponCode { get; set; }
        }

        [HttpPost("{courseId}/purchase")]
        [Authorize]
        public async Task<IActionResult> purchaseAsync([FromBody] PurchaseBody body, int courseId) {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var role = User.FindFirstValue(ClaimTypes.Role);

            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Role == role);

            if (user == null) {
                return UnprocessableEntity(
                    new response {
                        message = "only student can purchase the course"
                    });
            }

            Coupon code = null;

            if (!String.IsNullOrEmpty(body.couponCode)) {
                code = await context.Coupons.Where(x => x.Code == body.couponCode).FirstOrDefaultAsync();

                if (code == null) {
                    return UnprocessableEntity(
                        new {
                            message = "Validation error: coupon not found."
                        });
                }

                if (DateTime.Now.Date > code.ExpiryDate ||code.Quota <= await context.Purchases.Where(x => x.CouponId == code.Id).CountAsync()) {
                    return UnprocessableEntity(
                        new {
                            message = "Validation error: coupon code has expired or quota exceeded."
                        });
                }
            }

            var course = await context.Courses.FindAsync(courseId);

            if (course == null) {
                return UnprocessableEntity(
                    new {
                        message = "Validation error: course not found."
                    });
            }

            var purchases = new Purchase {
                UserId = user.Id,
                CourseId = courseId,
                CouponId = code == null ? null : code.Id,
                PricePaid = code == null ? course.Price : course.Price - (course.Price * code.DiscountPct / 100),
                PaymentMethod = body.paymentMethod,
                PurchasedAt = DateTime.Now
            };

            if (code != null) {
                code.Quota -= 1;
            }

            await context.Purchases.AddAsync(purchases);
            await context.SaveChangesAsync();

            return Ok(
                new {
                    message = "Course purchased successfully.",
                    data = purchases
                });
            
        }

        public class CoursesReqBody {
            [Required] public string title { get; set; }
            [Required] public string description { get; set; }
            [Required] public decimal price { get; set; }
            [Required] public int duration { get; set; }
            [Required] public List<String> modules { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index([FromBody][Required] CoursesReqBody reqBody) {
            
            if (reqBody.modules.Count < 3) {
                return UnprocessableEntity(new {
                    message = "Validation error: Modules at least 3 data"
                });
            }

            if (reqBody.price > 0 == false || reqBody.duration > 0 == false) {
                return UnprocessableEntity(new {
                    message = "price and duration must be more than 0."
                });
            }

            var course = new Models.Course() {
                Title = reqBody.title,
                Description = reqBody.description,
                Price = reqBody.price,
                Duration = reqBody.duration,
                CreatedAt = DateTime.Now,
            };

            await context.Courses.AddAsync(course);
            await context.SaveChangesAsync();

            int i = 1;

            foreach (var item in reqBody.modules) {
                var modules = new Module {
                    Title = item,
                    CourseId = course.Id,
                    Content = $"This module covers the topic of module {i}: " + item
                };

                await context.Modules.AddAsync(modules);

                i++;
            }

            await context.SaveChangesAsync();

            return Ok(new {
                message = "Course created successfully.",
                data = course
            });
        }

        public class CoursesEditReqBody {
            public string? title { get; set; }
            public string? description { get; set; }
            public decimal? price { get; set; }
            public int? duration { get; set; }
            public List<String>? modules { get; set; }
        }

        [HttpPut("{courseId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> editCourse([FromBody] CoursesEditReqBody reqBody, int courseId) {
            var course = await context.Courses.FindAsync(courseId);

            if (reqBody.title != null) {
                course.Title = reqBody.title;
            }

            if (reqBody.description != null) {
                course.Description = reqBody.description;
            }

            if (reqBody.price != null) {
                if (reqBody.price > 0 == false) {
                    return UnprocessableEntity(new {
                        message = "price must be more than 0."
                    });
                }

                course.Price = Convert.ToDecimal(reqBody.price);
            }

            if (reqBody.duration != null) {
                if (reqBody.duration > 0 == false) {
                    return UnprocessableEntity(new {
                        message = "duration must be more than 0."
                    });
                }

                course.Duration = Convert.ToInt32(reqBody.duration);
            }

            if (reqBody.modules != null && reqBody.modules.Count > 0) {
                if (reqBody.modules.Count < 3) {
                    return UnprocessableEntity(new {
                        message = "Validation error: Modules at least 3 data"
                    });
                }

                var existModule = await context.Modules.Where(x => x.CourseId == courseId).ToListAsync();

                foreach (var module in existModule) {
                    context.Modules.Remove(module);
                }

                await context.SaveChangesAsync();

                int i = 1;

                foreach (var item in reqBody.modules) {
                    var modules = new Module {
                        Title = item,
                        CourseId = course.Id,
                        Content = $"This module covers the topic of module {i}: " + item
                    };

                    await context.Modules.AddAsync(modules);

                    i++;
                }
            }

            course.UpdatedAt = DateTime.Now;
            await context.SaveChangesAsync();

            return Ok(new {
                Message = "Course updated successfully.",
                data = course
            });
        }

    }
}
