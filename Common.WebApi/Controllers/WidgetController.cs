using Common.DataContext.Sqlite;
using Common.EntityModels.Sqlite;
using Common.WebApi.Repositories.WidgetRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Common.WebApi.Controllers;

[EnableCors]
[Route("/api/[controller]")]
[ApiController]
public class WidgetController : ControllerBase
{
    private readonly IWidgetRepository _repo;

    public WidgetController(IWidgetRepository repo)
    {
        this._repo = repo;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Widget>))]
    public async Task<IEnumerable<Widget>> GetWidgets(int? id)
    {
        if (id == null)
        {
            return await _repo.RetrieveAllAsync();
        }
        else
        {
            return (await _repo.RetrieveAllAsync()).Where(w => w.Id == id);
        }
    }

    [HttpGet("{id}", Name = nameof(GetWidget))]
    [ProducesResponseType(200, Type = typeof(Widget))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetWidget(int id)
    {
        Widget? w = await _repo.RetrieveAsync(id);
        if (w is null)
        {
            return NotFound();
        }
        return Ok(w);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Widget))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] Widget w)
    {
        if (w is null)
        {
            return BadRequest();
        }
        Widget? addedWidget = await _repo.CreateAsync(w);
        if (addedWidget is null)
        {
            return BadRequest("repository failed to add widget");
        }
        return CreatedAtRoute(
            nameof(GetWidget),
            routeValues: new { id = addedWidget.Id },
            value: addedWidget
        );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] Widget w)
    {
        if (w is null || w.Id != id)
        {
            return BadRequest();
        }
        Widget? existing = await _repo.RetrieveAsync(id);
        if (existing is null)
        {
            return NotFound();
        }
        await _repo.UpdateAsync(id, w);

        return new NoContentResult();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        Widget? existing = await _repo.RetrieveAsync(id);
        if (existing is null)
        {
            return NotFound();
        }
        bool? deleted = await _repo.DeleteAsync(id);
        if (deleted.HasValue && deleted.Value)
        {
            return new NoContentResult();
        }
        else
        {
            return BadRequest($"Widget {id} was found but failed to delete");
        }
    }
}
