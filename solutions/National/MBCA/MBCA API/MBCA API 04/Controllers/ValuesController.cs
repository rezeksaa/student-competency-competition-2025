using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using MBCA_API_04.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MBCA_API_04.Controllers {
    [Route("api/")]
    [ApiController]
    public class ValuesController(MBCAContext db) : ControllerBase {

        [HttpGet("Prefix")]
        public IActionResult Get() {
            var prefix = db.PhonePrefixes.Select(x => x.Prefix).ToList();

            return Ok(prefix);
        }

        [HttpGet("Events")]
        public IActionResult getEvent() {
            var events = db.Events.ToList();

            return Ok(events.Select(x => new {
                x.Title,
                date = x.Date.ToString("dd-MM-yyyy") + " (" +
                x.StartTime.ToString().Split(':')[0] + ":" + x.StartTime.ToString().Split(':')[1] + " - " +
                x.EndTime.ToString().Split(':')[0] + ":" + x.EndTime.ToString().Split(':')[1] + ")",
                x.Price,
                banner = db.EventBanners.Where(z => z.EventId == x.Id).FirstOrDefault().Banner
            }));
        }

        public class TicketBody {
            public string eventName { get; set; }
            public int qty { get; set; }
            public string promoCode { get; set; }
            public double total { get; set; }
        }

        [HttpPost("Ticket")]
        [Authorize]
        public async Task<IActionResult> postTicket([FromBody] TicketBody body) {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = db.Users.Where(x => x.Username == username).FirstOrDefault();

            var promo = await db.Promos.Where(X => X.Code == body.promoCode).FirstOrDefaultAsync();

            if (body.promoCode.Length != 0 && promo == null) {
                return NotFound("Promo code not found!");
            }

            int? id = 0;

            if (promo == null) {
                id = null;
            } else {
                id = promo.Id;
            }

            var events = db.Events.Where(x => x.Title == body.eventName).FirstOrDefault();

            var ticket = new Ticket {
                EventId = events.Id,
                Qty = body.qty,
                UserId = user.Id,
                TotalPrice = Convert.ToDecimal(body.total),
                TransactionDate = DateTime.Now,
                PromoId = id
            };

            await db.Tickets.AddAsync(ticket);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Ticket")]
        [Authorize]
        public async Task<IActionResult> getTicket() {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = db.Users.Where(x => x.Username == username).FirstOrDefault();

            var ticket = await db.Tickets.Include(x => x.Event).ThenInclude(x => x.EventBanners).Include(x => x.User).Where(X => X.UserId == user.Id).ToListAsync();

            return Ok(ticket.Select(x => new {
                title = x.Event.Title + $" ({x.Qty} pcs)",
                date = x.Event.Date.ToString("dd-MM-yyyy") + " (" +
                x.Event.StartTime.ToString().Split(':')[0] + ":" + x.Event.StartTime.ToString().Split(':')[1] + " - " +
                x.Event.EndTime.ToString().Split(':')[0] + ":" + x.Event.EndTime.ToString().Split(':')[1] + ")",
                id = x.Id.ToString().Length == 1 ? "0000" + x.Id.ToString() :
                x.Id.ToString().Length == 2 ? "000" + x.Id.ToString() :
                x.Id.ToString().Length == 3 ? "00" + x.Id.ToString() :
                x.Id.ToString().Length == 4 ? "0" + x.Id.ToString() :
                x.Id.ToString(),
                banner = db.EventBanners.Where(z => z.EventId == x.Id).FirstOrDefault() == null ? "art_for_adults_1.jpg" : db.EventBanners.Where(z => z.EventId == x.Id).FirstOrDefault().Banner,
            }));
        }

    }
}
