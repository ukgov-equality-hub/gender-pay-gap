namespace GenderPayGap.WebUI.Models.ViewReports;

public class UserGeneratedFormattedTextViewModel
{

    private readonly string text;

    public UserGeneratedFormattedTextViewModel(string text)
    {
        this.text = text;
    }

    public List<string> GetParagraphs()
    {
        if (text == null)
        {
            return [];
        }
        
        return text.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries).ToList();
    }

}
