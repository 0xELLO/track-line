using App.BLL.DTO.List;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories.List;
using App.DAL.DTO.List;
using Base.Common;
using Base.Contracts.Base;
using IMDbApiLib;

namespace App.BLL.Services;

public class SearchService : ISearchService
{
    private readonly IListItemRepository _listItemRepository;

    public SearchService(IListItemRepository listItemRepository)
    {
        _listItemRepository = listItemRepository;
    }

    public async Task<IEnumerable<MinimalListItemDTO>> Search(string expression)
    {
        // Search expression in imdb
        var externalResult = await SearchExternal(expression);
        
        // Search expression in DB
        var localResult = await SearchLocal(expression);
        
        return externalResult.Concat(localResult);
    }

    public async Task<IEnumerable<MinimalListItemDTO>> SearchExternal(string expression)
    {
        // TODO Move api key to secure place
        // TODO Move instance of apilib to pipeline - get as dependency
        var apiLib = new ApiLib("k_ygxjc76m");
        
        // Search title in imdb
        var data = await apiLib.SearchTitleAsync(expression);
        
        // map result to Model
        var res = data.Results.Select(x => new MinimalListItemDTO
        {
            DefaultTitle = x.Title,
            Code = x.Id,
            TotalLength = 0,
            IsPublic = true,
            IsCreatedByUser = false
        });

        return res;
    }

    public async Task<IEnumerable<MinimalListItemDTO>> SearchLocal(string expression)
    {
        var res = new Dictionary<ListItemDTO, int>();
        
        var listItems = await _listItemRepository.GetAllPublic();
        var input = expression.ToUpper().Split(" ");
        
        foreach (var listItem in listItems)
        {
            foreach (var checkWord in listItem.DefaultTitle.ToUpper().Split(" "))
            {
                var distance = 0;
                foreach (var inputWord in input)
                {
                    distance += Compute(inputWord, checkWord);
                    Console.WriteLine(inputWord + " " +  checkWord + " " + distance);
                }
                
                if (2 >= distance)
                {
                    res.Add(listItem, distance);
                }
            }
        }

        return res.OrderBy(x => x.Value).Take(10).Select(x => new MinimalListItemDTO
        {
            DefaultTitle = x.Key.DefaultTitle,
            Code = x.Key.Code,
            TotalLength = x.Key.TotalLength,
            IsPublic = true,
            IsCreatedByUser = true
        });
    }
    
    private int Compute(string inputWord, string checkWord)
    {
        int n = inputWord.Length;
        int m = checkWord.Length;

        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        var wordMatrix = new int[n + 1, m + 1];

        for (int i = 0; i <= n; i++)
        {
            wordMatrix[i, 0] = i;
        }

        for (int j = 0; j <= m; j++)
        {
            wordMatrix[0, j] = j;
        }

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                if (inputWord[i - 1] == checkWord[j - 1])
                {
                    wordMatrix[i, j] = wordMatrix[i - 1, j - 1];
                }
                else
                {
                    int minimum = int.MaxValue;
                    if ((wordMatrix[i - 1, j]) + 1 < minimum)
                    {
                        minimum = (wordMatrix[i - 1, j]) + 1;
                    }

                    if ((wordMatrix[i, j - 1]) + 1 < minimum)
                    {
                        minimum = (wordMatrix[i, j - 1]) + 1;
                    }

                    if ((wordMatrix[i - 1, j - 1]) + 1 < minimum)
                    {
                        minimum = (wordMatrix[i - 1, j - 1]) + 1;
                    }

                    wordMatrix[i, j] = minimum;
                }
            }
        }

        return wordMatrix[inputWord.Length, checkWord.Length];
    }

}