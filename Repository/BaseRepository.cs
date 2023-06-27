using WeatherForecastRestAPI.Database;
using WeatherForecastRestAPI.Interfaces;
using WeatherForecastRestAPI.Model;

namespace WeatherForecastRestAPI.Repository;

public class BaseRepository<TModel> : IRepository<TModel> where TModel : BaseModel
{
    private readonly ApplicationContext _context;
    
    public BaseRepository(ApplicationContext context) => _context = context;

    public List<TModel> GetAll() => _context.Set<TModel>().ToList();

    public TModel? Get(Guid id) => _context.Set<TModel>().FirstOrDefault(n => n.Id == id);

    public TModel Create(TModel model)
    {
        _context.Set<TModel>().Add(model);
        _context.SaveChanges();
        return model;
    }

    public TModel Update(TModel model)
    {
        var modelToUpdate = _context.Set<TModel>().FirstOrDefault(n => n.Id == model.Id);
        
        if (modelToUpdate != null)
            modelToUpdate = model;
        
        _context.Update(modelToUpdate);
        _context.SaveChanges(); 
        
        return modelToUpdate;
    }

    public void Delete(Guid id)
    {
        var modelToDelete = _context.Set<TModel>().FirstOrDefault(n => n.Id == id);
        _context.Set<TModel>().Remove(modelToDelete);
        _context.SaveChanges();
    }
}   