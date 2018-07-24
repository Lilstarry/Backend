﻿using Backend.Models;
using Backend.Models.Entities;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface ICatalogService
    {
        Task EditCatalogAsync(string catalogId, Catalog catalog);
        Catalog FindCatalogId(string id);//отличается от GetCatalog. Служит для привязки одного к другому
        void DeleteCatalog(string id);
        Task CreateCatalogAsync(Catalog catalog);
        Task CreateMessage(Message message);
        Task EditMessage(string messageId, Message message);
        Task DeleteMessage(string id);
        Message GetMessage(string id);
        Catalog GetCatalog(string id);//Возвращает каталог для просмотра (в том числе дочек)
        Catalog GetCatalogWithMessages(string catalogId);
    }
}