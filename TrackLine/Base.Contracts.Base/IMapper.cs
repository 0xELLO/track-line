namespace Base.Contracts.Base;

public interface IMapper<TOut, TIn>
{
    TOut? Map(TIn? entity);
    TIn? Map(TOut? entity);
}