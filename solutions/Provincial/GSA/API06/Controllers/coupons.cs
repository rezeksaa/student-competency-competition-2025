using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using API06.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API06.Controllers {
    [Route("gsa-api/v1/[controller]")]
    [ApiController]
    public class coupons(GSAContext context) : ControllerBase {

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index() {
            var coupon = await context.Coupons.OrderByDescending(x => x.ExpiryDate).ToListAsync();

            return Ok(new {
                data = coupon
            });
        }

        public class reqBodyCoupon {
            [Required] public string couponCode { get; set; }
            [Required] public decimal discountValue { get; set; }
            [Required] public DateTime expiryDate { get; set; }
            [Required] public int quota { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> post([FromBody] reqBodyCoupon reqBody) {
            if (context.Coupons.Any(x => x.Code == reqBody.couponCode)) {
                return UnprocessableEntity(
                    new {
                        message = "Validation error : Coupon Code must be unique."
                    });
            }

            if (reqBody.quota > 0 == false || reqBody.discountValue > 0 == false) {
                return UnprocessableEntity(
                    new {
                        message = "Validation error : Quota and discount value must be higher than 0."
                    });
            }

            if (reqBody.expiryDate < DateTime.Now) {
                return UnprocessableEntity(
                    new {
                        message = "Validation error : The Expiration date must be greater than today’s date."
                    });
            }

            var coupon = new Coupon {
                Code = reqBody.couponCode,
                DiscountPct = reqBody.discountValue,
                Quota = reqBody.quota,
                ExpiryDate = reqBody.expiryDate,
                CreatedAt = DateTime.Now
            };

            await context.Coupons.AddAsync(coupon);
            await context.SaveChangesAsync();

            return Ok(new {
                message = "Coupon created successfully.",
                data = coupon
            });
        }

        public class reqBodyCouponEdit {
            public string? couponCode { get; set; } = "";
            public decimal? discountValue { get; set; } = null;
            public DateTime? expiryDate { get; set; } = null;
            public int? quota { get; set; } = null;
        }

        [HttpPut("coupons/{couponId}")]
        [Authorize(Roles = "admin")]
        [Consumes("application/json")]
        public async Task<IActionResult> putAsync([FromBody] reqBodyCouponEdit reqBody, int couponId) {
            var coupon = await context.Coupons.FindAsync(couponId);

            if (coupon == null) {
                return NotFound("Coupon not found!");
            }

            if (!String.IsNullOrEmpty(reqBody.couponCode) && coupon.Code != reqBody.couponCode) {

                if (context.Coupons.Any(x => x.Code == reqBody.couponCode)) {
                    return UnprocessableEntity(
                        new {
                            message = "Validation error : Coupon Code must be unique."
                        });
                }

                coupon.Code = reqBody.couponCode;
            }

            if (reqBody.discountValue != null) {
                if (reqBody.discountValue > 0 == false) {
                    return UnprocessableEntity(
                        new {
                            message = "Validation error : discount value must be higher than 0."
                        });
                }

                coupon.DiscountPct = Convert.ToDecimal(reqBody.discountValue);
            }

            if (reqBody.expiryDate != null) {
                if (reqBody.expiryDate < DateTime.Now) {
                    return UnprocessableEntity(
                        new {
                            message = "Validation error : The Expiration date must be greater than today’s date."
                        });
                }

                coupon.ExpiryDate = Convert.ToDateTime(reqBody.expiryDate);
            }

            if (reqBody.quota != null) {
                if (reqBody.quota > 0 == false) {
                    return UnprocessableEntity(
                        new {
                            message = "Validation error : Quota value must be higher than 0."
                        });
                }

                coupon.Quota = Convert.ToInt32(reqBody.quota);
            }

            await context.SaveChangesAsync();

            return Ok(new {
                message = "Coupon updated successfully.",
                data = coupon
            });

        }


    }
}
