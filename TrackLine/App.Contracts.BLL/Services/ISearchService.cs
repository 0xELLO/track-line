using App.BLL.DTO.List;

namespace App.Contracts.BLL.Services;

public interface ISearchService
{
    public Task<IEnumerable<MinimalListItemDTO>>  SearchExternal(string expression);

    public Task<IEnumerable<MinimalListItemDTO>> SearchLocal(string expression);

    public Task<IEnumerable<MinimalListItemDTO>> Search(string expression);
}