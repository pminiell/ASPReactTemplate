using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using Common.DataContext.Sqlite;
using Common.EntityModels.Sqlite;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Common.WebApi.Repositories.WidgetRepository;

public class WidgetRepository : IWidgetRepository
{
    private static ConcurrentDictionary<int, Widget>? _widgetCache;

    private WidgetContext _db;

    public WidgetRepository(WidgetContext injectedContext)
    {
        _db = injectedContext;
        if (_widgetCache is null)
        {
            _widgetCache = new ConcurrentDictionary<int, Widget>(
                _db.Widgets!.ToDictionary(w => w.Id)
            );
        }
    }

    public async Task<Widget?> CreateAsync(Widget w)
    {
        //add to db
        EntityEntry<Widget> added = await _db.Widgets!.AddAsync(w);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            //add to cache
            _widgetCache!.TryAdd(w.Id, w);
            return added.Entity;
        }
        else
        {
            return null;
        }
    }

    public async Task<Widget?> UpdateAsync(int id, Widget w)
    {
        //update in db
        _db.Widgets!.Update(w);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            return UpdateCache(id, w);
        }
        return null;
    }

    private Widget UpdateCache(int id, Widget w)
    {
        Widget? old;
        if (_widgetCache is not null)
        {
            if (_widgetCache.TryGetValue(id, out old))
            {
                if (_widgetCache.TryUpdate(id, w, old))
                {
                    return w;
                }
            }
        }
        return null!;
    }

    public Task<IEnumerable<Widget>> RetrieveAllAsync()
    {
        return Task.FromResult(
            _widgetCache is null ? Enumerable.Empty<Widget>() : _widgetCache.Values
        );
    }

    public Task<Widget?> RetrieveAsync(int id)
    {
        return Task.FromResult(_widgetCache is null ? null : _widgetCache.GetValueOrDefault(id));
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        //delete from db
        Widget? w = await _db.Widgets!.FindAsync(id);
        if (w is null)
            return null;
        _db.Widgets!.Remove(w);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            if (_widgetCache is null)
                return null;
            //remove from the cache
            return _widgetCache.TryRemove(id, out w);
        }
        else
        {
            return null;
        }
    }
}
