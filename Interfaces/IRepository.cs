using WeatherForecastRestAPI.Model;

namespace WeatherForecastRestAPI.Interfaces;

public interface IRepository<TModel> where TModel : BaseModel
{
    public List<TModel> GetAll();
    public TModel? Get(Guid id);
    public TModel Create(TModel model);
    public TModel Update(TModel model);
    public void Delete(Guid id);
}