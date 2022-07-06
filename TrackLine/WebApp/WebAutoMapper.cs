using AutoMapper;
using WebApp.Mappers;
namespace WebApp;

public class WebAutoMapper
{
    private readonly IMapper _mapper;

    public WebAutoMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    private FooBarMapper? _fooBarMapper;
    public FooBarMapper FooBarMapper => _fooBarMapper ?? new FooBarMapper(_mapper);
}