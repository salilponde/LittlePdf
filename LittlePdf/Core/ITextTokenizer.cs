namespace LittlePdf.Core
{
    public interface ITextTokenizer
    {
        void SetText(string text);
        void Reset();
        string GetNextToken();
    }
}
