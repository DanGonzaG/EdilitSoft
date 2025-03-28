using EdilitSoft.app.Models;
using EdilitSoft.app.Models.BookAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EdilitSoft.app.Controllers
{
    public class BooksApiController : Controller
    {

        private readonly HttpClient _httpClient;

        public BooksApiController (IHttpClientFactory httpClientfactory) 
        {
            _httpClient = httpClientfactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://www.googleapis.com/books/v1");
        }

        // GET: CotizacionesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CotizacionesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CotizacionesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CotizacionesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CotizacionesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CotizacionesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CotizacionesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CotizacionesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task <IActionResult> buscarISBN(string ISBN) 
        {
            var repuestaServer = await _httpClient.GetAsync($"v1/volumes?q=isbn:{ISBN}");
            if (repuestaServer.IsSuccessStatusCode)
            {
                var contenido = await repuestaServer.Content.ReadAsStringAsync();
                var detalles1 = JsonConvert.DeserializeObject<BookVolume>(contenido);              
                return View(detalles1);            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Error");
                return View();
            }

        }
    }
}
