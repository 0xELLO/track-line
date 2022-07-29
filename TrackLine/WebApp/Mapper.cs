using AutoMapper;
using WebApp.Mappers;
namespace WebApp;

public class Mapper
{
    private readonly IMapper _mapper;

    public Mapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    private FooBarMapper? _fooBarMapper;
    public FooBarMapper FooBarMapper => _fooBarMapper ?? new FooBarMapper(_mapper);
    
    private ExtendedListItemMapper? _extendedListItemMapper;
    public ExtendedListItemMapper ExtendedListItemMapper => _extendedListItemMapper ?? new ExtendedListItemMapper(_mapper);
    
    private MinimalListItemMapper? _minimalListItemMapper;
    public MinimalListItemMapper MinimalListItemMapper => _minimalListItemMapper ?? new MinimalListItemMapper(_mapper);
    
        
    private SubListMapper? _subListMapper;
    public SubListMapper SubListMapper => _subListMapper ?? new SubListMapper(_mapper);
    
        
    private HeadListMapper? _headListMapper;
    public HeadListMapper HeadListMapper => _headListMapper ?? new HeadListMapper(_mapper);
}