using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using OsnovanieService;
using OsnovanieWebApp.Info;
using ILogger = Serilog.ILogger;

namespace OsnovanieWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {

        private readonly ILogger _logger;
        private readonly IMainService _svc;

        public BookController(ILogger logger, IMainService svc)
        {
            _logger = logger;
            _svc = svc;
        }

        [HttpPost]
        [Route("addBook")]
        public ApiResponse AddBook(BookInfo book)
        {
            try
            {
                var bookWS = new Book
                {
                    Title = book.Title,
                    Pages = book.Pages,
                    AuthorId = book.AuthorId ?? 0,
                    IssueDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime((DateTime)book.IssueDate) ?? null

                };
                var result = _svc.AddBook(bookWS);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("addAuthor")]
        public ApiResponse AddAuthor(Author author)
        {
            try
            {
                var result = _svc.AddAuthor(author);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("updateBook")]
        public ApiResponse UpdateBook(Book book)
        {
            try
            {
                var result = _svc.UpdateBook(book);
                return new ApiResponse(result.Result, StatusCodes.Status200OK);
            }
            catch (Exception exc)
            {
                throw new ApiException(exc.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
