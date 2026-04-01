[Route("api/[controller]")]
[ApiController]
public class EmpanadaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmpanadaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Empanada (Leer todos)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Empanada>>> GetEmpanadas()
    {
        return await _context.Empanadas.ToListAsync();
    }

    // POST: api/Empanada (Crear nueva)
    [HttpPost]
    public async Task<ActionResult<Empanada>> PostEmpanada(Empanada empanada)
    {
        _context.Empanadas.Add(empanada);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmpanadas), new { id = empanada.Id }, empanada);
    }

    // GET: Empanada/Edit/5 (Carga la vista con los datos actuales)
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var empanada = await _context.Empanadas.FindAsync(id);
        if (empanada == null) return NotFound();

        return View(empanada);
    }

    // POST: Empanada/Edit/5 (Guarda los cambios en SQL Server)
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Sabor,Precio,CantidadInventario")] Empanada empanada)
    {
        if (id != empanada.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(empanada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(empanada);
    }
}