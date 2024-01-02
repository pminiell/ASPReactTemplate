using Common.EntityModels.Sqlite;

namespace Common.WebApi.Repositories.WidgetRepository;

public interface IWidgetRepository
{
    Task<Widget?> CreateAsync(Widget w);
    Task<IEnumerable<Widget>> RetrieveAllAsync();

    Task<Widget?> RetrieveAsync(int id);

    Task<Widget?> UpdateAsync(int id, Widget w);

    Task<bool?> DeleteAsync(int id);
}
