using ClientReportManager.Models;
using ClientReportManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ClientReportManager.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index(string? buscar, int? idEstadoCliente, int? idTipoCliente)
        {
            var modelo = await _clienteService.ObtenerClientesAsync(buscar, idEstadoCliente, idTipoCliente);

            return View(modelo);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        public async Task<IActionResult> Create()
        {
            await CargarCatalogosAsync();

            return View(new Cliente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (await _clienteService.ExisteIdentificacionAsync(cliente.Identificacion))
            {
                ModelState.AddModelError(nameof(cliente.Identificacion), "Ya existe un cliente registrado con esta identificación.");
            }

            if (!ModelState.IsValid)
            {
                // Los catálogos deben cargarse nuevamente cuando la vista se devuelve con errores.
                await CargarCatalogosAsync();
                return View(cliente);
            }

            await _clienteService.CrearClienteAsync(cliente);

            TempData["SuccessMessage"] = "Cliente registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            await CargarCatalogosAsync();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return BadRequest();
            }

            if (await _clienteService.ExisteIdentificacionAsync(cliente.Identificacion, cliente.IdCliente))
            {
                ModelState.AddModelError(nameof(cliente.Identificacion), "Ya existe otro cliente registrado con esta identificación.");
            }

            if (!ModelState.IsValid)
            {
                // Se recargan los catálogos para conservar los combos del formulario al mostrar errores.
                await CargarCatalogosAsync();
                return View(cliente);
            }

            var actualizado = await _clienteService.ActualizarClienteAsync(cliente);

            if (!actualizado)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Cliente actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var desactivado = await _clienteService.DesactivarClienteAsync(id);

            if (!desactivado)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Cliente desactivado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarCatalogosAsync()
        {
            var estados = await _clienteService.ObtenerEstadosActivosAsync();
            var tipos = await _clienteService.ObtenerTiposActivosAsync();

            ViewBag.EstadosCliente = new SelectList(estados, "IdEstadoCliente", "Nombre");
            ViewBag.TiposCliente = new SelectList(tipos, "IdTipoCliente", "Nombre");
        }
    }
}