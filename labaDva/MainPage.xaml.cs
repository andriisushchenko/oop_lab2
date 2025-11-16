using labaDva.Models;
using labaDva.Services;
using labaDva.Strategies;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Xsl;
using static System.Net.Mime.MediaTypeNames;

namespace labaDva;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    private string xmlFilePath;
    private string xslFilePath;
    private XmlAnalysisContext analysis_context;
    private XmlTransformer xmlTransformer;
    private Dictionary<string, List<string>> attributeValueMap;

    private ObservableCollection<Student> _searchResults;
    private ObservableCollection<string> _availableAttributes;
    private ObservableCollection<string> _availableValues; 
    private string _selectedValue;
    private string _selectedAttribute;
    private string _selectedStrategyName;
    private HtmlWebViewSource _htmlContentSource;

    public ObservableCollection<string> StrategyNames { get; set; }

    public ObservableCollection<Student> SearchResults
    {
        get => _searchResults;
        set { _searchResults = value; OnPropertyChanged(); }
    }
    public ObservableCollection<string> AvailableAttributes
    {
        get => _availableAttributes;
        set { _availableAttributes = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> AvailableValues
    {
        get => _availableValues;
        set { _availableValues = value; OnPropertyChanged(); }
    }

    public string SearchValue
    {
        get => _selectedValue;
        set { _selectedValue = value; OnPropertyChanged(); }
    }
    public string SelectedAttribute
    {
        get => _selectedAttribute;
        set
        {
            _selectedAttribute = value;
            OnPropertyChanged();
            UpdateAvailableValues();
        }
    }
    public string SelectedStrategyName
    {
        get => _selectedStrategyName;
        set { _selectedStrategyName = value; OnPropertyChanged(); }
    }
    public HtmlWebViewSource HtmlContentSource
    {
        get => _htmlContentSource;
        set { _htmlContentSource = value; OnPropertyChanged(); }
    }

    public MainPage()
    {
        InitializeComponent();

        analysis_context = new XmlAnalysisContext();
        xmlTransformer = new XmlTransformer();

        SearchResults = new ObservableCollection<Student>();
        AvailableAttributes = new ObservableCollection<string>();
        AvailableValues = new ObservableCollection<string>(); 
        StrategyNames = new ObservableCollection<string> { "SAX", "DOM", "LINQ" };

        this.BindingContext = this;
    }

    private async void LoadXmlButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var fileResult = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Виберіть XML-файл",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".xml" } },
                    { DevicePlatform.macOS, new[] { "public.xml" } }
                })
            });

            if (fileResult != null)
            {
                xmlFilePath = fileResult.FullPath;
                XmlFileLabel.Text = $"XML: {fileResult.FileName}";

                attributeValueMap = analysis_context.GetAttributeValueMap(xmlFilePath);

                AvailableAttributes.Clear();
                foreach (var attrName in attributeValueMap.Keys)
                {
                    AvailableAttributes.Add(attrName);
                }

                SelectedAttribute = AvailableAttributes.FirstOrDefault(); 

                SearchButton.IsEnabled = true; 
            }
        }
        catch (Exception ex)
        {
            xmlFilePath = null;
            XmlFileLabel.Text = "XML-файл не завантажено";
            await DisplayAlert("Помилка", $"Не вдалося завантажити XML: {ex.Message}", "OK");
        }
    }

    private void UpdateAvailableValues()
    {
        if (string.IsNullOrEmpty(SelectedAttribute) || attributeValueMap == null)
            return;

        if (attributeValueMap.TryGetValue(SelectedAttribute, out var values))
        {
            AvailableValues.Clear();
            foreach (var val in values)
            {
                AvailableValues.Add(val);
            }
            SearchValue = AvailableValues.FirstOrDefault();
        }
    }

    private async void LoadXslButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var fileResult = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Виберіть XSL-файл",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".xsl" } }
                })
            });

            if (fileResult != null)
            {
                var xslt = new XslCompiledTransform();
                xslt.Load(fileResult.FullPath);

                xslFilePath = fileResult.FullPath;
                XslFileLabel.Text = $"XSL: {fileResult.FileName}";
                TransformButton.IsEnabled = true; 
            }
        }
        catch (Exception ex)
        {
            xslFilePath = null;
            XslFileLabel.Text = "XSL-файл не завантажено";
            await DisplayAlert("Помилка", $"Не вдалося завантажити XSL: {ex.Message}", "OK");
        }
    }

    private async void SearchButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(xmlFilePath) || string.IsNullOrEmpty(SelectedStrategyName) ||
            string.IsNullOrEmpty(SelectedAttribute) || string.IsNullOrEmpty(SearchValue))
        {
            await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля для пошуку", "OK");
            return;
        }

        try
        {
            switch (SelectedStrategyName)
            {
                case "SAX":
                    analysis_context.SetStrategy(new SAXStrategy());
                    break;
                case "DOM":
                    analysis_context.SetStrategy(new DOMStrategy());
                    break;
                case "LINQ":
                default:
                    analysis_context.SetStrategy(new LINQStrategy());
                    break;
            }

            var parameters = new SearchParameters
            {
                FilePath = this.xmlFilePath,
                AttributeToSearch = this.SelectedAttribute,
                ValueToSearch = this.SearchValue
            };

            var results = analysis_context.ExecuteSearch(parameters);

            SearchResults.Clear();
            foreach (var student in results)
            {
                SearchResults.Add(student);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка пошуку", $"Сталася помилка: {ex.Message}", "OK");
        }
    }

    private async void TransformButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(xmlFilePath) || string.IsNullOrEmpty(xslFilePath))
        {
            await DisplayAlert("Помилка", "Будь ласка, завантажте XML та XSL файли", "OK");
            return;
        }

        try
        {
            string html = xmlTransformer.Transform(xmlFilePath, xslFilePath);

            HtmlContentSource = new HtmlWebViewSource { Html = html };
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка трансформації", $"Сталася помилка: {ex.Message}", "OK");
        }
    }

    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        SearchResults.Clear();
        AvailableAttributes.Clear();
        AvailableValues.Clear(); 
        HtmlContentSource = null;
        SearchValue = string.Empty;
        SelectedAttribute = null;
        SelectedStrategyName = null;

        xmlFilePath = null;
        xslFilePath = null;

        XmlFileLabel.Text = "XML-файл не завантажено";
        XslFileLabel.Text = "XSL-файл не завантажено";
        SearchButton.IsEnabled = false;
        TransformButton.IsEnabled = false;
    }

    private async void ExitButton_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Завершення роботи",
            "Чи дійсно ви хочете завершити роботу з програмою?",
            "Так", "Ні");

        if (answer)
        {
            Microsoft.Maui.Controls.Application.Current.Quit();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}