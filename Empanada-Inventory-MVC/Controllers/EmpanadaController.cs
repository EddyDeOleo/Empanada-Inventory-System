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
}