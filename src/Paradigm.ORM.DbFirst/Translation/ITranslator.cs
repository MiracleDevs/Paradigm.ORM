namespace Paradigm.ORM.DbFirst.Translation
{
    public interface ITranslator<in TInput, out TOutput>
    {
        TOutput Translate(TInput input);
    }
}