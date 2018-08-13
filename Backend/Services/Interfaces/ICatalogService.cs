using Backend.Models;
using Backend.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface ICatalogService
    {
        Task EditCatalogAsync(string catalogId, Catalog catalog);
        Catalog FindCatalogId(string id);//отличается от GetCatalog. Служит для привязки одного к другому
        Task DeleteCatalog(string id);
        Task DeleteMessage(string id);
        Task CreateCatalogAsync(Catalog catalog);
        Task CreateMessage(Message message);
        List<Catalog> GetRoots();
        Task EditMessage(string messageId, Message message);
        Message GetMessage(string id);
        Catalog GetCatalog(string id);//Возвращает каталог для просмотра (в том числе дочек)
        Catalog GetCatalogWithMessages(string catalogId);
    }
}